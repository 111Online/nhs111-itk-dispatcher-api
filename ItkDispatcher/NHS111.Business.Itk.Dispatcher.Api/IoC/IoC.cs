using StructureMap;

namespace NHS111.Business.Itk.Dispatcher.Api.IoC
{
    using Domain.Itk.Dispatcher.IoC;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var cont = new Container(c => c.AddRegistry<ItkDispatcherApiRegistry>());
            cont.Configure(c => c.AddRegistry<DispatcherDomainRegistry>());
            cont.AssertConfigurationIsValid();
            return cont;
        }
    }
}