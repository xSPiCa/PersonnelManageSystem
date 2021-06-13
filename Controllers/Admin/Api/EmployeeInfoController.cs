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
        /// 
        /// </summary>
        /// <returns>返回格式化的员工列表</returns>
        [HttpGet("GetAllStaffInfo")]
    
        public async Task<JsonResult> GetAllStaffInfo(int limit,int offset)
        {

            ReturnResult result =  await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
                
            if (result.Code == Utils.StatusCode.Success)
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
                queryResult.Code = Utils.StatusCode.Success;
                _logger.LogInformation(_controllerName +"成功获取职工列表");
                return new JsonResult(queryResult);
            }
            _logger.LogWarning(_controllerName +"获取职工列表失败");

            return new JsonResult(ReturnResult.Fail(Utils.StatusCode.InValidParameter));
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

            if (queryStaff.Code == Utils.StatusCode.Success)
            {
               ReturnResult queryAuth= await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
               if (queryStaff.Code == Utils.StatusCode.Success && queryAuth.Code == Utils.StatusCode.Success)
               {   // 保证授权用户和被查询用户是同一部门
                   if (((Staff) queryStaff.Data).DepartmentId == ((Staff) queryAuth.Data).DepartmentId)
                   {
                       // 两个实体互相包换会产生循环 序列化时会报错
                       ((Staff) queryStaff.Data).Department = null;
                       
                       _logger.LogInformation(_controllerName +"成功获取职工信息");

                       
                       return new JsonResult(queryStaff);
                   }
               }
            }
            _logger.LogWarning(_controllerName +"获取职工信息失败");
            return new JsonResult(ReturnResult.Fail(Utils.StatusCode.InValidUserInfo));
        }

        [HttpPost("UpdateStaff")]
        public async Task<JsonResult> UpdateStaff([FromBody] Staff staff)
        {

            ReturnResult queryAuth = await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
                if (queryAuth.Code == Utils.StatusCode.Success)
                {
                    var authStaff = (Staff)queryAuth.Data;
                    var tmp = _personnelContext.Department.Single(d => d.DepartmentId == authStaff.DepartmentId);
                    var queryHighAuthority = DepartmentMapper.GetValidDepartment(tmp.Authority,_personnelContext);
 
                    if (queryHighAuthority.Code == Utils.StatusCode.Success)
                    {

                        foreach (var item in (List<Department>)queryHighAuthority.Data)
                        {
                            if (item.DepartmentId == staff.DepartmentId)
                            {
                                var updateResult =await StaffMapper.UpdateStaff(staff, _personnelContext);
                                if (updateResult.Code == Utils.StatusCode.Success)
                                {
                                    _logger.LogInformation(_controllerName +"更新成功");
                                    //如果当前的账号被修改了 我们必须要删除当前授权信息并发送给前端相应的状态码
                                    if (authStaff.StaffId == staff.StaffId)
                                    {
                                        await HttpContext.SignOutAsync();
                                        return new JsonResult(new ReturnResult()
                                        {
                                            Code = Utils.StatusCode.UserHasUpdate,
                                        });
                                    }
                                    else
                                    {
                                        return new JsonResult(ReturnResult.Success());
                                    }
                                }
                            }
                        }
                    }

                
                }
                _logger.LogInformation(_controllerName +"更新失败");

                return new JsonResult(ReturnResult.Fail(Utils.StatusCode.InValidUserInfo));
        }

        [HttpGet("InValidStaff")]
        public async Task<JsonResult> InValidStaff(int id)
        {
            ReturnResult result = await StaffMapper.InValidStaff(id, _personnelContext);
            return new JsonResult(result);
        }

    }
}