using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers
{
    /// <summary>
    /// 部门管理API
    /// warining: 注意实体之间嵌套造成的序列化死循环
    /// </summary>
    [Route("Api/DepartmentInfo")]
    [ApiController]
    public class DepartmentController:Controller
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly PersonnelContext _personnelContext;

        public DepartmentController(ILogger<DepartmentController> logger, PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }

        /// <summary>
        /// 函数从数据库查找 权限值大于等于当前已授权员工所属部门的权限值 的部门
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDepartmentInfo")]
        public JsonResult GetAllDepartmentInfo()
        {
            
            ReturnResult queryResult = DepartmentMapper.GetAllDepartment(_personnelContext);

            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims.First().Type;
                var password = HttpContext.User.Claims.First().Value;
                ReturnResult queryAuth = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);
                if (queryAuth.Code == Utils.StatusCode.Success && queryResult.Code == Utils.StatusCode.Success)
                {
                    Staff authStaff = (Staff) queryAuth.Data;
                    
                    var tmp = _personnelContext.Department.Single(d => d.DepartmentId == authStaff.DepartmentId);

                    List<Department> queryHighAuthority =  (List<Department>)DepartmentMapper.GetValidDepartment(tmp.Authority, _personnelContext).Data;
                    
                    
                    // 如果部门中包含 员工 将会造成序列化死循环
                    queryHighAuthority.ForEach(d=> d.Staves=null);
                    
                    return new JsonResult(new ReturnResult()
                    {
                        Code = Utils.StatusCode.Success,
                        Data = queryHighAuthority
                    });
                }
            }

            return new JsonResult(new ReturnResult()
            {
                Code = Utils.StatusCode.InValidUserInfo
            });

        }
    }
}