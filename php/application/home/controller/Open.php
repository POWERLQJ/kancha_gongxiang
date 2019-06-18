<?php 
namespace app\home\controller;
use QRcode;
use think\Controller;
use think\Request;
use think\Db;
use app\home\controller\Abc;
class Open extends Controller 
{
	public function open()
	{
		$request = Request::instance();
	    $data=[
	    	'snid'=>$request->param('snid',''),
	    	//'end_time'=>time(),
	    ];
	    dump($data);exit;
	    $info= Db::name("test")->where("snid",$data['snid'])->find();
	    
	    if($data['end_time']<$info['end_time']){
	    	$abc=new Abc;
	    	$abc->index();
	    }else{
	    	echo json_encode(['error'=>"二维码错误"],JSON_UNESCAPED_UNICODE);
	    }
	}
	
}

