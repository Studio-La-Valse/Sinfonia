using Sinfonia.Implementations.ScoreDocument;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using System.Diagnostics.CodeAnalysis;
using YamlDotNet.Core.Tokens;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class MeasureBlockEditorProxy(MeasureBlock source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IMeasureBlock
    {
        private readonly MeasureBlock source = source;
        private readonly ICommandManager commandManager = commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;



        public RythmicDuration RythmicDuration => source.RythmicDuration;
        public Position Position => source.Position;
        public Tuplet Tuplet => source.Tuplet;
        public InstrumentMeasure HostMeasure => source.RibbonMeasure;
        public int Id => source.Id;


        public AuthorMeasureBlockLayout Layout => source.AuthorLayout;  
        public TemplateProperty<StemDirection> StemDirection => Layout.StemDirection.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, HostMeasure);
        public TemplateProperty<double> StemLength => Layout.StemLength.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, HostMeasure);
        public TemplateProperty<double> BeamAngle => Layout.BeamAngle.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, HostMeasure);
        public ReadonlyTemplateProperty<double> BeamThickness => Layout.BeamThickness;
        public ReadonlyTemplateProperty<double> BeamSpacing => Layout.BeamSpacing;




        public void AppendChord(RythmicDuration rythmicDuration, params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockModel>(source, (s) => s.AppendChord(rythmicDuration, rebeam: true, pitches: pitches)).ThenInvalidate(notifyEntityChanged, HostMeasure);
            transaction.Enqueue(command);
        }

        public void Splice(int index)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockModel>(source, (s) => s.Splice(index)).ThenInvalidate(notifyEntityChanged, HostMeasure);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<MeasureBlock, MeasureBlockModel>(source, (s) => s.Clear()).ThenInvalidate(notifyEntityChanged, HostMeasure);
            transaction.Enqueue(command);
        }

        public void Restore()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<AuthorMeasureBlockLayout, MeasureBlockLayoutMembers>(Layout).ThenInvalidate(notifyEntityChanged, HostMeasure);
            transaction.Enqueue(command);
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlock? right)
        {
            right = null;
            if (source.TryReadNext(out var _right))
            {
                right = _right.ProxyEditor(commandManager, notifyEntityChanged);
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlock? previous)
        {
            previous = null;
            if (source.TryReadNext(out var _prev))
            {
                previous = _prev.ProxyEditor(commandManager, notifyEntityChanged);
            }
            return previous is not null;
        }

        public IEnumerable<IChord> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
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
}
