using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace ArizaApp.Helpers
{
    public static class RequestHelper
    {
        public static string GetIpAddress(HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
        
        public static string GetPort(HttpRequest request)
        {
            return request.HttpContext.Connection.RemotePort.ToString();
        }
    }
}