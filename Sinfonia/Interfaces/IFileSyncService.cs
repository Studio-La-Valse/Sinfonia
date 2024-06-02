using System.Threading.Tasks;

namespace Sinfonia.Interfaces;

public interface IFileSyncService
{
    Task CreateNew();
    Task Download();
    Task Save();
    Task Delete();
}
