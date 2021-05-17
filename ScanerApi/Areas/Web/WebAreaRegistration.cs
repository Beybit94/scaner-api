using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Business.Manager;
using Business.Mappers;
using Data.Repositories;
using ScanerApi.Areas.Web.App_Start;
using Data.Access;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ScanerApi.Areas.Web
{
    public class WebAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Web";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(context);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfig.Register();
            WebPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}