using PdfSharp.Fonts;

namespace Sinfonia.Implementations.PDF
{
    public class FontResolver : IFontResolver
    {
        private readonly SystemFontResolver systemFontResolver;
        private readonly ResourceFontResolver resourceFontResolver;

        public FontResolver(SystemFontResolver systemFontResolver, ResourceFontResolver resourceFontResolver)
        {
            this.systemFontResolver = systemFontResolver;
            this.resourceFontResolver = resourceFontResolver;
        }

        public byte[]? GetFont(string faceName)
        {
            var bytes = resourceFontResolver.GetFont(faceName);
            if (bytes is not null)
            {
                return bytes;
            }

            bytes = systemFontResolver.GetFont(faceName);
            if (bytes is not null)
            {
                return bytes;
            }

            throw new Exception();
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var info = resourceFontResolver.ResolveTypeface(familyName, isBold, isItalic);
            if (info is not null)
            {
                return info;
            }

            info = systemFontResolver.ResolveTypeface(familyName, isBold, isItalic);
            if (info is not null)
            {
                return info;
            }

            throw new Exception();
        }
    }
}