using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers
{
    /// <summary>
    /// 此控制类主要用于返回各个管理子系统的视图
    /// 每个获取实体的控制器任务基本是一样的 可能有优化的空间
    /// 
    /// 业务部分使用前端js 和后端API实现
    /// </summary>
    [Route("View")]
    public class ManagerBodyController : Controller
    {
        
        private readonly ILogger<ManagerBodyController> _logger;
        private readonly PersonnelContext _personnelContext;
        
        public ManagerBodyController(ILogger<ManagerBodyController> logger,PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }
        
        // 通过授权信息返回用户对应部门下的员工信息
        [HttpGet("EmployeeInfo")]
        public IActionResult EmployeeInfo()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims.First().Type;
                var password = HttpContext.User.Claims.First().Value;

                ReturnResult result = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);

                if (result.Code == Utils.StatusCode.Success)
                {
                    _logger.LogInformation("自动授权: " + result.Data.ToString());
                    result.Authorization = true;
                    ((Staff) result.Data).Department = null;
                    return View("EmployeeInfo", result);
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await HttpContext.SignOutAsync();
                    }).Wait();
                    return View("EmployeeInfo", result);
                }
            }

            return View("EmployeeInfo");
        }
    }
}