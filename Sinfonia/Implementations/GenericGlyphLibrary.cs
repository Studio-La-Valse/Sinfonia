using StudioLaValse.ScoreDocument.GlyphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Implementations;
internal class GenericGlyphLibrary : BaseGlyphLibrary
{
    private readonly IScoreDocumentLayout scoreDocumentLayout;

    public GenericGlyphLibrary(IScoreDocument scoreDocumentLayout)
    {
        this.scoreDocumentLayout = scoreDocumentLayout;
    }

    private readonly Dictionary<string, Uri> fontFamilyKeys = new()
    {
        { "#Bravura", new Uri("avares://Sinfonia/Resources/Fonts/bravura") },
        { "#Campania", new Uri("avares://Sinfonia/Resources/Fonts/campania") },
        { "#Edwin", new Uri("avares://Sinfonia/Resources/Fonts/edwin") },
        { "#Finale Broadway", new Uri("avares://Sinfonia/Resources/Fonts/finalebroadway") },
        { "#Finale Maestro", new Uri("avares://Sinfonia/Resources/Fonts/finalemaestro") }
    };

    public override Uri FontFamilyKey => fontFamilyKeys[FontFamily];

    public override string FontFamily => "#" + scoreDocumentLayout.GlyphFamily;
}
