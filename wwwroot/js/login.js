//读取 模态框表单中的用户和密码 然后登录
// 你问我为什么不直接提交表单 因为我想不刷新页面完成登录操作而我知道的方法就只有这个

function login() {


    $('#login-cycle')[0].innerHTML= '  <div  class="d-flex justify-content-center">\
                                            <div class="spinner-border" role="status">\
                                                <span class="sr-only">Loading...</span>\
                                            </div>\
                                        </div>';
        
    $.ajax({
        url:"/Api/User/Login",
        type: "Post",
        data:{
            username: $('#login-form')[0]['username'].value,
            password: $('#login-form')[0]['password'].value
        },
        success:function (result) {
            console.log(result);
            if(result['code']==200){
                $(location).attr("href","/");
            }else if(result['code']==10003){
                $('#login-message')[0].innerHTML = result['message'];
            }else{
                $('#login-message')[0].innerHTML = result['message'];
            }
            $('#login-cycle')[0].innerHTML=' ';
        },
        error:function (result){
            console.log(result);
            $('#login-message')[0].innerHTML = "网络错误";
            $('#login-cycle')[0].innerHTML=' ';
        }
    })
}
// 登出
function back(){
    $.ajax({
        url:"/Api/User/Back",
        success:function () {
            $(location).attr("href","/");
        },
        error:function (){
            
        }
    })
}

function launch(){
    $('#login-cycle')[0].innerHTML = ""
    
    $('#login-message')[0].innerHTML = "";
    $('#login-Modal').modal('show');
}

