using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Admin
{
    /// <summary>
    /// 此控制类用于返回admin 下的视图
    ///
    /// </summary>
    [Route("/Admin")]
    [Controller]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly PersonnelContext _personnelContext;
        
        public AdminController(ILogger<AdminController> logger,PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }
        
        public async Task<IActionResult> Index()
        {
            ReturnResult result = await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);

            if (result.Code == Utils.StatusCode.Success)
            {
                ((Staff) result.Data).Department = null;
                return View("Home", result);
            }
            else
            {
                _logger.LogWarning("授权信息发生变化!");
                await HttpContext.SignOutAsync();
                return View("Home", ReturnResult.Fail(Utils.StatusCode.InValidUserInfo));
            }

        }
        
        /// <summary>
        /// 返回Employee管理子系统视图
        /// </summary>
        /// <returns></returns>
        [HttpGet("EmployeeInfo")]
        public IActionResult EmployeeInfo()
        {
            return View("EmployeeInfo");
        }
    }
    
}