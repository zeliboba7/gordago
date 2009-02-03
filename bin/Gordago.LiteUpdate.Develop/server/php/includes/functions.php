<?php
/*======================================================================*\
|| #################################################################### ||
|| # LiteUpdate 1.1                                                   # ||
|| # ---------------------------------------------------------------- # ||
|| # Copyright ©2004-2007 Gordago Software Ltd. All Rights Reserved.  # ||
|| # http://www.gordago.com                                           # ||
|| #################################################################### ||
\*======================================================================*/

if (!function_exists('file_get_contents'))
{
	// use file_get_contents as it will provide improvements for those in 4.3.0 and above
	// but older versions wont notice any difference.
	function file_get_contents($filename)
	{
		$handle = @fopen($filename, 'rb');
		if ($handle)
		{
			do
			{
				$data = fread($handle, 8192);
				if (strlen($data) == 0)
				{
					break;
				}
				$contents .= $data;
			}
			while (true);

			@fclose($handle);
			return $contents;
		}
		return false;
	}
}

?>