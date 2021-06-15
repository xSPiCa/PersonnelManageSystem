//为了加快项目速度前端暂时不对返回的状态码进行验证
$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

});

//为了实现不刷新页面的查看编辑删除操作 使用了ajax + html注入 来实现
function ViewById(Id) {
    $.ajax({
        url: "/Api/EmployeeInfo/GetStaffInfo",
        data: { 'id': Id },
        type: "GET",
        success: function (result) {
            //将格式以及后台数据注入模态框
            console.log(result)
            var info = result["data"]
            $('#e-Modal').find('h5[class="modal-title"]')[0].innerHTML = '详情';
            
            $('#e-Modal').find('div[class="modal-body"]')[0].innerHTML = '\n' +
                '<div class="container-fluid">' +
                    '<div class="row">' +
                        '<div class="col-6 font-weight-lighter">工号: '+info['staffId']+'</div> ' +
                        '<div class="col-6 font-weight-lighter">姓名: '+info['name']+'</div>' +
                    '</div>' +
                    '<div class="row">' +
                         '<div class="col-6 font-weight-lighter">职位: ' + info['post'] + '</div> ' +
                         '<div class="col-6 font-weight-lighter">部门号: ' + info['departmentId'] + '</div>' +
                     '</div>' +
                    '<div class="row">' +
                        '<div class="col-6 font-weight-lighter">年龄: ' + info['age'] + '</div> ' +
                        '<div class="col-6 font-weight-lighter">电话: ' + info['phone'] + '</div>' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col-6 font-weight-lighter">入职时间: ' + info['entryTime'] + '</div> ' +
                    '</div>' +
                    '<div class="row">' +
                        '<div class="col font-weight-lighter">地址: ' + info['address'] + '</div> ' +
                    '</div>' +
                '</div>'

            $('#e-Modal').find('div[class="modal-footer"]')[0].innerHTML = '' +
                '<button type = "button" class="btn btn-secondary" data-dismiss="modal" > Close</button>';
        }
    })

    
    $('#e-Modal').modal('show');

}


function EditById(Id) {
    $.ajax({
        url: "/Api/EmployeeInfo/GetStaffInfo",
        data: { 'id': Id },
        type: "GET",
        success: function (result) {
            //将格式以及后台数据注入模态框
            var info = result["data"]
            $('#e-Modal').find('h5[class="modal-title"]')[0].innerHTML = '编辑';

            $('#e-Modal').find('div[class="modal-body"]')[0].innerHTML = '\n' +
                '<form id ="e-form" action="/EmployeeInfo/UpdateStaff" method="post">' +
                '<div class="form-group">'+
                '<label for="e-form-id">工号: </label>' +
                '<input type="hide" id = "e-form-id" class="form-control" name="StaffId" value=' + info["staffId"] + '>' + 
                '</div">' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-name">姓名: </label>' +
                '<input type="text" class="form-control" id="e-form-name" name="Name" value='+info['name']+'>' + 
                '</div>' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-post">职位: </label>' +
                '<input type="text" class="form-control" id="e-form-post" name="Post" value=' + info["post"] + '>' + 
                '</div>' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-sex">性别: </label>' +
                '<select class="form-control" id="e-form-sex" name="Sex">' +
                '\n<option value="男">男</option>'+
                '\n<option value="女" selected>女</option>'+
                '</select>' +
                '</div>' +
                '<div class="form-group">' +
                '<label for="e-form-depart">部门: </label>' +
                '<select class="form-control" id="e-form-depart" name="DepartmentId">' +
                '</select>' +
                '</div>' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-age">年龄: </label>' +
                '<input type="text" class="form-control" id="e-form-age" name="Age" value=' + info["age"] + '>' +
                '</div>' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-phone">电话: </label>' +
                '<input type="text" class="form-control" id="e-form-phone" name="Phone" value=' + info["phone"] + '>' +
                '</div>' +
                '<div class="form-group">' +
                '<label for="e-form-password">密码: </label>' +
                '<input type="text" class="form-control" id="e-form-password" name="Password" value=' + info["password"] + '>' +
                '</div>' +
                '\n' +
                '<div class="form-group">' +
                '<label for="e-form-addr">地址: </label>' +
                '<input type="text" class="form-control" id="e-form-addr" name="Address" value=' + info["address"] + '>' +
                '</div>' +
                '\n' +
                '</form>';
            $('#e-Modal').find('div[class="modal-footer"]')[0].innerHTML ='' +
                '<button type = "button" class="btn btn-secondary" data-dismiss="modal" > Close</button>' +
                ' <button type="button" class="btn btn-secondary" onclick="SubmitEdit()" data-dismiss="modal">Save</button>'

            if(info['sex'] == '男'){
                $('#e-Modal').find('select[id="e-form-sex"]')[0].innerHTML = ''+
                  '<option value="男" selected>男</option>' +
                  '<option value="女">女</option>'
                  
            }
            //注入部门种类 以提供选择 
            $.ajax({
                url: "/Api/DepartmentInfo/GetAllDepartmentInfo",
                success: function (result) {
                    $('#e-Modal').find('select[id="e-form-depart"]')[0].innerHTML = "";
                    
                    result["data"].forEach((e) => {
                        
                        $('#e-Modal').find('select[id="e-form-depart"]')[0].innerHTML +=
                            '\n<option value='+e["departmentId"] +'>'+e["name"]+ '</option>'
                    })
                    $('#e-Modal').find('select[id="e-form-depart"]')[0].value = info["departmentId"];
                }
            })

        }
        
    })
    $('#e-Modal').modal('show');

}

