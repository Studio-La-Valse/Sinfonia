using System.Xml.Linq;

namespace Sinfonia.Implementations.ScoreDocument.Factories.MusicXML
{
    internal class ScoreDocumentXmlConverter
    {
        private readonly ScorePartXmlConverter scorePartXmlConverter;

        public ScoreDocumentXmlConverter(ScorePartXmlConverter scorePartXmlConverter)
        {
            this.scorePartXmlConverter = scorePartXmlConverter;
        }

        public void Create(XDocument xDocument, ScoreDocumentCore scoreEditor)
        {
            scoreEditor.Clear();

            foreach (var element in xDocument.Elements())
            {
                if (element.Name == "score-partwise")
                {
                    ProcessScorePartWise(element, scoreEditor);
                    return;
                }
            }

            throw new Exception("No score-partwise found in music xml.");
        }

        private void ProcessScorePartWise(XElement scorePartwise, ScoreDocumentCore scoreEditor)
        {
            foreach (var element in scorePartwise.Elements())
            {
                if (element.Name == "part-list")
                {
                    PrepareParts(element, scoreEditor);
                }
            }

            foreach (var element in scorePartwise.Elements())
            {
                if (element.Name == "part")
                {
                    PrepareMeasures(element, scoreEditor);
                    break;
                }
            }

            foreach (var element in scorePartwise.Elements())
            {
                if (element.Name == "part")
                {
                    var id = element.Attributes().Single(a => a.Name == "id").Value;
                    var ribbon = scoreEditor.EnumerateRibbonsCore().First(r => r.DisplayName == id);
                    scorePartXmlConverter.Create(element, ribbon);
                }
            }
        }

        private void PrepareParts(XElement partList, ScoreDocumentCore scoreEditor)
        {
            var partNodes = partList.Elements().Where(d => d.Name == "score-part");

            foreach (var partListNode in partNodes)
            {
                var name = partListNode.Elements().Single(d => d.Name == "part-name").Value;
                var instrument = Instrument.TryGetFromName(name);
                scoreEditor.AddInstrumentRibbon(instrument);

                var ribbon = scoreEditor.GetInstrumentRibbonCore(scoreEditor.NumberOfInstruments - 1);
                ribbon.DisplayName = partListNode.Attributes().Single(a => a.Name == "id").Value;
            }
        }

        private void PrepareMeasures(XElement part, ScoreDocumentCore scoreEditor)
        {
            var lastKeySignature = 0;
            var lastBeats = 4;
            var lastBeatsType = 4;

            var measures = part.Elements().Where(e => e.Name == "measure");
            var n = 0;
            foreach (var measure in measures)
            {
                lastKeySignature = part.Descendants().FirstOrDefault(d => d.Name == "fifths")?.Value.ToIntOrNull() ?? lastKeySignature;
                lastBeats = part.Descendants().FirstOrDefault(d => d.Name == "beats")?.Value.ToIntOrNull() ?? lastBeats;
                lastBeatsType = part.Descendants().FirstOrDefault(d => d.Name == "beat-type")?.Value.ToIntOrNull() ?? lastBeatsType;

                var newSystem = part.Descendants().FirstOrDefault(d => d.Name == "print")?.Attribute("new-system")?.Value.Equals("yes") ?? false;
                var newPage = part.Descendants().FirstOrDefault(d => d.Name == "print")?.Attribute("new-page")?.Value.Equals("yes") ?? false;
                var timeSignature = new TimeSignature(lastBeats, lastBeatsType);
                scoreEditor.AppendScoreMeasure(timeSignature);

                var appendedMeasure = scoreEditor.GetScoreMeasureCore(scoreEditor.NumberOfMeasures - 1);
                var keySignature = new KeySignature(Step.C.MoveAlongCircleOfFifths(lastKeySignature), MajorOrMinor.Major);
                var width = measure.Attribute("width")?.Value.ToIntOrNull();

                appendedMeasure.Width = width ?? 100;
                appendedMeasure.IsNewSystem = n % 4 == 0;
                appendedMeasure.KeySignature = keySignature;

                n++;
            }
        }
    }
}