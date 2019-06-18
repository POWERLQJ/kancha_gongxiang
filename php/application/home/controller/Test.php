<?php 
namespace app\home\controller;
use QRcode;
use think\Controller;
use think\Request;
use think\Db;

class Test extends Controller 
{
  public function test()
  {
    $info=[];
    include 'phpqrcode.php';
    $a=rand('1111111111','999999999');
    $data['snid']=$a;
    //入库
    $data=[
      'snid'=>$a,
      'start_time'=>time(),
      'end_time'=>time()+600,
    ];
    Db::name("test")->insert($data);

    $value = $a;         //二维码内容
    $errorCorrectionLevel = 'L';  //容错级别
    $matrixPointSize = 5;      //生成图片大小
  //生成二维码图片
    $filename = 'uploads/test/'.$a.'.png';
    QRcode::png($value,$filename , $errorCorrectionLevel, $matrixPointSize, 2);
    $QR = $filename;        //已经生成的原始二维码图片文件

    $QR = imagecreatefromstring(file_get_contents($QR));
    
    imagepng($QR,'qrcode.png');
    
    imagedestroy($QR);

    $info['qrcode']="http://www.test.com/qrcode.png";
    echo json_encode($info);
  }

}