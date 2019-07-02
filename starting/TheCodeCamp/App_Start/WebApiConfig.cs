using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using Microsoft.Web.Http.Versioning.Conventions;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using TheCodeCamp.Controllers;

namespace TheCodeCamp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            AutofacConfig.Register();

            config.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = new UrlSegmentApiVersionReader();
                //ApiVersionReader.Combine(
                //    new HeaderApiVersionReader("X-Version"),
                //    new QueryStringApiVersionReader("ver")
                //    );

                cfg.Conventions.Controller<TalksController>()   //<= This style of convension allows you to enable versioning from centralized place 
                    .HasApiVersion(1, 0)                        //rather than attributes
                    .HasApiVersion(1, 1)
                    .Action(m => m.Get(default(string), default(int), default(bool)))
                        .MapToApiVersion(2, 0);

            });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                       new CamelCasePropertyNamesContractResolver();

            var constriantResolver = new DefaultInlineConstraintResolver() // <== You have to enable this to enable Url Segmention Versioning
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof(ApiVersionRouteConstraint)
                }
            };


            // Web API routes
            config.MapHttpAttributeRoutes(constriantResolver);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
