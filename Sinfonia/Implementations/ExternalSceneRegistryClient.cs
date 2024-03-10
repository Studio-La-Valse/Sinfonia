using Microsoft.Extensions.DependencyInjection;

namespace Sinfonia.Implementations
{
    internal class ExternalSceneRegistryClient : ITypeRegistryClient<IServiceCollection>
    {
        public void Register(Type addin, IServiceCollection container)
        {
            _ = container.AddSingleton(typeof(IExternalScene), addin);
        }
    }
}
