using Application.Common.Interfaces.Services;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Logger;
using Domain.Entities.Log;
using Application.Common.Attributes;
using System.ComponentModel;
using Newtonsoft.Json;
using Application.Common.Extentions;
using Microsoft.AspNetCore.Http.Features;
namespace Application.Common.Behaviours
{
    public class LogBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
 where TRequest : IRequest<TResponse>
    {

        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _logger;
        private IDateTime _datetime;
        private readonly ILoggerPointer _loggerPointer;
        private readonly ICurrentUserService _currentUserService;

        public LogBehaviour(IHttpContextAccessor httpContextAccessor, ILogger logger, IDateTime datetime, ILoggerPointer loggerPointer, ICurrentUserService currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _datetime = datetime;
            _loggerPointer = loggerPointer;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_loggerPointer.Pointer == true)
            {
                return await next();
            }
            var httpContext = _httpContextAccessor.HttpContext;
            TResponse? response = default;
            Exception? exception = null;
            RequestAdminLog requestLogClass = new RequestAdminLog();

            requestLogClass.IPAddress = GetIPAddress();
            requestLogClass.RequestedUrl = GetRequestedUrl();

            var logAttribute = GetAttribute<DotekLogAttribute>(httpContext);
            if (logAttribute == null)
            {
                return await next();
            }
            else
            {
                requestLogClass.ServiceId = (int)logAttribute.services;
                requestLogClass.ServiceOrginalId =(int) logAttribute.services;
                requestLogClass.ServiceMethodId = (int)logAttribute.methods;
                requestLogClass.MethodOrginalId = (int)logAttribute.methods;
                requestLogClass.UrlCode = int.Parse(_currentUserService.UserRoleId ?? "-1000");

            }

            var req = (object)request;
            var reqSerilized = JsonConvert.SerializeObject(req, Formatting.None);



            var pointerIdAttribute = GetAttribute<PointerIdAttribute>(httpContext);
            if (pointerIdAttribute == null)
            {
                requestLogClass.PointerId = -1000;
            }
            else
            {
                requestLogClass.PointerId = GetPointerId(pointerIdAttribute.PointerId, req);
            }

            var sensitiveData = GetAttribute<SensitiveDataAttribute>(httpContext);
            if (sensitiveData == null)
            {
                requestLogClass.MethodInput = reqSerilized;
            }
            else
            {
                var CopyReq = req.DeepCopy();
                var safeMethodInput = RemoveSensitiveData(sensitiveData, CopyReq);
                requestLogClass.MethodInput = safeMethodInput;
            }

            var maxRequestLength = GetAttribute<MaxRequestLengthAttribute>(httpContext);
            if (maxRequestLength != null)
            {
                requestLogClass.MethodInput = CorrectMaxLength(requestLogClass.MethodInput, maxRequestLength.MaxRequestLength);
            }

            requestLogClass.CallTime = _datetime.Now;
            await _logger.Log(requestLogClass);

            _loggerPointer.Pointer = true;
            try
            {
                response = await next();
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {


                var res = response as object;
                string errorMessage = "";

                var resSerilized = JsonConvert.SerializeObject(res, Formatting.None);
                ResponseAdminLog responseLogClass = new ResponseAdminLog()
                {
                    CallTime = requestLogClass.CallTime,
                    Exception = exception?.ToString(),
                    ResponseTime = _datetime.Now,
                    MethodInput = requestLogClass.MethodInput,
                    MethodOrginalId = requestLogClass.MethodOrginalId,
                    RequestId = requestLogClass.RequestId,
                    PointerId = requestLogClass.PointerId,
                    ServiceOrginalId = requestLogClass.ServiceOrginalId,
                    ServiceMethodId = requestLogClass.ServiceMethodId,
                    MethodOutput = resSerilized,
                    ServiceId = requestLogClass.ServiceId,
                    UrlCode = requestLogClass.UrlCode,
                    ErrorCode = exception == null ? GetErrorCode(res) : GetErrorCode(exception)
                };
                var maxResponseLength = GetAttribute<MaxResponseLengthAttribute>(httpContext);
                if (maxRequestLength != null)
                {
                    responseLogClass.MethodOutput = CorrectMaxLength(responseLogClass.MethodOutput, maxResponseLength.MaxResponseLength);
                }

                await _logger.Log(responseLogClass);
            }
        }
        private string? CorrectMaxLength(string input, int maxLength)
        {
            return input?[..maxLength];
        }
        private string RemoveSensitiveData(SensitiveDataAttribute sensitiveData, object request)
        {
            Type type = request.GetType();

            foreach (var data in sensitiveData.SensitiveData)
            {
                SetVariableValue(data, ref request);
            }
            return JsonConvert.SerializeObject(request, Formatting.None);
        }
        public void SetVariableValue(string variableName, ref object data)
        {
            Type type = data.GetType(); // Get the type of the current instance
            string[] parts = variableName.Split('.'); // Split the variable name into its components
            object obj = data; // Set the initial object to the current instance of the class

            // Iterate over the components of the variable name and retrieve the corresponding object
            for (int i = 0; i < parts.Length - 1; i++)
            {
                PropertyInfo property = type.GetProperty(parts[i]); // Get the property with the current name
                obj = property.GetValue(obj); // Get the value of the property and set it as the new object
                type = obj.GetType(); // Update the type to the type of the new object
            }

            // Set the value of the final property to "*"
            PropertyInfo finalProperty = type.GetProperty(parts[parts.Length - 1]);
            try
            {
                finalProperty.SetValue(obj, "***", null);
            }
            catch (Exception)
            {
                try
                {
                    finalProperty.SetValue(obj, default, null);
                }
                catch (Exception)
                {
                    //TODO : LOG Exception
                }
            }
        }

        private long GetPointerId(string pointerId, object request)
        {
            Type type = request.GetType();
            long pId = -1000;

            // var value = request.GetType().GetProperties().Where(c => c.Name == pointerId).SingleOrDefault()?.GetValue(request)?.ToString();
            var value = GetPropertyValue(request, pointerId);
            _ = long.TryParse(value.ToString(), out pId);

            return pId;
        }
        public static object? GetPropertyValue(object src, string propName)
        {
            if (src == null) return -2000;
            if (propName == null) return -2000;

            if (propName.Contains('.'))//complex type nested
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop?.GetValue(src, null);
            }
        }
        private T? GetAttribute<T>(HttpContext httpContext) where T : Attribute
        {
            return (httpContext.Features?.Get< IEndpointFeature >()?.Endpoint?.Metadata
                                         .FirstOrDefault(m => m is T)) as T;
        }
        private static bool CheckAttribute<T>(HttpContext httpContext) where T : Attribute
        {
            var result = httpContext.Features?.Get<IEndpointFeature>()?.Endpoint?.Metadata
                                              .Any(m => m is T);
            if (result == null)
                return false;
            return result.Value;
        }

