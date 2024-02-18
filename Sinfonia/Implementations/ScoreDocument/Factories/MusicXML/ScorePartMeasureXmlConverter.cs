using System.Xml.Linq;

namespace Sinfonia.Implementations.ScoreDocument.Factories.MusicXML
{
    internal class ScorePartMeasureXmlConverter
    {
        private readonly BlockChainXmlConverter blockChainXmlConverter;

        public ScorePartMeasureXmlConverter(BlockChainXmlConverter blockChainXmlConverter)
        {
            this.blockChainXmlConverter = blockChainXmlConverter;
        }

        public void Create(XElement measure, InstrumentMeasure measureEditor, ref int durationOfOneQuarter)
        {
            measureEditor.Clear();

            var attributes = measure.Elements().FirstOrDefault(e => e.Name == "attributes");
            if (attributes is not null)
            {
                ProcessMeasureAttributes(attributes, ref durationOfOneQuarter, out var clefChanges);
                foreach (var clefChange in clefChanges)
                {
                    measureEditor.AddClefChange(clefChange);
                }
            }

            var voices = measure.ExtractVoices();
            foreach (var voice in voices)
            {
                measureEditor.AddVoice(voice);
                var blockChainEditor = measureEditor.GetBlockChainOrThrowCore(voice);
                blockChainEditor.DivideEqual(measureEditor.TimeSignature.Denominator);

                var elements = measure.ExtractElements(voice);
                blockChainXmlConverter.ProcessElements(elements, blockChainEditor, durationOfOneQuarter);

                foreach (var block in blockChainEditor.GetBlocksCore())
                {
                    block.Rebeam();
                }
            }
        }

        private void ProcessMeasureAttributes(XElement measureAttributes, ref int divisions, out IEnumerable<ClefChange> clefChanges)
        {
            var _clefChanges = new List<ClefChange>();

            foreach (var element in measureAttributes.Elements())
            {
                if (element.Name == "clef")
                {
                    var sign = element.Descendants().Single(d => d.Name == "sign").Value.ToLower();
                    var clef = sign switch
                    {
                        "g" => Clef.Treble,
                        "f" => Clef.Bass,
                        _ => throw new NotSupportedException("Unknown clef species found in XML document: " + sign)
                    };
                    var staff = element.Attributes().Single(a => a.Name == "number").Value.ToIntOrThrow() - 1;
                    var clefChange = new ClefChange(clef, staff, new Position(0, 4));
                    _clefChanges.Add(clefChange);
                }

                if (element.Name == "divisions")
                {
                    divisions = element.Value.ToIntOrThrow();
                }
            }

            clefChanges = _clefChanges;
        }
    }
}