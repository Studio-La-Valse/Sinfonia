using Sinfonia.API;

namespace Sinfonia.Native
{
    public abstract class BaseExternalAddin
    {
        public string Author { get; } = "Studio La Valse";


        public virtual void RegisterSettings(IAddinSettingsManager animationSettingsManager)
        {

        }
    }
}
