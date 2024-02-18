using System.Xml.Linq;

namespace Sinfonia.Implementations.ScoreDocument.Factories.MusicXML
{
    internal class BlockChainXmlConverter
    {
        public void ProcessElements(IEnumerable<XElement> elements, MeasureBlockChain editor, int divisionsOfOneQuarter)
        {
            var position = new Position(0, 4);
            foreach (var element in elements)
            {
                ProcessMeasureElement(element, editor, divisionsOfOneQuarter, ref position);
            }
        }

        private void ProcessMeasureElement(XElement measureElement, MeasureBlockChain measureBlockChain, int divisionsOfOneQuarter, ref Position position)
        {
            if (!measureElement.IsNoteOrForwardOrBackup())
            {
                return;
            }

            GetRythmicInformationFromNode(measureElement, divisionsOfOneQuarter, out var actualDuration, out var displayDuration, out var grace);

            GetInformationFromMeasureElement(measureElement, out var chord, out var rest, out var staff, out var forward, out var backup, out var pitch, out var stemUp);

            if (backup)
            {
                position -= actualDuration;
                return;
            }

            if (forward)
            {
                position += actualDuration;
                return;
            }

            if (chord && !grace)
            {
                position -= actualDuration;
            }

            var _position = new Position(position.Numinator, position.Denominator);
            var measureBlock = measureBlockChain.GetBlocksCore().First(b => b.ContainsPosition(_position));

            //create a new chord in the block if the no 'chord' attribute is specified, or if there are no chords in the block.
            if (!chord || !measureBlock.GetChordsCore().Any())
            {
                if (displayDuration is null)
                {
                    if (!RythmicDuration.TryConstruct(actualDuration, out displayDuration))
                    {
                        throw new Exception("A valid rythmic duration is required for a note or measure element with a duration, but could not be found.");
                    }
                }

                measureBlock.AppendChord(displayDuration);
            }

            //Always add to the last chord in the block
            var chordDocument = measureBlock.GetChordsCore().Last();

            if (pitch is not null)
            {
                chordDocument.Add(pitch);
                var note = chordDocument.EnumerateNotesCore().Single(n => n.Pitch.Equals(pitch));
                note.StaffIndex = staff ?? 0;
            }

            if (!grace)
            {
                position += actualDuration;
            }
        }



        private static void GetRythmicInformationFromNode(XElement measureElement, int durationOfOneQuarter, out Fraction actualDuration, out RythmicDuration? displayDuration, out bool grace)
        {
            displayDuration = null;
            grace = measureElement.Descendants().Any(d => d.Name == "grace");

            var typeString = measureElement.Descendants().SingleOrDefault(d => d.Name == "type")?.Value;
            Tuplet? tupletInformation = null;
            if (measureElement.Descendants().SingleOrDefault(d => d.Name == "time-modification") is XElement timeModificationElement)
            {
                //bijvoorbeeld 3, van 3 achtsten in de plaats van 1 kwart = 2 achtsten
                var actualNumberValue = timeModificationElement.Descendants().Single(d => d.Name == "actual-notes").Value;
                // bijvoorbeeld 2, van 3 achtsten in de plaats van 1 kwart = 2 achtsten
                var normalNumberValue = timeModificationElement.Descendants().Single(d => d.Name == "normal-notes").Value;
                // bijvoorbeel 8, van 3 achtsten in de plaats van 1 kwart = 2 achtsten
                var normalTypeValue = timeModificationElement.Descendants().SingleOrDefault(d => d.Name == "normal-type")?.Value;

                var normalRythmicType = normalTypeValue ?? typeString ?? throw new Exception("Cannot handle tuplet information if the type xml node does not exist.");
                var normalRythmicDuration = normalRythmicType.FromTypeString();
                var normalRythmicDots = timeModificationElement.Descendants().Where(d => d.Name == "normal-dot").Count();
                var normalDuration = new RythmicDuration(normalRythmicDuration, normalRythmicDots);
                var tupletContent = Enumerable.Range(0, actualNumberValue.ToIntOrThrow())
                    .Select(e => normalDuration)
                    .ToArray();
                tupletInformation = new Tuplet(normalDuration, tupletContent);
            }

            if (typeString is null)
            {
                var _duration = measureElement.Descendants().FirstOrDefault(d => d.Name == "duration");
                var noteDuration = _duration?.Value.ToIntOrNull() ?? 0;
                actualDuration = new Duration(noteDuration, durationOfOneQuarter * 4).Simplify();
            }
            else
            {
                var type = typeString.FromTypeString();
                var dots = measureElement.Descendants().Where(d => d.Name == "dot").Count();
                displayDuration = new RythmicDuration(type, dots);
                actualDuration = displayDuration;
                if (tupletInformation is not null)
                {
                    actualDuration = tupletInformation.ToActualDuration(displayDuration);
                }
            }
        }

        private static void GetInformationFromMeasureElement(XElement measureElement,
            out bool chord, out bool rest, out int? staff, out bool forward, out bool backup, out Pitch? pitch, out bool stemUp)
        {
            pitch = null;
            chord = measureElement.Element("chord") != null;
            rest = measureElement.Element("rest") != null;
            stemUp = measureElement.Descendants().SingleOrDefault(d => d.Name == "stem")?.Value == "up";

            forward = measureElement.Name == "forward";
            backup = measureElement.Name == "backup";

            staff = measureElement.Descendants().SingleOrDefault(d => d.Name == "staff")?.Value.ToIntOrThrow() - 1;
            if (forward || backup || rest)
            {
                return;
            }

            pitch = measureElement.ParsePitch();
        }
    }
}