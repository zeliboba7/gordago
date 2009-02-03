<?php
/*======================================================================*\
|| #################################################################### ||
|| # LiteUpdate 1.1                                                   # ||
|| # ---------------------------------------------------------------- # ||
|| # Copyright ©2004-2007 Gordago Software Ltd. All Rights Reserved.  # ||
|| # http://www.gordago.com                                           # ||
|| #################################################################### ||
\*======================================================================*/

define('TYPE_BOOL',     1); // force boolean
define('TYPE_INT',      2); // force integer
define('TYPE_UINT',     3); // force unsigned integer
define('TYPE_NUM',      4); // force number
define('TYPE_UNUM',     5); // force unsigned number
define('TYPE_UNIXTIME', 6); // force unix datestamp (unsigned integer)
define('TYPE_STR',      7); // force trimmed string
define('TYPE_NOTRIM',   8); // force string - no trim
define('TYPE_NOHTML',   9); // force trimmed string with HTML made safe
define('TYPE_ARRAY',   10); // force array
define('TYPE_FILE',    11); // force file

define('TYPE_ARRAY_BOOL',     101);
define('TYPE_ARRAY_INT',      102);
define('TYPE_ARRAY_UINT',     103);
define('TYPE_ARRAY_NUM',      104);
define('TYPE_ARRAY_UNUM',     105);
define('TYPE_ARRAY_UNIXTIME', 106);
define('TYPE_ARRAY_STR',      107);
define('TYPE_ARRAY_NOTRIM',   108);
define('TYPE_ARRAY_NOHTML',   109);
define('TYPE_ARRAY_ARRAY',    110);
define('TYPE_ARRAY_FILE',     11);  // An array of "Files" behaves differently than other <input> arrays. TYPE_FILE handles both types.

define('TYPE_ARRAY_KEYS_INT', 202);

define('TYPE_CONVERT_SINGLE', 100); // value to subtract from array types to convert to single types
define('TYPE_CONVERT_KEYS',   200); // value to subtract from array => keys types to convert to single types

class LU_Input_Cleaner
{
	var $shortvars = array();

	var $superglobal_lookup = array(
	'g' => '_GET',
	'p' => '_POST',
	'r' => '_REQUEST',
	'c' => '_COOKIE',
	's' => '_SERVER',
	'e' => '_ENV',
	'f' => '_FILES'
	);

	var $scriptpath = '';

	var $registry = null;

	function LU_Input_Cleaner(&$registry)
	{
		$this->registry =& $registry;

		if (!is_array($GLOBALS))
		{
			die('<strong>Fatal Error:</strong> Invalid URL.');
		}

		// reverse the effects of magic quotes if necessary
		if (get_magic_quotes_gpc())
		{
			$this->stripslashes_deep($_REQUEST); // needed for some reason (at least on php5 - not tested on php4)
			$this->stripslashes_deep($_GET);
			$this->stripslashes_deep($_POST);
			$this->stripslashes_deep($_COOKIE);

			if (is_array($_FILES))
			{
				foreach ($_FILES AS $key => $val)
				{
					$_FILES["$key"]['tmp_name'] = str_replace('\\', '\\\\', $val['tmp_name']);
				}
				$this->stripslashes_deep($_FILES);
			}
		}
		set_magic_quotes_runtime(0);
		@ini_set('magic_quotes_sybase', 0);

		foreach (array('_GET', '_POST') AS $arrayname)
		{
			if (isset($GLOBALS["$arrayname"]['do']))
			{
				$GLOBALS["$arrayname"]['do'] = trim($GLOBALS["$arrayname"]['do']);
			}

			$this->convert_shortvars($GLOBALS["$arrayname"]);
		}

		// reverse the effects of register_globals if necessary
		if (@ini_get('register_globals') OR !@ini_get('gpc_order'))
		{
			foreach ($this->superglobal_lookup AS $arrayname)
			{
				$registry->superglobal_size["$arrayname"] = sizeof($GLOBALS["$arrayname"]);

				foreach (array_keys($GLOBALS["$arrayname"]) AS $varname)
				{
					// make sure we dont unset any global arrays like _SERVER
					if (!in_array($varname, $this->superglobal_lookup))
					{
						unset($GLOBALS["$varname"]);
					}
				}
			}
		}
		else
		{
			foreach ($this->superglobal_lookup AS $arrayname)
			{
				$registry->superglobal_size["$arrayname"] = sizeof($GLOBALS["$arrayname"]);
			}
		}

		// deal with cookies that may conflict with _GET and _POST data, and create our own _REQUEST with no _COOKIE input
		foreach (array_keys($_COOKIE) AS $varname)
		{
			unset($_REQUEST["$varname"]);
			if (isset($_POST["$varname"]))
			{
				$_REQUEST["$varname"] =& $_POST["$varname"];
			}
			else if (isset($_GET["$varname"]))
			{
				$_REQUEST["$varname"] =& $_GET["$varname"];
			}
		}

		// fetch complete url of current page
		$registry->scriptpath = $this->fetch_scriptpath();
		define('SCRIPTPATH', $registry->scriptpath);

		// fetch url of current page without the variable string
		$quest_pos = strpos($registry->scriptpath, '?');
		if ($quest_pos !== false)
		{
			$registry->script = substr($registry->scriptpath, 0, $quest_pos);
		}
		else
		{
			$registry->script = $registry->scriptpath;
		}
		define('SCRIPT', $registry->script);

		define('SESSION_IDHASH', md5($_SERVER['HTTP_USER_AGENT'] . $registry->alt_ip)); // this should *never* change during a session
		define('SESSION_HOST',   substr($registry->ipaddress, 0, 15));

		// define some useful contants related to environment
		define('USER_AGENT',     $_SERVER['HTTP_USER_AGENT']);
		define('REFERRER',       $_SERVER['HTTP_REFERER']);
	}

