using PdfSharp.Fonts;
using System;
using System.IO;

public class FontHelper : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        using (var stream = File.OpenRead("wwwroot/fonts/tahoma.ttf"))
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/fonts/tahoma.ttf");
        return new FontResolverInfo(fontPath);
    }
}