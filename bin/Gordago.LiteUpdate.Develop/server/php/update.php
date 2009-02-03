<?php
/*======================================================================*\
|| #################################################################### ||
|| # LiteUpdate 1.1                                                   # ||
|| # ---------------------------------------------------------------- # ||
|| # Copyright ©2004-2007 Gordago Software Ltd. All Rights Reserved.  # ||
|| # http://www.gordago.com                                           # ||
|| #################################################################### ||
\*======================================================================*/

error_reporting(E_ALL & ~E_NOTICE);

define('THIS_SCRIPT', 'check');

require_once('./global.php');

$go = $liteupdate->input->clean_gpc('r', 'go', TYPE_STR);
$product =  $liteupdate->input->clean_gpc('r', 'p', TYPE_STR);

define("OK_MESSAGE", "ok");
define("ERROR_MESSAGE", "error");

if ($product != ''){
	$product = $product."/";
}

if ($go == 'check'){

	$filename = DIR."/files/".$product."lu_version.txt";

	check_file($filename);

	$version_number = file_get_contents($filename);

	echo (OK_MESSAGE."\n".$version_number);

}else {
	$fname =  $liteupdate->input->clean_gpc('r', 'f', TYPE_STR);
	$filename = DIR."/files/".$product.$fname;
	check_file($filename);

	if ($go == 'down'){
		readfile($filename);
	}else if ($go == 'size'){
		echo (OK_MESSAGE."\n".filesize($filename));
	}else{
		echo (ERROR_MESSAGE."\nServer error");
	}
}

function check_file($filename){
	if (file_exists($filename))
	return;

	echo(ERROR_MESSAGE."\nThe file $filename does not exist");
	exit;
}

?>
