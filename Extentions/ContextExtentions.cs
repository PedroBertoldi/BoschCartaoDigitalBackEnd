using Microsoft.AspNetCore.Http;

namespace BoschCartaoDigitalBackEnd.Extentions
{
    public static class ContextExtentions
    {
        public static string GetLocationURI(this HttpContext context, string route, string args = "")
        {
            var locationUri = $"{context.Request.Scheme}://{context.Request.Host.ToUriComponent()}/{route}";
            return (!string.IsNullOrEmpty(args)) ? locationUri : locationUri + $"?{args}";
        }
    }
}