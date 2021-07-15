using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Tornado.Shared.Enums;
using Tornado.Shared.Identity;
using Tornado.Shared.Infrastructure;
using Tornado.Shared.ViewModels;

namespace Tornado.Shared.AspNetCore
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController()
        {
            _logger = ServiceLocator.Current.GetInstance<ILogger<BaseController>>();
        }

        /// <summary>
        /// Read ModelError into string collection
        /// </summary>
        /// <returns></returns>
        protected List<string> ListModelErrors
        {
            get
            {
                return ModelState.Values
                  .SelectMany(x => x.Errors
                    .Select(ie => ie.ErrorMessage))
                    .ToList();
            }
        }

        protected UserPrincipal CurrentUser
        {
            get
            {
                return new UserPrincipal(User as ClaimsPrincipal);
            }
        }

        protected string GetModelStateValidationError()
        {
            string message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
            return message;
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            ApiResponse<string> rsp = new ApiResponse<string>();
            rsp.Code = Enums.ApiResponseCodes.ERROR;
#if DEBUG
            rsp.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";
            return Ok(rsp);
#else
             rsp.Description =  "An error occurred while processing your request!";
             return Ok(rsp);
#endif
        }

        protected async Task<ApiResponse<T>> HandleApiOperationAsync
            <T>
            (
           Func<Task<ApiResponse<T>>> action,
           [CallerLineNumber] int lineNo = 0,
           [CallerMemberName] string method = "")
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = ApiResponseCodes.OK
            };

            try
            {

                var methodResponse = await action.Invoke();

                apiResponse.ResponseCode = methodResponse.ResponseCode;
                apiResponse.Payload = methodResponse.Payload;
                apiResponse.TotalCount = methodResponse.TotalCount;
                apiResponse.Code = methodResponse.Code;
                apiResponse.Errors = methodResponse.Errors;
                apiResponse.Description = string.IsNullOrEmpty(apiResponse.Description) ? methodResponse.Description : apiResponse.Description;
                apiResponse.PayloadMetaData = methodResponse.PayloadMetaData;
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.StackTrace);
                apiResponse.Code = ApiResponseCodes.EXCEPTION;

#if DEBUG
                apiResponse.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";
#else
                apiResponse.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";

                //apiResponse.Description = "An error occurred while processing your request!";
#endif
                apiResponse.Errors.Add(apiResponse.Description);
                return apiResponse;
            }
        }
    }
}