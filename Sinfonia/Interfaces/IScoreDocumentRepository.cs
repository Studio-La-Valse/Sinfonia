using Sinfonia.Implementations.ScoreDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Interfaces
{
    public interface IScoreDocumentRepository : IDisposable
    {
        IEnumerable<Guid> AvailableScores();

        void Upload(IScoreDocumentReader documentReader);

        void Upload(IScoreDocumentLayout scoreDocumentLayout);

        ScoreDocumentMemento RestoreLayout(ScoreDocumentMemento scoreDocumentMemento);

        ScoreDocumentMemento Get(Guid guid);
    }
}
