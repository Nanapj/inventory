using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
namespace Inventory.Models
{
    public class OdataFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            actionContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            actionContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //actionContext.RequestContext.Url.ToString().Replace("http:", "https:");

            base.OnActionExecuting(actionContext);
        }
    }
}