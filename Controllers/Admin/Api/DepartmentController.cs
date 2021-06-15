using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Admin.Api
{
    /// <summary>
    /// 部门管理API
    /// warining: 注意实体之间嵌套造成的序列化死循环
    /// </summary>
    [Route("Api/DepartmentInfo")]
    [ApiController]
    [Authorize(Roles = "admin")]
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
        public async Task<JsonResult> GetAllDepartmentInfo()
        {
            
            ReturnResult queryResult = DepartmentMapper.GetAllDepartment(_personnelContext);

            ReturnResult queryAuth =await MyUtils.GetHasAuthentication(HttpContext,_personnelContext);
            if (queryAuth.Code == Utils.ResultCode.Success && queryResult.Code == Utils.ResultCode.Success)
            {
                Staff authStaff = (Staff) queryAuth.Data;
                
                var tmp = _personnelContext.Department.Single(d => d.DepartmentId == authStaff.DepartmentId);

                List<Department> queryHighAuthority =  (List<Department>)DepartmentMapper.GetValidDepartment(tmp.Authority, _personnelContext).Data;
                
                
                // 如果部门中包含 员工 将会造成序列化死循环
                queryHighAuthority.ForEach(d=> d.Staves=null);

                return new JsonResult(ReturnResult.Success(queryHighAuthority));
            }


            return new JsonResult(ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));

        }
    }
}