using System.Web.Http;
using NHS111.Business.Itk.Dispatcher.Api.IoC;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(StructuremapWebApi), "Start")]

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    public static class StructuremapWebApi {
        public static void Start() {
			var container = StructuremapMvc.StructureMapDependencyScope.Container;
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }
    }
}