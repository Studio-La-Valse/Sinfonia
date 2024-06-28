using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models.Base;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument;

public class ScoreMeasureEditorProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreMeasure
{
    private readonly ScoreMeasure source = source;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;



    public int IndexInScore => source.IndexInScore;

    public TimeSignature TimeSignature => source.TimeSignature;

    public int Id => source.Id;

    public bool IsLastInScore => source.IsLastInScore;

    public AuthorScoreMeasureLayout Layout => source.AuthorLayout;

    public TemplateProperty<KeySignature> KeySignature => Layout.KeySignature.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostDocument);

    public TemplateProperty<double?> PaddingBottom => Layout.PaddingBottom.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostDocument);

    public ReadonlyTemplateProperty<double> PaddingLeft => Layout.PaddingLeft;

    public ReadonlyTemplateProperty<double> PaddingRight => Layout.PaddingRight;




    public bool TryReadNext([NotNullWhen(true)] out IScoreMeasure? next)
    {
        _ = source.TryReadNext(out var _next);
        next = _next?.ProxyEditor(commandManager, notifyEntityChanged);
        return next != null;
    }

    public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasure? previous)
    {
        _ = source.TryReadPrevious(out var _previous);
        previous = _previous?.ProxyEditor(commandManager, notifyEntityChanged);
        return previous != null;
    }

    public IInstrumentMeasure ReadMeasure(int ribbonIndex)
    {
        return source.GetMeasureCore(ribbonIndex).ProxyEditor(commandManager, notifyEntityChanged);
    }

    public IEnumerable<IInstrumentMeasure> ReadMeasures()
    {
        return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return ReadMeasures();
    }

    public void Restore()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(Layout).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }

    public bool Equals(IUniqueScoreElement? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Id == Id;
    }
}
