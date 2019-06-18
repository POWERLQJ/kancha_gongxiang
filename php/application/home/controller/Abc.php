<?php
namespace app\home\controller;
use QRcode;
use think\Controller;
use think\Request;
use think\Db;

Class Abc extends Controller {
	public function index()
	{
		//2019-03-08 12:29:42 测试可以开门成功
		$sn = 453116355;    //控制器的sn
		$doorNO = 1;        //指定门号 默认为1号门
		$server_ip = '47.101.164.199';  //服务器IP
		$server_port = '61005';    //服务器port
		$virtualCardNO = 1234567890; //虚拟卡号

		 $bytes = array();
		 for($i=0;$i<64;$i++)
		 {
		 	$bytes[$i]=0;
		 }
		$bytes[0]=0x37; 
		$bytes[1]=0x40; //2019-03-17 12:47:59 开门指令

		$bytes[4]=$sn& 0xff; //0xff; 
		$sn = $sn>>8;
		$bytes[5]=$sn& 0xff; //0xff; 
		$sn = $sn>>8;
		$bytes[6]=$sn& 0xff; //0xff; 
		$sn = $sn>>8;
		$bytes[7]=$sn& 0xff; //0xff; 

		$bytes[8]=$doorNO; //0x01;   //2019-03-17 12:48:10 1号门

		$bytes[20]=$virtualCardNO & 0xff; //0xff;  //虚拟卡号
		$virtualCardNO = $virtualCardNO>>8;
		$bytes[21]=$virtualCardNO& 0xff; //0xff; 
		$virtualCardNO = $virtualCardNO>>8;
		$bytes[22]=$virtualCardNO& 0xff; //0xff; 
		$virtualCardNO = $virtualCardNO>>8;
		$bytes[23]=$virtualCardNO& 0xff; //0xff; 

		$bytes[28]=0;  //0表示进门  1表示出门
		$bytes[32]=0x5A;  //= 0x5A 不受设备内的权限约束


		//流水号 利用秒
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

		$result = $this->udpGet($this->hexToStr($sendStr),$server_ip,$server_port);//
		echo "recv:\r\n".$this->strToHex($result)."\r\n";

	}

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
	*字符串转十六进制函数
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
		*十六进制转字符串函数
		*@pream string $hex='616263';
		*/ 
	function hexToStr($hex){   
		$str=""; 
		for($i=0;$i<strlen($hex)-1;$i+=2)
		$str.=chr(hexdec($hex[$i].$hex[$i+1]));
		return  $str;
	}
}
?>