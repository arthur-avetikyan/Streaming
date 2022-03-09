using Newtonsoft.Json;

using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace KioskStream.Web.Server.Wrappers
{
    [Serializable]
    [DataContract]
    public class ApiResponse<T> where T : class
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ApiError ResponseException { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public T Result { get; set; }

        [JsonConstructor]
        public ApiResponse(int statusCode,
                           string message = "",
                           object result = null,
                           ApiError apiError = null,
                           string apiVersion = "")
        {
            StatusCode = statusCode;
            Message = message;
            Result = result as T;
            ResponseException = apiError;
            Version = string.IsNullOrWhiteSpace(apiVersion) ? Assembly.GetEntryAssembly().GetName().Version.ToString() : apiVersion;
            IsError = false;
        }

        public ApiResponse(int statusCode, ApiError apiError)
        {
            StatusCode = statusCode;
            Message = apiError.ExceptionMessage;
            ResponseException = apiError;
            IsError = true;
        }
    }
}
