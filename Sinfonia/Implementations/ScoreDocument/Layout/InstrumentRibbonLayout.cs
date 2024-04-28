using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class InstrumentRibbonLayout : IInstrumentRibbonLayout, ILayout<InstrumentRibbonLayoutMemento>
    {
        private readonly InstrumentRibbon instrumentRibbon;
        private readonly ReferenceTemplateProperty<string> abbreviatedName;
        private readonly ReferenceTemplateProperty<string> displayName;
        private readonly ValueTemplateProperty<int> numberOfStaves;


        public string AbbreviatedName { get => abbreviatedName.Value; set => abbreviatedName.Value = value; }
        public string DisplayName { get => displayName.Value; set => displayName.Value = value; }
        public int NumberOfStaves { get => numberOfStaves.Value; set => numberOfStaves.Value = value; }


        public bool Collapsed { get; set; }


        public InstrumentRibbonLayout(InstrumentRibbon instrumentRibbon)
        {
            this.instrumentRibbon = instrumentRibbon;

            displayName = new ReferenceTemplateProperty<string>(() => this.instrumentRibbon.Instrument.Name);
            abbreviatedName = new ReferenceTemplateProperty<string>(() => CreateDefaultNickName(displayName.Value));
            numberOfStaves = new ValueTemplateProperty<int>(() => instrumentRibbon.Instrument.NumberOfStaves);
            Collapsed = false;
        }


        public static string CreateDefaultNickName(string name)
        {
            return string.IsNullOrWhiteSpace(name)
                ? ""
                : name.Length == 1 ? string.Concat(name.AsSpan(0, 1), ".") : string.Concat(name.AsSpan(0, 2), ".");
        }


        public InstrumentRibbonLayoutMemento GetMemento()
        {
            return new InstrumentRibbonLayoutMemento()
            {
                AbbreviatedName = abbreviatedName.Field,
                DisplayName = displayName.Field,
                NumberOfStaves = numberOfStaves.Field,
                Collapsed = Collapsed,
            };
        }

        public void ApplyMemento(InstrumentRibbonLayoutMemento memento)
        {
            abbreviatedName.Field = memento.AbbreviatedName;
            displayName.Field = memento.DisplayName;
            numberOfStaves.Field = memento.NumberOfStaves;
            Collapsed = memento.Collapsed ?? false;
        }

        public void Restore()
        {
            abbreviatedName.Reset();
            displayName.Reset();
            numberOfStaves.Reset();
            Collapsed = false;
        }
    }
}