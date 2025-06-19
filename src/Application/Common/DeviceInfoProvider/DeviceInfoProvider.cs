using Microsoft.AspNetCore.Http;

namespace Application.Common.DeviceInfoProvider
{
    public interface IDeviceInfoProvider
    {
        string GetDeviceInfo();
    }
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetDeviceInfo()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
                return "Không thể xác định thiết bị";

            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var ipAddress = GetClientIpAddress(context);

            var browser = GetBrowser(userAgent);
            var os = GetOS(userAgent);

            return $"{browser} - {os} - IP: {ipAddress}";
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            return !string.IsNullOrWhiteSpace(forwarded)
                ? forwarded
                : context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        }

        private string GetBrowser(string userAgent)
        {
            if (userAgent.Contains("Edge")) return "Edge";
            if (userAgent.Contains("Chrome")) return "Chrome";
            if (userAgent.Contains("Firefox")) return "Firefox";
            if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) return "Safari";
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident")) return "Internet Explorer";

            return "Trình duyệt không xác định";
        }

        private string GetOS(string userAgent)
        {
            if (userAgent.Contains("Windows NT 10.0")) return "Windows 10";
            if (userAgent.Contains("Windows NT 6.1")) return "Windows 7";
            if (userAgent.Contains("Mac OS X")) return "macOS";
            if (userAgent.Contains("Android")) return "Android";
            if (userAgent.Contains("iPhone")) return "iOS";

            return "Hệ điều hành không xác định";
        }
    }
}
