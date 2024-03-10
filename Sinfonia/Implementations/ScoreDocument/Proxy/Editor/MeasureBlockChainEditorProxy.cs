using Sinfonia.Implementations.Commands;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class MeasureBlockChainEditorProxy : IMeasureBlockChainEditor, IUniqueScoreElement
    {
        private readonly MeasureBlockChain source;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public int Voice => source.Voice;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public MeasureBlockChainEditorProxy(MeasureBlockChain source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public void Append(RythmicDuration duration, bool grace)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.Append(duration, grace));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public void Divide(params int[] steps)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.Divide(steps));
            transaction.Enqueue(command);

        }

        public void DivideEqual(int number)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.DivideEqual(number));
            transaction.Enqueue(command);
        }

        public void Insert(Position position, RythmicDuration duration, bool grace)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.Insert(position, duration, grace));
            transaction.Enqueue(command);
        }

        public void Prepend(RythmicDuration duration, bool grace)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<MeasureBlockChain, RibbonMeasureVoiceMemento> command = new(source, s => s.Prepend(duration, grace));
            transaction.Enqueue(command);
        }

        public IEnumerable<IMeasureBlockEditor> ReadBlocks()
        {
            return source.GetBlocksCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadBlocks();
        }
    }
}
