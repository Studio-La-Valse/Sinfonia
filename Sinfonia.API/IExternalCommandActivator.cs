namespace Sinfonia.API
{
    public interface IExternalCommandActivator : IExternalAddin
    {
        IExternalCommand Activate();
    }
}