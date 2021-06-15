using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Client
{
    [Route("/")]
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly PersonnelContext _personnelContext;
        
        public ClientController(ILogger<ClientController> logger,PersonnelContext personnelContext)
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
                return View("Index", result);
            }
            else
            {
                _logger.LogWarning("授权信息发生变化!");
                await HttpContext.SignOutAsync();
                return View("Index", ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
            }
        }


        
        /// <summary>
        /// 返回个人信息页面
        /// </summary>
        /// <returns></returns>
        [Route("Individual")]
        public async Task<IActionResult> IndividualInfo()
        {
            ReturnResult result = await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);

            if (result.Code == Utils.ResultCode.Success)
            {
                ((Staff) result.Data).Department = null;
                return View("Individual", result);
            }
            else
            {
                _logger.LogWarning("授权信息发生变化!");
                await HttpContext.SignOutAsync();
                return View("Individual", ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
            }
        }

        
        /// <summary>
        /// 返回打卡页面
        /// </summary>
        /// <returns></returns>
        [Route("Clock")]
        public async Task<IActionResult> Clock()
        {
            ReturnResult result = await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);

            if (result.Code == Utils.ResultCode.Success)
            {
                ((Staff) result.Data).Department = null;
                return View("Clock", result);
            }
            else
            {
                _logger.LogWarning("授权信息发生变化!");
                await HttpContext.SignOutAsync();
                return View("Clock", ReturnResult.Fail(Utils.ResultCode.InValidUserInfo));
            }
        }

    }
}