using System;
using System.Web;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;
using Microsoft.Practices.ServiceLocation;
using StructureMap.Configuration.DSL;

namespace FubuMVC.StructureMap
{
    // This is only here for the purposes of having 
    // a stand in during testing and container validation
    public class StandInRequestData : IRequestData
    {
        public object Value(string key)
        {
            return null;
        }

        public bool Value(string key, Action<object> callback)
        {
            return false;
        }
    }

    public class StructureMapFubuRegistry : Registry
    {
        public StructureMapFubuRegistry()
        {
            For<HttpRequestWrapper>().Use(c => BuildRequestWrapper());

            //Needed for AssertConfigurationIsValid in global.asax
            For<AggregateDictionary>().Use(new AggregateDictionary());

            For<HttpContextBase>().Use<HttpContextWrapper>().Ctor<HttpContext>().Is(
                x => x.ConstructedBy(BuildContextWrapper));
            For<IServiceLocator>().Use<StructureMapServiceLocator>();
            For<IWebFormsControlBuilder>().Use<StructureMapWebFormsControlBuilder>();

            SetAllProperties(x =>
            {
                x.Matching(p => p.DeclaringType == typeof (FubuPage));
                x.OfType<IServiceLocator>();
            });

            
        }

        public HttpContext BuildContextWrapper()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current;
                }
            }
            catch (HttpException)
            {
                //This is only here for web startup when HttpContext.Current is not available.
            }

            return null;
        }

        public static HttpRequestWrapper BuildRequestWrapper()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    return new HttpRequestWrapper(HttpContext.Current.Request);
                }
            }
            catch (HttpException)
            {
                //This is only here for web startup when HttpContext.Current is not available.
            }

            return null;
        }
    }
}