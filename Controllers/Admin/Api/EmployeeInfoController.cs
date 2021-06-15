using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Admin.Api
{
    
    /// <summary>
    /// 员工管理API
    /// warining: 注意实体之间嵌套造成的序列化死循环
    /// </summary>
    [Route("Api/EmployeeInfo")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [IgnoreAntiforgeryToken]
    public class EmployeeInfoController : Controller
    {

        private readonly String _controllerName = "EmployeeInfoController: ";
        private readonly ILogger<EmployeeInfoController> _logger;
        private readonly PersonnelContext _personnelContext;

        public EmployeeInfoController(ILogger<EmployeeInfoController> logger, PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }
        
        /// <summary>
        /// 通过已授权的信息 查询授权用户部门下的员工
        /// 参数前面可以加[FromQuery] 表示从get方法的参数中获取的 不加默认也是从get方法获取
        /// 我有的地方加有的地方又没加所以提醒一下
        /// </summary>
        /// <returns>返回格式化的员工列表</returns>
        [HttpGet("GetAllStaffInfo")]
    
        public async Task<JsonResult> GetAllStaffInfo(int limit,int offset)
        {

            ReturnResult result =  await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
                
            if (result.Code == Utils.ResultCode.Success)
            {
                ReturnResult queryResult= await StaffMapper.GetSameDepartmentStaff((Staff) result.Data,limit,offset, _personnelContext);

                List<Staff> staves = (List<Staff>) queryResult.Data;
                
                var formatResult = from s in staves
                    select new
                    {
                        dName = s.Department.Name,
                        sName = s.Name,
                        sId=s.StaffId,
                        addr=s.Address,
                        age = s.Age,
                        phone = s.Phone,
                        post = RoleInfo.GetRoleStr(s.Post),
                        sex = s.Sex,
                        dId = s.DepartmentId,
                    };
                queryResult.Data = formatResult;
                queryResult.Code = Utils.ResultCode.Success;
                _logger.LogInformation(_controllerName +"成功获取职工列表");
                return new JsonResult(queryResult);
            }
            _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);

            return new JsonResult(result);
        }
        
        /// <summary>
        /// 使用员工Id 获得员工详细信息
        /// 在获取数据之前必须验证授权信息
        /// </summary>
        /// <param name="id">员工Id</param>
        /// <returns>返回经过格式化后的员工信息</returns>
        [HttpGet("GetStaffInfo")]
        public async Task<JsonResult> GetStaffInfo(int id)
        {
            
            ReturnResult queryStaff=await StaffMapper.GetStaffById(id,_personnelContext);

            if (queryStaff.Code == Utils.ResultCode.Success)
            {
               ReturnResult queryAuth= await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
               if (queryStaff.Code == Utils.ResultCode.Success && queryAuth.Code == Utils.ResultCode.Success)
               {   // 保证授权用户和被查询用户是同一部门
                   if (((Staff) queryStaff.Data).DepartmentId == ((Staff) queryAuth.Data).DepartmentId)
                   {
                       // 两个实体互相包换会产生循环 序列化时会报错
                       ((Staff) queryStaff.Data).Department = null;
                       
                       _logger.LogInformation(_controllerName +"成功获取职工信息");

                       
                       return new JsonResult(queryStaff);
                   }
               }
               else
               {
                   _logger.LogWarning(_controllerName +((Exception)queryAuth.Data).StackTrace);
               }
            }
            else
            {
                _logger.LogWarning(_controllerName +((Exception)queryStaff.Data).StackTrace);
            }
            
            return new JsonResult(ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="staff">模型绑定从Post中获取Staff</param>
        /// <returns></returns>
        [HttpPost("UpdateStaff")]
        public async Task<JsonResult> UpdateStaff([FromBody] Staff staff)
        {

            ReturnResult queryAuth = await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
                if (queryAuth.Code == Utils.ResultCode.Success)
                {
                    var authStaff = (Staff)queryAuth.Data;
                    var tmp = _personnelContext.Department.Single(d => d.DepartmentId == authStaff.DepartmentId);
                    var queryHighAuthority = DepartmentMapper.GetValidDepartment(tmp.Authority,_personnelContext);
 
                    if (queryHighAuthority.Code == Utils.ResultCode.Success)
                    {

                        foreach (var item in (List<Department>)queryHighAuthority.Data)
                        {
                            if (item.DepartmentId == staff.DepartmentId)
                            {
                                var updateResult =await StaffMapper.UpdateStaff(staff, _personnelContext);
                                if (updateResult.Code == Utils.ResultCode.Success)
                                {
                                    _logger.LogInformation(_controllerName +"更新成功");
                                    //如果当前的账号被修改了 我们必须要删除当前授权信息并发送给前端相应的状态码
                                    if (authStaff.StaffId == staff.StaffId)
                                    {
                                        await HttpContext.SignOutAsync();
                                        return new JsonResult(new ReturnResult()
                                        {
                                            Code = Utils.ResultCode.UserHasUpdate,
                                        });
                                    }
                                    else
                                    {
                                        return new JsonResult(ReturnResult.Success());
                                    }
                                }
                                else
                                {
                                    _logger.LogWarning(_controllerName +((Exception)updateResult.Data).StackTrace);
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning(_controllerName +((Exception)queryHighAuthority.Data).StackTrace);
                    }


                }
                _logger.LogInformation(_controllerName +"更新失败");

                return new JsonResult(ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
        }

        [HttpGet("InValidStaff")]
        public async Task<JsonResult> InValidStaff(int id)
        {
            ReturnResult result = await StaffMapper.InValidStaff(id, _personnelContext);
            if (result.Code != ResultCode.Success)
            {
                _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);
            }

            return new JsonResult(result);
        }

    }
}