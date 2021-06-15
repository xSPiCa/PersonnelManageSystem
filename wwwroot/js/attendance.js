//为了加快项目速度前端暂时不对返回的状态码进行验证
$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();
});


var EditById = function (aid) {
    console.log(aid)
}

var DeleById = function (aid) {
    console.log(aid)
}



var TableInit = function () {
    var oTableInit = new Object();



    //初始化Table
    oTableInit.Init = function () {

        function actionFormatter(value, row, index) {
            //表格操作列 写入html元素
            return' <a href="#" onclick = EditById("' + row.attendanceId+'")> 编辑</a> ' +
                '<a href="#" onclick =DeleById("' + row.attendanceId+'")> 删除</a>'
        }

        

        BootstrapTable.DEFAULTS.dataField = "data";
        BootstrapTable.DEFAULTS.totalField = "total";
        $('#e-table').bootstrapTable({
            url: '/Api/AttendanceInfo/GetAllAttendance', //请求后台的URL（*）
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
            uniqueId: "attendanceId", //每一行的唯一标识，一般为主键列
            showToggle: true, //是否显示详细视图和列表视图的切换按钮
            cardView: false, //是否显示详细视图
            detailView: false, //是否显示父子表
            columns: [{
                checkbox: true
            },
                {
                    field: 'attendanceId',
                    title: '考勤号'
                },
                {
                    field: 'staffName',
                    title: '姓名'
                },
                {
                    field: 'startTime',
                    title:'上班时间'
                },
                {
                    field: 'endTime',
                    title:"下班时间"
                },

                {
                    field: 'workStatus',
                    title: '考勤状态'
                },
                {
                    field: 'attendanceId',
                    title: '操作',
                    formatter:actionFormatter
                }
            ]
        });
    };


    return oTableInit;
};

