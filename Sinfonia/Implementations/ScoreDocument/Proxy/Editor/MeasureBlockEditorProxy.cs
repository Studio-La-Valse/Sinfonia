using Sinfonia.Implementations.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockEditorProxy(MeasureBlock source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IMeasureBlockEditor, IUniqueScoreElement
    {
        private readonly MeasureBlock source = source;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
        private readonly ICommandManager commandManager = commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


        public bool Grace => source.Grace;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Position Position => source.Position;

        public Tuplet Tuplet => source.Tuplet;

        public Guid Guid => source.Guid;

        public int Id => source.Id;




        public void AppendChord(RythmicDuration rythmicDuration)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.AppendChord(rythmicDuration));
            transaction.Enqueue(command);
        }

        public void Splice(int index)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.Splice(index));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlock, MeasureBlockMemento> command = new(source, (s) => s.Clear());
            transaction.Enqueue(command);
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockEditor? right)
        {
            right = null;
            if (source.TryReadNext(out MeasureBlock? _right))
            {
                right = _right.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockEditor? previous)
        {
            previous = null;
            if (source.TryReadNext(out MeasureBlock? _prev))
            {
                previous = _prev.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
            }
            return previous is not null;
        }

        public IEnumerable<IChordEditor> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadChords();
        }

        public MeasureBlockLayout ReadLayout()
        {
            return scoreLayoutDictionary.MeasureBlockLayout(this);
        }

        public void ApplyLayout(MeasureBlockLayout layout)
        {
            scoreLayoutDictionary.Apply(this, layout);
        }

        public void RemoveLayout()
        {
            scoreLayoutDictionary.Restore(this);
        }
    }
}
