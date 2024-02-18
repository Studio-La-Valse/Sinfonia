using System.Xml.Linq;

namespace Sinfonia.Implementations.ScoreDocument.Factories.MusicXML
{
    internal class ScorePartXmlConverter
    {
        private readonly ScorePartMeasureXmlConverter measureConverter;

        public ScorePartXmlConverter(ScorePartMeasureXmlConverter measureConverter)
        {
            this.measureConverter = measureConverter;
        }

        public void Create(XElement scorePart, InstrumentRibbon ribbonEditor)
        {
            var durationOfOneQuarter = 4;
            foreach (var element in scorePart.Elements())
            {
                if (element.Name == "measure")
                {
                    var measureIndex = element.Attributes().Single(a => a.Name == "number").Value.ToIntOrThrow() - 1;
                    var measureToEdit = ribbonEditor.GetMeasureCore(measureIndex);
                    measureConverter.Create(element, measureToEdit, ref durationOfOneQuarter);
                }
            }
        }
    }
}