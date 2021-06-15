using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonnelManageSystem.DAL;
using PersonnelManageSystem.Models;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Controllers.Admin.Api
{
    /// <summary>
    /// 考勤系统 api接口
    /// </summary>
    [Route("Api/AttendanceInfo")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [IgnoreAntiforgeryToken]
    public class AttendanceInfoController : Controller
    {
        private readonly String _controllerName = "AttendanceInfoController: ";
        private readonly ILogger<AttendanceInfoController> _logger;
        private readonly PersonnelContext _personnelContext;


        
        public AttendanceInfoController(ILogger<AttendanceInfoController> logger, PersonnelContext personnelContext)
        {
            _logger = logger;
            _personnelContext = personnelContext;
            

        }
        
        [Route("GetAllAttendance")]
        public async Task<JsonResult> GetAllAttendance([FromQuery]int limit,[FromQuery]int offset)
        {
            ReturnResult queryStaff =  await MyUtils.GetHasAuthentication(HttpContext, _personnelContext);
            if (queryStaff.Code == ResultCode.Success)
            {
                ReturnResult result = await AttendanceMapper.GetAllAttendanceInfo((Staff)queryStaff.Data,limit,offset,_personnelContext);
                if (result.Code != ResultCode.Success)
                {
                    _logger.LogWarning(_controllerName +((Exception)result.Data).StackTrace);;
                }

                return Json(result);
            }
            else
            {
                _logger.LogWarning(_controllerName +((Exception)queryStaff.Data).StackTrace);
            }

            return new JsonResult(queryStaff);
        }
    }
}