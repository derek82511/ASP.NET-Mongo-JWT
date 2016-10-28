using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VieshowManager.Services.Helpers
{
    public static class ApplicationHelper
    {
        public static string getAppBaseUrl()
        {
            return string.Format("{0}{1}{2}",
                "http://",
                HttpContext.Current.Request.Url.Authority,
                HttpRuntime.AppDomainAppVirtualPath
            );
        }
    }
}