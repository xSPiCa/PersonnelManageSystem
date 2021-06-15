using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Client.Api
{
    /// <summary>
    /// 员工打卡控制器
    /// </summary>
    [Route("Api/Clock")]
    [ApiController]
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ClockController : Controller
    {
        private readonly String _controllerName = "ClockController: ";
        private readonly ILogger<ClockController> _logger;
        private readonly PersonnelContext _personnelContext;

        private readonly String _workingTime;
        private readonly String _closingTime;
        
        public ClockController(ILogger<ClockController> logger, PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
            
            //从配置文件config中读取出上下班时间
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");
            var config = builder.Build();
            _workingTime = config.GetValue<String>("WorkingTime");
            _closingTime = config.GetValue<String>("ClosingTime");
            
        }


        /// <summary>
        /// 获取该职工的所有 打卡记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryClockInfo")]
        public async Task<JsonResult> GetHistoryClockInfo([FromQuery] int limit,[FromQuery] int offset)
        {

            
            ReturnResult result = null;
            result=await MyUtils.GetHasAuthentication(HttpContext,_personnelContext);

            if (result.Code == ResultCode.Success)
            {
                result = await AttendanceMapper.GetStaffAttendance((Staff) result.Data, _personnelContext);
                
                return new JsonResult(result);
            }
            else
            {
                _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);
                return new JsonResult(result);
            }
        }

        /// <summary>
        /// 上班打卡
        /// </summary>
        /// <returns></returns>
        [HttpGet("GoToClock")]
        public async Task<JsonResult> GoToClock()
        {

            var result=await MyUtils.GetHasAuthentication(HttpContext,_personnelContext);

            if (result.Code == ResultCode.Success)
            {
                var queryResult =await AttendanceMapper.GetAttendanceByDate((Staff)result.Data,DateTime.Now,_personnelContext);

                if (queryResult.Code == ResultCode.NoFund)
                {


                    // 生成一个上班时间
                    //具体过程就是 先获得一个当前时间 然后正则替换掉其中的时分秒
                    // 后续生成下班时间也是一样的
                    var now = DateTime.Now.ToString("g");


                    String working = Regex.Replace(now, @"\d\d\:\d\d", _workingTime);
                    
                    
                    var execResult = AttendanceMapper.ExecGoToClock((Staff)result.Data,Convert.ToDateTime(working),_personnelContext);
                    if (execResult.Code == ResultCode.Success)
                    {
                        return new JsonResult(execResult);
                    }
                    else
                    {
                        
                        _logger.LogWarning(_controllerName +((Exception)execResult.Data).StackTrace);
                        return new JsonResult(execResult);
                    }
                }
                else
                {
                    return new JsonResult(ReturnResult.Fail(ResultCode.HasClock));
                }

            }
            else
            {
                _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);
                return new JsonResult(result);
            }
            
        }

        /// <summary>
        /// 下班打卡
        /// </summary>
        /// <returns></returns>
        [HttpGet("GoOffClock")]
        public async Task<JsonResult> GoOffClock()
        {
            var result=await MyUtils.GetHasAuthentication(HttpContext,_personnelContext);
            if (result.Code == ResultCode.Success)
            {
                var queryResult =await AttendanceMapper.GetAttendanceByDate((Staff)result.Data,DateTime.Now,_personnelContext);

                if (queryResult.Code == ResultCode.Success)
                {
                    if (AttendanceStatus.IsGoOffWork(((Attendance) queryResult.Data).WorkStatus))
                    {
                        return new JsonResult(ReturnResult.Fail(ResultCode.HasClock));
                    }
                    else
                    {
                        
                        var now = DateTime.Now.ToString("g");


                        String Closing = Regex.Replace(now, @"\d\d\:\d\d", _closingTime);
                        
                        var execResult = AttendanceMapper.ExecGoOffClock((Attendance) queryResult.Data, Convert.ToDateTime(Closing), _personnelContext);

                        return new JsonResult(ReturnResult.Success());

                    }

                }
                else
                {
                    return new JsonResult(ReturnResult.Fail(ResultCode.NoClock));
                }
                

            }
            else
            {
                _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);
                return new JsonResult(result);
            }

        }
    }
}