using Avalonia.Platform;
using PdfSharp.Fonts;
using System.IO;

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

            try
            {
                var fontLocation = faceName.Replace("#", "") + ".otf";
                var uri = new Uri(fontLocation);
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
            return new FontResolverInfo(familyName, isBold, isItalic);
        }
    }
}