        public static int GetErrorCode(object obj)
        {
            string variableName = "Code";
            try
            {
                Type type = obj.GetType();
                PropertyInfo property = type.GetProperty(variableName);

                if (property != null)
                {
                    return (int)property.GetValue(obj);
                }

                // Search recursively in nested objects
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo nestedProperty in properties)
                {
                    object nestedObject = nestedProperty.GetValue(obj);

                    if (nestedObject != null)
                    {
                        try
                        {
                            object nestedValue = GetErrorCode(nestedObject);
                            return (int)nestedValue;
                        }
                        catch (ArgumentException)
                        {
                            // Property not found in the nested object, continue searching
                        }
                    }
                }

                // Property not found
                return -4000;
            }
            catch (Exception ex)
            {
                return -3000;
            }


        }

        public string GetIPAddress()
        {
            var ipAddressStr = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddressStr))
            {
                var forwarededIp = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(ipAddressStr))
                    ipAddressStr = forwarededIp;
            }

            if (string.IsNullOrEmpty(ipAddressStr))
                ipAddressStr = "Unknown";

            return ipAddressStr;
        }

        public string GetRequestedUrl()
        {
            var path = _httpContextAccessor.HttpContext.Request.Path;
            var queryString = _httpContextAccessor.HttpContext.Request.QueryString;
            var requestedUrl = $"{path}{queryString}";
            return requestedUrl;
        }
    }
}
