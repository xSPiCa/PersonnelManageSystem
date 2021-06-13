using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.Controllers.Client;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers
{
    
    
    /// <summary>
    /// 负责登录登出认证相关控制器
    /// </summary>
    [Route("User")]
    [ApiController]
    public class UserController:Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly PersonnelContext _personnelContext;


        public UserController(ILogger<UserController> logger,PersonnelContext personnelContext)
        {
            
            _logger = logger;
            _personnelContext = personnelContext;
        }
        
        /// <summary>
        /// 返回登录视图
        /// </summary>
        /// <param name="message">传递视图登录状况</param>
        /// <returns></returns>
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Index([FromQuery] string message)
        {
            return View("Login",ReturnResult.Success(null,message) );
        }
        

        /// <summary>
        /// 认证接口
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        [HttpPost("LoginWithUser")]
        public async Task<IActionResult> LoginWithUser([FromForm(Name = "username")]String username,[FromForm(Name = "password")]string password)
        {
            ReturnResult result = await StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);

            
            if (result.Code == Utils.StatusCode.Success)
            {
                Staff staff = (Staff) result.Data;
                //获取职位字符串 post原本是个枚举类型静态方法RoleInfo.getRoleStr可以转换为对应的字符串名
                var post = RoleInfo.GetRoleStr(staff.Post);
                //创建凭证 类型为Role 之后使用注释[Authorize(Roles = "")]来对控制器进行访问控制
                var claims = new []
                {
                    new Claim(ClaimTypes.Role,post),
                    new Claim("SID",(staff.StaffId).ToString()),
                };
            //使用 Cookie 生成的认    证信息
                var chaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                
                ClaimsPrincipal user = new ClaimsPrincipal(chaimIdentity);

                //异步登录
                await HttpContext.SignInAsync(user);
                _logger.LogInformation("ID" + staff.StaffId+" : " +post + "获得认证!");
                //根据角色 跳转刀相应页面
                if ( RoleInfo.GetRoleStr(staff.Post) == "user")
                {
                    return RedirectToAction("Index", controllerName:"Client");
                }
                else if(RoleInfo.GetRoleStr(staff.Post) == "admin")
                {
                    
                    return RedirectToAction("Index", controllerName:"Admin");
                }

            }
            //登录失败则返回登录界面 并返回信息
            return RedirectToAction("Index",new
            {
                message = "账号或密码错误!"
            });
        }
        
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [Route("Back")]
        [Authorize]
        public async Task<JsonResult> Back()
        {
            await HttpContext.SignOutAsync();
            return new JsonResult(ReturnResult.Success());
        }
        
    }
}