	function &clean_array(&$source, $variables)
	{
		$return = array();

		foreach ($variables AS $varname => $vartype)
		{
			$return["$varname"] =& $this->clean($source["$varname"], $vartype, isset($source["$varname"]));
		}

		return $return;
	}

	/**
	* Makes GPC variables safe to use
	*
	* @param	string	Either, g, p, c, r or f (corresponding to get, post, cookie, request and files)
	* @param	array	Array of variable names and types we want to extract from the source array
	*
	* @return	array
	*/
	function clean_array_gpc($source, $variables)
	{
		$sg =& $GLOBALS[$this->superglobal_lookup["$source"]];

		foreach ($variables AS $varname => $vartype)
		{
			if (!isset($this->registry->GPC["$varname"])) // limit variable to only being "cleaned" once to avoid potential corruption
			{
				$this->registry->GPC_exists["$varname"] = isset($sg["$varname"]);
				$this->registry->GPC["$varname"] =& $this->clean(
				$sg["$varname"],
				$vartype,
				isset($sg["$varname"])
				);
			}
		}
	}

	function &clean_gpc($source, $varname, $vartype = TYPE_NOCLEAN)
	{
		if (!isset($this->registry->GPC["$varname"])) // limit variable to only being "cleaned" once to avoid potential corruption
		{
			$sg =& $GLOBALS[$this->superglobal_lookup["$source"]];

			$this->registry->GPC_exists["$varname"] = isset($sg["$varname"]);
			$this->registry->GPC["$varname"] =& $this->clean($sg["$varname"],	$vartype,isset($sg["$varname"]));
		}

		return $this->registry->GPC["$varname"];
	}

	function &clean(&$var, $vartype = TYPE_NOCLEAN, $exists = true)
	{
		if ($exists)
		{
			if ($vartype < TYPE_CONVERT_SINGLE)
			{
				$this->do_clean($var, $vartype);
			}
			else if (is_array($var))
			{
				if ($vartype >= TYPE_CONVERT_KEYS)
				{
					$var = array_keys($var);
					$vartype -=  TYPE_CONVERT_KEYS;
				}
				else
				{
					$vartype -= TYPE_CONVERT_SINGLE;
				}

				foreach (array_keys($var) AS $key)
				{
					$this->do_clean($var["$key"], $vartype);
				}
			}
			else
			{
				$var = array();
			}
			return $var;
		}
		else
		{
			if ($vartype < TYPE_CONVERT_SINGLE)
			{
				switch ($vartype)
				{
					case TYPE_INT:
					case TYPE_UINT:
					case TYPE_NUM:
					case TYPE_UNUM:
					case TYPE_UNIXTIME:
						{
							$var = 0;
							break;
						}
					case TYPE_STR:
					case TYPE_NOHTML:
					case TYPE_NOTRIM:
						{
							$var = '';
							break;
						}
					case TYPE_BOOL:
						{
							$var = 0;
							break;
						}
					case TYPE_ARRAY:
					case TYPE_FILE:
						{
							$var = array();
							break;
						}
					case TYPE_NOCLEAN:
						{
							$var = null;
							break;
						}
					default:
						{
							$var = null;
						}
				}
			}
			else
			{
				$var = array();
			}

			return $var;
		}
	}

