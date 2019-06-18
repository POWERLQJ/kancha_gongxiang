<?php
//echo 9223372036854775807;
//echo PHP_INT_MAX;
//echo ((2^32)-1);
$a = 123456789123456789123456789123456789123; //考虑用hex方式
$a = sprintf("\r\n%1.0f\r\n", $a);
echo $a;
//echo dechex(9223372036854775807);
echo dechex(PHP_INT_MAX); //0x7fffffff & 0x7fffffff);
echo dechex(PHP_INT_MIN); //0x7fffffff & 0x7fffffff);
//服务器信息
//$server = 'udp://127.0.0.1:9998';
$server = 'udp://47.100.38.223:61005';
//消息结束符号
$msg_eof = "\n";
$socket = stream_socket_server($server, $errno, $errstr, STREAM_SERVER_BIND);
if (!$socket) {
	die("$errstr ($errno)");
}
 
do {
	//接收客户端发来的信息
	$inMsg = stream_socket_recvfrom($socket, 1024, 0, $peer);
	//服务端打印出相关信息
	echo "Client : $peer\n";
	echo "Receive : {$inMsg}";
	//给客户端发送信息
	$outMsg = substr($inMsg, 0, (strrpos($inMsg, $msg_eof))).' -- '.date("D M j H:i:s Y\r\n");
	stream_socket_sendto($socket, $outMsg, 0, $peer);
	
} while ($inMsg !== false);


?>