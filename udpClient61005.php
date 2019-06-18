<?php
//2019-03-08 12:29:42 ���Կ��Կ��ųɹ�
$sn = 453116355;    //��������sn
$doorNO = 1;        //ָ���ź� Ĭ��Ϊ1����
$server_ip = '47.101.164.199';  //������IP
$server_port = '61005';    //������port
$virtualCardNO = 1234567890; //���⿨��

function udpGet($sendMsg,$ip,$port ){
	$handle = stream_socket_client("udp://{$ip}:{$port}", $errno, $errstr);
	if( !$handle ){
		die("ERROR: {$errno} - {$errstr}\n");
	}
	fwrite($handle, $sendMsg);
	$result = fread($handle, 1024);
	fclose($handle);
	return $result;
}

/**
	*�ַ���תʮ�����ƺ���
	*@pream string $str='abc';
	*/
	//public
	 function strToHex($str){ 
		$hex="";
		for($i=0;$i<strlen($str);$i++)
		{
			 if (strlen(dechex(ord($str[$i])))==1)
			 {
			 	$hex .=  "0";
 			 }
		   $hex.=dechex(ord($str[$i]));
	  }
		$hex=strtoupper($hex);
		return $hex;
	}   
	 
	 
	/**
	*ʮ������ת�ַ�������
	*@pream string $hex='616263';
	*/ 
	 function hexToStr($hex){   
		$str=""; 
		for($i=0;$i<strlen($hex)-1;$i+=2)
		$str.=chr(hexdec($hex[$i].$hex[$i+1]));
		return  $str;
	} 
	
 $bytes = array();
 for($i=0;$i<64;$i++)
 {
 	$bytes[$i]=0;
 }
$bytes[0]=0x37; 
$bytes[1]=0x40; //2019-03-17 12:47:59 ����ָ��

$bytes[4]=$sn& 0xff; //0xff; 
$sn = $sn>>8;
$bytes[5]=$sn& 0xff; //0xff; 
$sn = $sn>>8;
$bytes[6]=$sn& 0xff; //0xff; 
$sn = $sn>>8;
$bytes[7]=$sn& 0xff; //0xff; 

$bytes[8]=$doorNO; //0x01;   //2019-03-17 12:48:10 1����

$bytes[20]=$virtualCardNO & 0xff; //0xff;  //���⿨��
$virtualCardNO = $virtualCardNO>>8;
$bytes[21]=$virtualCardNO& 0xff; //0xff; 
$virtualCardNO = $virtualCardNO>>8;
$bytes[22]=$virtualCardNO& 0xff; //0xff; 
$virtualCardNO = $virtualCardNO>>8;
$bytes[23]=$virtualCardNO& 0xff; //0xff; 

$bytes[28]=0;  //0��ʾ����  1��ʾ����
$bytes[32]=0x5A;  //= 0x5A �����豸�ڵ�Ȩ��Լ��


//��ˮ�� ������
list($usec, $sec) = explode(" ", microtime());  
$bytes[40]=$sec& 0xff;
$sec = $sec>>8;
$bytes[41]=$sec& 0xff;
$sec = $sec>>8;
$bytes[42]=$sec& 0xff;
$sec = $sec>>8;
$bytes[43]=$sec& 0xff;

$sendStr = "";
for($i=0;$i<64;$i++)
{
 if ($bytes[$i]<16)
 {
 	$sendStr .=  "0";
 }
 $sendStr .=   dechex($bytes[$i]);
}

$result = udpGet(hexToStr($sendStr),$server_ip,$server_port);//
echo "recv:\r\n".strToHex($result)."\r\n";

?>