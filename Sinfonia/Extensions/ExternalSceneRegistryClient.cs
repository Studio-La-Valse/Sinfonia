using Microsoft.Extensions.DependencyInjection;

namespace Sinfonia.Extensions
{
    internal class ExternalSceneRegistryClient : ITypeRegistryClient<IServiceCollection>
    {
        public void Register(Type addin, IServiceCollection container)
        {
            container.AddSingleton(typeof(IExternalScene), addin);
        }
    }
}
