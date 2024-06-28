using Avalonia;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.Platform;
using PdfSharp.Fonts;
using System;
using System.IO;
using System.Reflection;

namespace Sinfonia.Implementations.PDF
{
    public class ResourceFontResolver : IFontResolver
    {
        public ResourceFontResolver()
        {

        }

        public byte[]? GetFont(string faceName)
        {
            if (!faceName.StartsWith("avares:/"))
            {
                return null;
            }

            if(!Uri.IsWellFormedUriString(faceName, UriKind.Absolute))
            {
                return null;
            }

            try
            {
                var uri = new Uri(faceName);
                using var stream = AssetLoader.Open(uri);
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();
                return bytes;
            }
            catch
            {
                return null;
            }
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic) 
        {
            var resolverInfo = familyName switch
            {
                "avares://Sinfonia/Resources/Fonts/bravura#Bravura" => new("avares://Sinfonia/Resources/Fonts/bravura/Bravura.otf"),
                "avares://Sinfonia/Resources/Fonts/campania#Campania" => new("avares://Sinfonia/Resources/Fonts/campania/Campania.otf"),
                "avares://Sinfonia/Resources/Fonts/edwin#Edwin" =>  CreateEdwin(isBold, isItalic),
                "avares://Sinfonia/Resources/Fonts/finalebroadway#Finale Broadway" => new("avares://Sinfonia/Resources/Fonts/finalebroadway/FinaleBroadway.otf"),
                "avares://Sinfonia/Resources/Fonts/finalemaestro#Finale Maestro" => new("avares://Sinfonia/Resources/Fonts/finalemaestro/FinaleMaestro.otf"),
                _ => null
            };

            return resolverInfo;
        }

        private static FontResolverInfo? CreateEdwin(bool isBold, bool isItalic)
        {
            if (isBold && isItalic)
            {
                return new("avares://Sinfonia/Resources/Fonts/edwin/Edwin-Bdlta.otf");
            }
            else
            {
                if (isBold)
                {
                    return new("avares://Sinfonia/Resources/Fonts/edwin/Edwin-Bold.otf");
                }
                
                if(isItalic)
                {
                    return new("avares://Sinfonia/Resources/Fonts/edwin/Edwin-Italic.otf");
                }

                return new("avares://Sinfonia/Resources/Fonts/edwin/Edwin-Roman.otf");
            }
        }
    }
}