using Sinfonia.Implementations.Commands;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockChainEditorProxy : IMeasureBlockChainEditor, IUniqueScoreElement
    {
        private readonly MeasureBlockChain source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public int Voice => source.Voice;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public MeasureBlockChainEditorProxy(MeasureBlockChain source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void Append(RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.Append(duration, grace)).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.Clear()).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);
        }

        public void Divide(params int[] steps)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.Divide(steps)).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);

        }

        public void DivideEqual(int number)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.DivideEqual(number)).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);
        }

        public void Insert(Position position, RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.Insert(position, duration, grace)).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);
        }

        public void Prepend(RythmicDuration duration, bool grace)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new ParentMementoCommand<MeasureBlockChain, InstrumentMeasure, InstrumentMeasureModel>(source.RibbonMeasure, source, s => s.Prepend(duration, grace)).ThenInvalidate(notifyEntityChanged, source.RibbonMeasure);
            transaction.Enqueue(command);
        }

        public IEnumerable<IMeasureBlockEditor> ReadBlocks()
        {
            return source.GetBlocksCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadBlocks();
        }
    }
}
