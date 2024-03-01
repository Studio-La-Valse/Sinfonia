﻿using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockReaderProxy : IMeasureBlockReader
    {
        private readonly MeasureBlock source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public MeasureBlockReaderProxy(MeasureBlock source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public bool Grace => source.Grace;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public int Id => source.Id;
        public Guid Guid => source.Guid;

        public Position Position => source.Position;

        public Tuplet Tuplet => source.Tuplet;



        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public IEnumerable<IChordReader> ReadChords()
        {
            return source.GetChordsCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public bool TryReadNext([NotNullWhen(true)] out IMeasureBlockReader? right)
        {
            right = null;
            if (source.TryReadNext(out var _right))
            {
                right = _right.Proxy(commandManager, notifyEntityChanged);
            }
            return right is not null;
        }

        public bool TryReadPrevious([NotNullWhen(true)] out IMeasureBlockReader? previous)
        {
            previous = null;
            if (source.TryReadNext(out var _prev))
            {
                previous = _prev.Proxy(commandManager, notifyEntityChanged);
            }
            return previous is not null;
        }
    }
}
