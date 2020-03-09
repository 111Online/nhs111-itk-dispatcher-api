using NHS111.Utils.Configuration;
using NHS111.Utils.Helpers;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Utils.IoC
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {
            For<ISqliteConfiguration>().Use<SqliteConfiguration>().Singleton();
            For<IConnectionManager>().Use<SqliteConnectionManager>().Singleton();
            For<IRestfulHelper>().Use<RestfulHelper>().SelectConstructor(() => new RestfulHelper());
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}