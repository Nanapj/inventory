using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Inventory.Core;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Inventory.Domain.ViewModels;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Inventory
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            builder.Namespace = "OdataService";
            builder.EntitySet<WarehouseViewModel>("Warehouse");
            builder.EntitySet<ProductViewModel>("Product");
            builder.EntitySet<NoteViewModel>("Note");
            builder.EntityType<NoteViewModel>()
                .Collection
                .Function("GetProductInStock")
                .Returns<StockGroupViewModel>();
            builder.EntitySet<NoteDetailViewModel>("NoteDetail");
            builder.EntitySet<PaymentTypeViewModel>("PaymentType");
            builder.EntitySet<DebtNoteViewModel>("DebtNote");
            builder.EntityType<DebtNoteViewModel>()
                    .Collection
                    .Function("GetDebtInStock")
                    .Returns<DebtStockGroupViewModel>();
            builder.EntitySet<ApplicationUser>("ApplicationUser");
            // Web API routes
            var model = builder.GetEdmModel();
            config.MapODataServiceRoute("odata", "odata", model);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
