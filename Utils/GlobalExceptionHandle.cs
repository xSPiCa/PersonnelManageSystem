using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PersonnelManageSystem.Utils
{
    /// <summary>
    /// 自定义全局异常过滤器
    /// </summary>
    public class GlobalExceptionHandle :IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<GlobalExceptionHandle> _logger;
        public GlobalExceptionHandle(
            IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider, ILogger<GlobalExceptionHandle> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
        }

        /// <summary>
        /// 程序抛出异常时自动调用该函数
        /// 跳转页面 GlobalError打印异常内容
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError($"产生错误异常 \n{ex.Message}\n{ex.StackTrace}");
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }
            var result = new ViewResult
            {
                ViewName = "/Views/Shared/GlobalError.cshtml",
                ViewData = new ViewDataDictionary(_modelMetadataProvider,
                    context.ModelState) {{"Exception", ex}}
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}