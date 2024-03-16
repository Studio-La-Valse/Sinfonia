using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Interfaces;
using StudioLaValse.CommandManager;
using StudioLaValse.Drawable;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Core.Primitives;
using StudioLaValse.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Tests
{
    [TestClass]
    public class MeasureBlockChainTests
    {
        private readonly IScoreBuilderFactory scoreBuilderFactory;
        public MeasureBlockChainTests()
        {
            IServiceProvider serviceProvider = App.CreateHostBuilder([]).Build().Services;
            scoreBuilderFactory = serviceProvider.GetRequiredService<IScoreBuilderFactory>();
        }

        [TestMethod]
        public void TestDivisions()
        {
            _Assert(new TimeSignature(4, 4), [3, 3, 2], [new RythmicDuration(4, 1), new RythmicDuration(4, 1), new RythmicDuration(4)]);
            _Assert(new TimeSignature(4, 4), [1, 1], [new RythmicDuration(2), new RythmicDuration(2)]);
            _Assert(new TimeSignature(7, 8), [2, 3, 2], [new RythmicDuration(4), new RythmicDuration(4, 1), new RythmicDuration(4)]);
            _Assert(new TimeSignature(3, 8), [1, 1, 1], [new RythmicDuration(8), new RythmicDuration(8), new RythmicDuration(8)]);
            _Assert(new TimeSignature(3, 8), [3, 3], [new RythmicDuration(8, 1), new RythmicDuration(8, 1)]);
            _Assert(new TimeSignature(4, 4), [7, 1], [new RythmicDuration(2, 2), new RythmicDuration(8)]);
            _Assert(new TimeSignature(4, 4), [15, 1], [new RythmicDuration(2, 3), new RythmicDuration(16)]);
        }

        private void _Assert(TimeSignature timeSignature, int[] values, RythmicDuration[] expectedLenghts)
        {
            ICommandManager commandManager = CommandManager.CreateGreedy();
            INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();
            ScoreDocumentStyleTemplate style = new();
            (StudioLaValse.ScoreDocument.Builder.IScoreBuilder builder, IScoreDocumentReader reader, IScoreDocumentLayout layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged, style);

            StudioLaValse.ScoreDocument.Builder.IScoreBuilder score = builder
                .Edit(editor =>
                {
                    editor.AddInstrumentRibbon(Instrument.Violin);

                    editor.AppendScoreMeasure(timeSignature);

                    StudioLaValse.ScoreDocument.Builder.IInstrumentMeasureEditor measure = editor.ReadScoreMeasure(0).ReadMeasure(0);
                    measure.AddVoice(0);

                    StudioLaValse.ScoreDocument.Builder.IMeasureBlockChainEditor chain = measure.ReadBlockChainAt(0);
                    chain.Divide(values);
                })
                .Build();

            IMeasureBlockChainReader outChain = reader.ReadScoreMeasure(0).ReadMeasure(0).ReadBlockChainAt(0);
            RythmicDuration[] lengths = outChain.ReadBlocks().Select(b => b.RythmicDuration).ToArray();

            Assert.IsTrue(lengths.SequenceEqual(expectedLenghts));
        }

        [TestMethod]
        public void TestDivisionsExceptions()
        {
            _AssertException(new TimeSignature(4, 4), [0]);
            _AssertException(new TimeSignature(4, 4), [7, 5]);
            _AssertException(new TimeSignature(4, 4), [2, 2, 2]);
            _AssertException(new TimeSignature(5, 8), [1]);
        }

        private void _AssertException(TimeSignature timeSignature, int[] values)
        {
            ICommandManager commandManager = CommandManager.CreateGreedy();
            INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();
            ScoreDocumentStyleTemplate style = new();
            (StudioLaValse.ScoreDocument.Builder.IScoreBuilder builder, IScoreDocumentReader reader, IScoreDocumentLayout layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged, style);

            StudioLaValse.ScoreDocument.Builder.IScoreBuilder score = builder
                .Edit(editor =>
                {
                    editor.AddInstrumentRibbon(Instrument.Violin);

                    editor.AppendScoreMeasure(timeSignature);

                    StudioLaValse.ScoreDocument.Builder.IInstrumentMeasureEditor measure = editor.ReadScoreMeasure(0).ReadMeasure(0);
                    measure.AddVoice(0);

                    StudioLaValse.ScoreDocument.Builder.IMeasureBlockChainEditor chain = measure.ReadBlockChainAt(0);
                    _ = Assert.ThrowsException<InvalidOperationException>(() => chain.Divide(values));
                })
                .Build();
        }

        [TestMethod]
        public void TestEqualDivisions()
        {
            _AssertEqual(new TimeSignature(4, 4), 2, [new RythmicDuration(2), new RythmicDuration(2)]);
            _AssertEqual(new TimeSignature(3, 8), 2, [new RythmicDuration(8, 1), new RythmicDuration(8, 1)]);
            _AssertEqual(new TimeSignature(3, 8), 6, [new RythmicDuration(16), new RythmicDuration(16), new RythmicDuration(16), new RythmicDuration(16), new RythmicDuration(16), new RythmicDuration(16)]);
            _AssertEqual(new TimeSignature(6, 8), 2, [new RythmicDuration(4, 1), new RythmicDuration(4, 1)]);
            _AssertEqual(new TimeSignature(6, 8), 3, [new RythmicDuration(4), new RythmicDuration(4), new RythmicDuration(4)]);
            _AssertEqual(new TimeSignature(7, 16), 1, [new RythmicDuration(4, 2)]);
        }

        private void _AssertEqual(TimeSignature timeSignature, int number, RythmicDuration[] expectedLenghts)
        {
            ICommandManager commandManager = CommandManager.CreateGreedy();
            INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();
            ScoreDocumentStyleTemplate style = new();
            (StudioLaValse.ScoreDocument.Builder.IScoreBuilder builder, IScoreDocumentReader reader, IScoreDocumentLayout layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged, style);

            StudioLaValse.ScoreDocument.Builder.IScoreBuilder score = builder
                .Edit(editor =>
                {
                    editor.AddInstrumentRibbon(Instrument.Violin);

                    editor.AppendScoreMeasure(timeSignature);

                    StudioLaValse.ScoreDocument.Builder.IInstrumentMeasureEditor measure = editor.ReadScoreMeasure(0).ReadMeasure(0);
                    measure.AddVoice(0);

                    StudioLaValse.ScoreDocument.Builder.IMeasureBlockChainEditor chain = measure.ReadBlockChainAt(0);
                    chain.DivideEqual(number);
                })
                .Build();

            IMeasureBlockChainReader outChain = reader.ReadScoreMeasure(0).ReadMeasure(0).ReadBlockChainAt(0);
            RythmicDuration[] lengths = outChain.ReadBlocks().Select(b => b.RythmicDuration).ToArray();

            Assert.IsTrue(lengths.SequenceEqual(expectedLenghts));
        }
    }
}