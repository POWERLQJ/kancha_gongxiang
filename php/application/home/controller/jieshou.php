<?php
namespace app\home\controller;
use Think\Controller;
use Think\Log;
class jieshou extends Controller {
//    //接受post参数值
//    public function index(){
//        $aa = $_POST["aa"];
//        $v = $_POST["v"];
//        Log::record("值aa为:".$aa);
//        Log::record("值v为:".$v);
//    }
//    public function test(){
//        echo 11;
//    }
    //接受get参数值
    public function index1(){
        $get1 = $_GET["get1"];
        //$get2 = $_GET["get2"];
        Log::record("值get1为:".$get1);
        //Log::record("值get2为:".$get2);
        dump($get1);
    }
}