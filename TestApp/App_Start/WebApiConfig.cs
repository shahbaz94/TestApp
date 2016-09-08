using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;

namespace TestApp
{
    public static class WebApiConfig
    {
       // config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html") );

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
            config.Routes.MapHttpRoute(
            "Register",                                              // Route name 
            "{controller}/{action}/{fname}/{lname}/{email}/{pswrd}",                           // URL with parameters 
            new { controller = "Customer", action = "Register", fname = "",lname = "", email = "", pswrd = "" }  // Parameter defaults
            );
            config.Routes.MapHttpRoute(
            "Login",                                              // Route name 
            "{controller}/{action}/{email}/{pswrd}",                           // URL with parameters 
            new { controller = "Customer", action = "PostLogin", email = "" , pswrd ="" }  // Parameter defaults
            );
        }
    }
}