	function &do_clean(&$data, $type)
	{
		static $booltypes = array('1', 'yes', 'y', 'true');

		switch ($type)
		{
			case TYPE_INT:    $data = intval($data);                                   break;
			case TYPE_UINT:   $data = ($data = intval($data)) < 0 ? 0 : $data;         break;
			case TYPE_NUM:    $data = strval($data) + 0;                               break;
			case TYPE_UNUM:   $data = strval($data);
											  ($data < 0) ? 0 : $data;                                 break;
			case TYPE_STR:    $data = trim(strval($data));                             break;
			case TYPE_NOTRIM: $data = strval($data);                                   break;
			case TYPE_NOHTML: $data = htmlspecialchars_uni(trim(strval($data)));       break;
			case TYPE_BOOL:   $data = in_array(strtolower($data), $booltypes) ? 1 : 0; break;
			case TYPE_ARRAY:  $data = (is_array($data)) ? $data : array();             break;
			case TYPE_FILE:
				{
					// perhaps redundant :p
					if (is_array($data))
					{
						if (is_array($data['name']))
						{
							$files = count($data['name']);
							for ($index = 0; $index < $files; $index++)
							{
								$data['name']["$index"] = trim(strval($data['name']["$index"]));
								$data['type']["$index"] = trim(strval($data['type']["$index"]));
								$data['tmp_name']["$index"] = trim(strval($data['tmp_name']["$index"]));
								$data['error']["$index"] = intval($data['error']["$index"]);
								$data['size']["$index"] = intval($data['size']["$index"]);
							}
						}
						else
						{
							$data['name'] = trim(strval($data['name']));
							$data['type'] = trim(strval($data['type']));
							$data['tmp_name'] = trim(strval($data['tmp_name']));
							$data['error'] = intval($data['error']);
							$data['size'] = intval($data['size']);
						}
					}
					else
					{
						$data = array(
						'name'     => '',
						'type'     => '',
						'tmp_name' => '',
						'error'    => 0,
						'size'     => 4, // UPLOAD_ERR_NO_FILE
						);
					}
					break;
				}
		}

		return $data;
	}

	function stripslashes_deep(&$value)
	{
		if (is_array($value))
		{
			foreach ($value AS $key => $val)
			{
				if (is_string($val))
				{
					$value["$key"] = stripslashes($val);
				}
				else if (is_array($val))
				{
					$this->stripslashes_deep($value["$key"]);
				}
			}
		}
	}

	function convert_shortvars(&$array)
	{
		// extract long variable names from short variable names
		foreach ($this->shortvars AS $shortname => $longname)
		{
			if (isset($array["$shortname"]) AND !isset($array["$longname"]))
			{
				$array["$longname"] =& $array["$shortname"];
				$GLOBALS['_REQUEST']["$longname"] =& $array["$shortname"];
			}
		}
	}

	function strip_sessionhash(&$string)
	{
		$string = preg_replace('/(s|sessionhash)=[a-z0-9]{32}?&?/', '', $string);
		return $string;
	}


	function fetch_scriptpath()
	{
		if ($this->registry->scriptpath != '')
		{
			return $this->registry->scriptpath;
		}
		else
		{
			if ($_SERVER['REQUEST_URI'] OR $_ENV['REQUEST_URI'])
			{
				$scriptpath = $_SERVER['REQUEST_URI'] ? $_SERVER['REQUEST_URI'] : $_ENV['REQUEST_URI'];
			}
			else
			{
				if ($_SERVER['PATH_INFO'] OR $_ENV['PATH_INFO'])
				{
					$scriptpath = $_SERVER['PATH_INFO'] ? $_SERVER['PATH_INFO'] : $_ENV['PATH_INFO'];
				}
				else if ($_SERVER['REDIRECT_URL'] OR $_ENV['REDIRECT_URL'])
				{
					$scriptpath = $_SERVER['REDIRECT_URL'] ? $_SERVER['REDIRECT_URL'] : $_ENV['REDIRECT_URL'];
				}
				else
				{
					$scriptpath = $_SERVER['PHP_SELF'] ? $_SERVER['PHP_SELF'] : $_ENV['PHP_SELF'];
				}

				if ($_SERVER['QUERY_STRING'] OR $_ENV['QUERY_STRING'])
				{
					$scriptpath .= '?' . ($_SERVER['QUERY_STRING'] ? $_SERVER['QUERY_STRING'] : $_ENV['QUERY_STRING']);
				}
			}

			$scriptpath = $this->strip_sessionhash($scriptpath);

			$this->registry->scriptpath = $scriptpath;

			return $scriptpath;
		}
	}

}

class LU_Registry
{
	var $input; //LU_Input_Cleaner
	var $config;

	function LU_Registry()
	{
		$this->input =& new LU_Input_Cleaner($this);
	}

	function fetch_config()
	{
		$config = array();
		include(CWD . '/includes/config.php');

		if (sizeof($config) == 0)
		{
			if (file_exists(CWD. '/includes/config.php'))
			{
				// config.php exists, but does not define $config
				die('<br /><br /><strong>Configuration</strong>: includes/config.php exists, but is not in the 1.1 format. Please convert your config file via the new config.php.new.');
			}
			else
			{
				die('<br /><br /><strong>Configuration</strong>: includes/config.php does not exist. Please fill out the data in config.php.new and rename it to config.php');
			}
		}

		$this->config =& $config;
	}
}
?>