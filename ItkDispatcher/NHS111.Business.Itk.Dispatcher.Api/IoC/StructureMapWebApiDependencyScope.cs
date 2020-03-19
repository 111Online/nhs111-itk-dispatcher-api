namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    using System.Web.Http.Dependencies;

    using StructureMap;

    public class StructureMapWebApiDependencyScope : StructureMapDependencyScope, IDependencyScope
    {
        public StructureMapWebApiDependencyScope(IContainer container) : base(container)
        {
        }
    }
}