function SubmitEdit() {

    //这个表单提交会受模型绑定验证 如果更新失败了 其实就是因为不满足模型的条件 模型的条件在模型类中以注释的形式给出了 比如密码太短
    //实在是懒得在去 写前端来显示这部分错误内容了
    var formData=new FormData($("#e-form")[0]);
    console.log(formData);
    $.ajax({
        url: "/Api/EmployeeInfo/UpdateStaff",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({
            StaffId: $("#e-form")[0][name = "StaffId"].value,
            Name: $("#e-form")[0][name = "Name"].value,
            Post:parseInt($("#e-form")[0][name = "Post"].value,10),
            DepartmentId: $("#e-form")[0][name = "DepartmentId"].value,
            Age: parseInt($("#e-form")[0][name = "Age"].value,10),
            Phone: $("#e-form")[0][name = "Phone"].value,
            Address: $("#e-form")[0][name = "Address"].value,
            Sex:$("#e-form")[0][name = "Sex"].value,
            Password:$("#e-form")[0][name = "Password"].value,
        }),
        type: "Post",
        dataType: "json",
        success: function(result) {
            
            if(result['code'] == 10007){
                alert("授权信息已更新");
                $(location).attr("href","/");
            }
           alert("更新成功");
            $('#e-table').bootstrapTable('refresh');
        },
        error: function(result) {
            
            alert("失败: "+result['responseJSON']['title']);
        }
    })

}


function DeleById(Id) {
    $.ajax({
        url: "/Api/EmployeeInfo/InValidStaff",
        data: { id: Id },
        type: "get",
        success:function (result) {
            if(result['code'] == 200){
                alert("删除成功")
                $('#e-table').bootstrapTable('refresh');
            }
        }
    })
    console.log(Id + "删除")
}


var TableInit = function () {
    var oTableInit = new Object();

    function actionFormatter(value, row, index) {
        //表格操作列 写入html元素
        return '<a href="#" onclick=ViewById("'+ row.sId+'")> 查看 </a> ' +
            ' <a href="#" onclick = EditById("' + row.sId+'")> 编辑</a> ' +
            '<a href="#" onclick =DeleById("' + row.sId+'")> 删除</a>'
    }
    
    
    //初始化Table
    oTableInit.Init = function () {
        
        //BootstrapTable  设置默认接收返回值json中键为data 的数据来解析 将键为total作为返回数量 
        // 原本的默认值是rows 但为了统一前后端命名规则 修改为了data
        BootstrapTable.DEFAULTS.dataField = "data";
        BootstrapTable.DEFAULTS.totalField = "total";
        $('#e-table').bootstrapTable({
            url: '/Api/EmployeeInfo/GetAllStaffInfo', //请求后台的URL（*）
            method: 'get', //请求方式（*）
            striped: true, //是否显示行间隔色
            cache: false, //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true, //是否显示分页（*）
            queryParams: function(params) {
                var temp = { //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                    limit: params.limit, //页面大小
                    offset: params.offset / params.limit, //页码
                };
                return temp;
            },//传递参数（*）
            sidePagination: "server", //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1, //初始化加载第一页，默认第一页
            pageSize: 20, //每页的记录行数（*）
            search: true, //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true, //是否显示所有的列
            showRefresh: true, //是否显示刷新按钮
            minimumCountColumns: 2, //最少允许的列数
            clickToSelect: true, //是否启用点击选中行
            uniqueId: "sId", //每一行的唯一标识，一般为主键列
            showToggle: true, //是否显示详细视图和列表视图的切换按钮
            cardView: false, //是否显示详细视图
            detailView: false, //是否显示父子表
            columns: [{
                checkbox: true
                },
                {
                    field: 'sId',
                    title: '工号'
                },
                {
                    field: 'dName',
                    title: '部门'
                },
                {
                    field: 'sName',
                    title:'姓名'
                },
                {
                    field: 'post',
                    title:"职位"
                },

                {
                    field: 'age',
                    title: '年龄'
                },
                {
                    field: 'sex',
                    title: '性别'
                },
                {
                    field: 'phone',
                    title:'电话'
                },
                {
                    field: 'sID',
                    title: '操作',
                    formatter:actionFormatter
                }
            ]
        });
    };


    return oTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //初始化页面上面的按钮事件
    };

    return oInit;
};

(function($){
    $.fn.serializeJson=function(){
        var serializeObj={};
        var array=this.serializeArray();
        var str=this.serialize();
        $(array).each(function(){
            if(serializeObj[this.name]){
                if($.isArray(serializeObj[this.name])){
                    serializeObj[this.name].push(this.value);
                }else{
                    serializeObj[this.name]=[serializeObj[this.name],this.value];
                }
            }else{
                serializeObj[this.name]=this.value;
            }
        });
        return serializeObj;
    };
})(jQuery);  