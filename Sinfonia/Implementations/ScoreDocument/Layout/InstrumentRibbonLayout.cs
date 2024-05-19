using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public abstract class BaseInstrumentRibbonLayout
    {
        public abstract ReferenceTemplateProperty<string> abbreviatedName { get; }
        public abstract ReferenceTemplateProperty<string> displayName { get; }
        public abstract ValueTemplateProperty<int> numberOfStaves { get; }
        public abstract ValueTemplateProperty<bool> collapsed { get; }


        public string AbbreviatedName { get => abbreviatedName.Value; set => abbreviatedName.Value = value; }
        public string DisplayName { get => displayName.Value; set => displayName.Value = value; }
        public int NumberOfStaves { get => numberOfStaves.Value; set => numberOfStaves.Value = value; }
        public bool Collapsed { get => collapsed.Value; set => collapsed.Value = value; }
        public string Name { get => displayName.Value; set => displayName.Value = value; }


        public BaseInstrumentRibbonLayout()
        {
            
        }

        public void Restore()
        {
            abbreviatedName.Reset();
            displayName.Reset();
            numberOfStaves.Reset();
            collapsed.Reset();
        }

        public void ApplyMemento(InstrumentRibbonLayoutMembers? memento)
        {
            Restore();
            if(memento is null)
            {
                return;
            }

            abbreviatedName.Field = memento.AbbreviatedName;
            displayName.Field = memento.DisplayName;
            numberOfStaves.Field = memento.NumberOfStaves;
            collapsed.Field = memento.Collapsed;
        }
        public void ApplyMemento(InstrumentRibbonLayoutModel? memento)
        {
            ApplyMemento(memento as InstrumentRibbonLayoutMembers);
        }
    }

    public class InstrumentRibbonLayout : BaseInstrumentRibbonLayout 
    {
        public override ReferenceTemplateProperty<string> abbreviatedName { get; }
        public override ReferenceTemplateProperty<string> displayName { get; }
        public override ValueTemplateProperty<int> numberOfStaves { get; }
        public override ValueTemplateProperty<bool> collapsed { get; }

        public InstrumentRibbonLayout(Instrument instrument)
        {
            displayName = new ReferenceTemplateProperty<string>(() => instrument.Name);
            numberOfStaves = new ValueTemplateProperty<int>(() => instrument.NumberOfStaves);
            collapsed = new ValueTemplateProperty<bool>(() => false);
            abbreviatedName = new ReferenceTemplateProperty<string>(() => CreateDefaultNickName(displayName.Value));
        }

        public static string CreateDefaultNickName(string name)
        {
            return string.IsNullOrWhiteSpace(name)
                ? ""
                : name.Length == 1 ? string.Concat(name.AsSpan(0, 1), ".") : string.Concat(name.AsSpan(0, 2), ".");
        }
    }

    public class SecondaryInstrumentRibbonLayout : BaseInstrumentRibbonLayout, IInstrumentRibbonLayout, ILayout<InstrumentRibbonLayoutModel>
    {
        public Guid Id { get; }
        public override ReferenceTemplateProperty<string> abbreviatedName { get; }
        public override ReferenceTemplateProperty<string> displayName { get; }
        public override ValueTemplateProperty<int> numberOfStaves { get; }
        public override ValueTemplateProperty<bool> collapsed { get; }

        public SecondaryInstrumentRibbonLayout(InstrumentRibbonLayout layout, Guid id)
        {
            Id = id;
            displayName = new ReferenceTemplateProperty<string>(() => layout.Name);
            numberOfStaves = new ValueTemplateProperty<int>(() => layout.NumberOfStaves);
            collapsed = new ValueTemplateProperty<bool>(() => layout.Collapsed);
            abbreviatedName = new ReferenceTemplateProperty<string>(() => layout.AbbreviatedName);
        }



        public InstrumentRibbonLayoutModel GetMemento()
        {
            return new InstrumentRibbonLayoutModel()
            {
                Id = Id,
                AbbreviatedName = abbreviatedName.Field,
                DisplayName = displayName.Field,
                NumberOfStaves = numberOfStaves.Field,
                Collapsed = Collapsed,
            };
        }
    }
}