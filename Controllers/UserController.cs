using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers
{
    [Route("Api/User")]
    [ApiController]
    public class UserController:Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly PersonnelContext _personnelContext;

        /// <summary>
        /// 此控制器主要负责 登录登出的操作
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="personnelContext"></param>
        public UserController(ILogger<UserController> logger,PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
        }
        
        /// <summary>
        /// 登录请求
        /// 接收前端发送的json中的username 和password
        /// 参数模型绑定 https://docs.microsoft.com/zh-cn/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0
        /// </summary>
        /// <param name="data">用于接收json格式的数据</param>
        /// <returns>
        /// 如果登录成功 将登录的账号进行授权 重定向到管理系统
        /// 如果登录失败 返回登录界面并添加失败信息
        /// </returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("Login")]
        public JsonResult Login([FromForm(Name = "username")]String username,[FromForm(Name = "password")]string password)
        {
            //判断是否已经授权
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return new JsonResult(new ReturnResult()
                {
                    Authorization = true,
                    Code = Utils.StatusCode.UserHasLogin,
                    Data = null,
                    Message = "账号已登录",
                });
            }
            else
            {
                ReturnResult result = StaffMapper.GetStaffByNamePassword(username, password, _personnelContext);

                if (result.Code == Utils.StatusCode.InValidUserInfo)
                {

                    result.Message = "账号或密码错误";
                    result.Authorization = false;
                    
                }else if (result.Code == Utils.StatusCode.UserDbException)
                {
                    result.Message = "内部错误";
                    result.Authorization = false;
                }else if (result.Code == Utils.StatusCode.Success)
                {
                    result.Message = "成功";
                    result.Authorization = true;
                    // 通过将用户名密码存入cookie的方式实现授权
                    // 参考 https://www.cnblogs.com/OpenCoder/p/8341843.html
                    var claims = new []{new Claim(username,password)};
                
                    var chaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    
                    ClaimsPrincipal user = new ClaimsPrincipal(chaimIdentity);
    
                    //使用异步函数SignInAsync 进行登录操作
                    //需要等待函数结果 
                    Task.Run(async() =>
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
                    }).Wait();
                    ((Staff) result.Data).Department = null;
                    _logger.LogInformation("用户{s}授权成功",username );
                    return new JsonResult(result);

                }
                else
                {
                    result.Message = "未知错误";
                    result.Authorization = false;
                }
                _logger.LogWarning(result.Message);
                
                return new JsonResult(result);
            }
        }
        
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>返回 一些状态信息</returns>
        [IgnoreAntiforgeryToken]
        [Route("Back")]
        public JsonResult Back()
        {
            //判断是否已经授权
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                Task.Run(async () =>
                {
                    await HttpContext.SignOutAsync();
                }).Wait();
                return new JsonResult(new ReturnResult()
                {
                    Code = Utils.StatusCode.Success,
                    Data = null,
                    Authorization = false,
                    Message = "成功"
                });
            }
            else
            {
                return new JsonResult(new ReturnResult()
                {
                    Code = Utils.StatusCode.InValidUserInfo,
                    Data = null,
                    Authorization = false,
                    Message = "失败"
                });
            }

            
        }
    }
}