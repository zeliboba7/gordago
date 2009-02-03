<?php
/*======================================================================*\
|| #################################################################### ||
|| # LiteUpdate 1.1                                                   # ||
|| # ---------------------------------------------------------------- # ||
|| # Copyright ©2004-2007 Gordago Software Ltd. All Rights Reserved.  # ||
|| # http://www.gordago.com                                           # ||
|| #################################################################### ||
\*======================================================================*/

if (!defined('LU_AREA') AND !defined('THIS_SCRIPT'))
{
	echo 'LU_AREA or THIS_SCRIPT must be defined to continue';
	exit;
}

if (isset($_REQUEST['GLOBALS']) OR isset($_FILES['GLOBALS']))
{
	echo 'Request tainting attempted.';
	exit;
}

if (!defined('CWD'))
{
	define('CWD', (($getcwd = getcwd()) ? $getcwd : '.'));
}

require_once(CWD . '/includes/class_core.php');
$liteupdate =& new LU_Registry();

$liteupdate->fetch_config();

if (CWD == '.')
{
	// getcwd() failed and so we need to be told the full LiteUpdate path in config.php
	if (!empty($liteupdate->config['Misc']['liteupdatepath']))
	{
		define('DIR', $liteupdate->config['Misc']['liteupdatepath']);
	}
	else
	{
		trigger_error('<strong>Configuration</strong>: You must insert a value for <strong>liteupdatepath</strong> in config.php', E_USER_ERROR);
	}
}
else
{
	define('DIR', CWD);
}

?>