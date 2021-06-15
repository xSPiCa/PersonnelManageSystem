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
    /// 控制器名和视图文件夹名是对应的
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

            if (result.Code == Utils.ResultCode.Success)
            {
                ((Staff) result.Data).Department = null;
                return View("Home", result);
            }
            else
            {
                _logger.LogWarning("授权信息发生变化!");
                await HttpContext.SignOutAsync();
                return View("Home", ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
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

        /// <summary>
        /// 返回Attendance管理子系统视图
        /// </summary>
        /// <returns></returns>
        [HttpGet("AttendanceInfo")]
        public IActionResult AttendanceInfo()
        {
            return View("Attendance");
        }

        /// <summary>
        /// 返回Department管理子系统视图
        /// 未编写
        /// </summary>
        /// <returns></returns>
        [HttpGet("DepartmentInfo")]
        public IActionResult DepartmentInfo()
        {
            return View("Department");
        }

        /// <summary>
        /// 返回OperationLog管理子系统视图
        /// 未编写
        /// </summary>
        /// <returns></returns>
        [HttpGet("OperationLogInfo")]
        public IActionResult OperationLogInfo()
        {
            return View("OperationLog");
        }

        /// <summary>
        /// 返回Salary管理子系统视图
        /// 未编写
        /// </summary>
        /// <returns></returns>
        [HttpGet("SalaryInfo")]
        public IActionResult SalaryInfo()
        {
            return View("Salary");
        }

    }
    
}