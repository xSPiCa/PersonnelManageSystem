using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;
using PersonnelManageSystem.DAL;

namespace PersonnelManageSystem.Controllers
{
    /// <summary>
    /// 此控制类只要用于总系统的页面控制 也就是Index 页面的控制
    ///
    /// warining: 注意实体之间嵌套造成的序列化死循环
    /// </summary>
    [Route("/")]
    [Controller]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PersonnelContext _personnelContext;
        
        public HomeController(ILogger<HomeController> logger,PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }

        
        public IActionResult Index()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                var username = HttpContext.User.Claims.First().Type;
                var password = HttpContext.User.Claims.First().Value;

                ReturnResult result = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);

                if (result.Code == Utils.StatusCode.Success)
                {
                    _logger.LogInformation("自动授权: "+result.Data.ToString());
                    result.Authorization = true;
                    ((Staff) result.Data).Department = null;
                    return View("Home", result);
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await HttpContext.SignOutAsync();
                    }).Wait();
                    return View("Home", result);
                }
            }
            return View("Home");
        }
        

        
    }
}