using System;
using System.Net;
using PersonnelManageSystem.Utils;

namespace PersonnelManageSystem.Models
{
    /// <summary>
    /// 用于返回给视图层统一的模型
    /// </summary>
    public class ReturnResult
    {
        public StatusCode Code { get; set; }
        public String Message { get; set; }

        /// <summary>
        /// 登录状态 用于辅助控制视图层对是否登录做出相应变化
        /// </summary>

        /// <summary>
        /// 模型数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 如果返回结果是集合 则total表示返回数量
        /// </summary>
        public int Total { get; set; }

        public static ReturnResult Success()
        {
            return new ReturnResult()
            {
                Code = StatusCode.Success,
            };
        }
        public static ReturnResult Success(Object data)
        {
            return new ReturnResult()
            {
                Code = StatusCode.Success,
                Data = data
            };
        }
        public static ReturnResult Success(Object data,String message)
        {
            return new ReturnResult()
            {
                Code = StatusCode.Success,
                Data = data,
                Message = message
            };
        }
        public static ReturnResult Success(Object data,int total)
        {
            return new ReturnResult()
            {
                Code = StatusCode.Success,
                Data = data,
                Total = total
            };
        }
        
        public static ReturnResult Fail( StatusCode code)
        {
            return new ReturnResult()
            {
                Code = code,
            };
        }
        public static ReturnResult Fail( StatusCode code,String message)
        {
            return new ReturnResult()
            {
                Code = code,
                Message = message
            };
        }
        
        

    }
}