using Abp.Dependency;
using Pixel.GEC.Attendance.Configuration;
using Pixel.GEC.Attendance.Url;
using Pixel.GEC.Attendance.Web.Url;

namespace Pixel.GEC.Attendance.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}