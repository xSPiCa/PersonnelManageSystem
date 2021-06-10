using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using PersonnelManageSystem.DAL;

namespace PersonnelManageSystem.Controllers
{
    
    /// <summary>
    /// 员工管理API
    /// warining: 注意实体之间嵌套造成的序列化死循环
    /// </summary>
    [Route("Api/EmployeeInfo")]
    [ApiController]
    public class EmployeeInfoController : Controller
    {
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
        public JsonResult GetAllStaffInfo(int limit,int offset)
        {

            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims.First().Type;
                var password = HttpContext.User.Claims.First().Value;

                ReturnResult result = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);

                
                if (result.Code == Utils.StatusCode.Success)
                {
                    ReturnResult queryResult= StaffMapper.GetSameDepartmentStaff((Staff) result.Data,limit,offset, _personnelContext);

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
                    queryResult.Authorization = true;
                    queryResult.Code = Utils.StatusCode.Success;
                    return new JsonResult(queryResult);
                }
            }
            _logger.LogWarning("授权信息失效");
            return new JsonResult(new ReturnResult()
            {
                Authorization = false,
                Code = Utils.StatusCode.InValidUserInfo,
                Data = null,
                Message = "授权失败"
            });
        }
        
        /// <summary>
        /// 使用员工Id 获得员工详细信息
        /// 在获取数据之前必须验证授权信息
        /// </summary>
        /// <param name="id">员工Id</param>
        /// <returns>返回经过格式化后的员工信息</returns>
        [HttpGet("GetStaffInfo")]
        public JsonResult GetStaffInfo(int id)
        {
            
            ReturnResult queryStaff=StaffMapper.GetStaffById(id,_personnelContext);

            if (queryStaff.Code == Utils.StatusCode.Success)
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    var username = HttpContext.User.Claims.First().Type;
                    var password = HttpContext.User.Claims.First().Value;
                    ReturnResult queryAuth = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);
                    
                    if (queryStaff.Code == Utils.StatusCode.Success && queryAuth.Code == Utils.StatusCode.Success)
                    {   // 保证授权用户和被查询用户是同一部门
                        if (((Staff) queryStaff.Data).DepartmentId == ((Staff) queryAuth.Data).DepartmentId)
                        {
                            // 两个实体互相包换会产生循环 序列化时会报错
                            ((Staff) queryStaff.Data).Department = null;
                            return new JsonResult(queryStaff);
                        }
                    }
                }
            }
            return new JsonResult(new ReturnResult()
            {
                Code = Utils.StatusCode.InValidUserInfo,
                Data = null
            });
        }

        [HttpPost("UpdateStaff")]
        public JsonResult UpdateStaff([FromBody] Staff staff)
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims.First().Type;
                var password = HttpContext.User.Claims.First().Value;
                ReturnResult queryAuth = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);
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
                                if (StaffMapper.UpdateStaff(staff,_personnelContext).Code == Utils.StatusCode.Success)
                                {
                                    //如果已授权的账号密码被修改了 我们必须要删除当前授权信息并发送给前端相应的状态码
                                    if (authStaff.StaffId == staff.StaffId && (username!=staff.Name || password!=staff.Password))
                                    {
                                        Task.Run(async () =>
                                        {
                                            await HttpContext.SignOutAsync();
                                        }).Wait();
                                        
                                        return new JsonResult(new ReturnResult()
                                        {
                                            Code = Utils.StatusCode.UserHasUpdate,
                                            Authorization = false
                                        });
                                    }
                                    else
                                    {
                                        return new JsonResult(new ReturnResult()
                                        {
                                            Code = Utils.StatusCode.Success
                                        });
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return new JsonResult(new ReturnResult()
            {
                Code = Utils.StatusCode.InValidParameter,
                Message = "授权信息失效或权限不足以修改部门信息",
            });
        }

        [HttpGet("InValidStaff")]
        public JsonResult InValidStaff(int id)
        {
            ReturnResult result = StaffMapper.InValidStaff(id, _personnelContext);
            return new JsonResult(result);
        }

    }
}