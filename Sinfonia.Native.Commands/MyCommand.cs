using Sinfonia.API;
using StudioLaValse.ScoreDocument.Core;

namespace Sinfonia.Native.Commands
{
    public class MyCommandActivator : IExternalCommandActivator
    {
        private readonly IApplication application;

        public string Name => "My command name";
        public string Description => "This command will blow your mind.";
        public string Author => "Who am I";
        public Guid Guid => new("332B2D99-7678-411C-8599-6880A839A901");

        public MyCommandActivator(IApplication application)
        {
            this.application = application;
        }

        public IExternalCommand Activate()
        {
            return new MyExternalCommand(application);
        }
    }

    public class MyExternalCommand : IExternalCommand
    {
        private readonly IApplication application;

        public MyExternalCommand(IApplication application)
        {
            this.application = application;
        }

        public void ExecuteCommand()
        {
            IDocument document = application.ActiveDocumentOrThrow();

            StudioLaValse.ScoreDocument.Builder.IScoreBuilder builder = document.ScoreBuilder
                .Edit(editor =>
                {
                    editor.AddInstrumentRibbon(Instrument.Violin);
                })
                .Edit(editor =>
                {
                    for (int i = 0; i < 4; i++)
                    {
                        editor.AppendScoreMeasure();
                    }
                })
                .Build();
        }
    }
}
