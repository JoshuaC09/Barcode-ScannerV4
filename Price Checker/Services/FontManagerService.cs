using System.Drawing.Text;
using System.Drawing;
using System.IO;

namespace Price_Checker.Services
{
    internal class FontManagerService
    {
        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();
        private readonly Font customFont;

        public FontManagerService()
        {
            customFont = LoadFont();
        }

        private Font LoadFont()
        {
            string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fontFilePath = Path.Combine(appDirectory, "assets", "Fonts", "Schibsted_Grotesk", "static", "SchibstedGrotesk-Regular.ttf");

            if (File.Exists(fontFilePath))
            {
                privateFontCollection.AddFontFile(fontFilePath);
                return new Font(privateFontCollection.Families[0], 28f);
            }
            else
            {
                throw new FileNotFoundException($"The font file 'SchibstedGrotesk-Regular.ttf' was not found in the directory '{Path.GetFullPath(Path.Combine(appDirectory, "assets", "Fonts", "Schibsted_Grotesk", "static"))}'.");
            }
        }

        public Font GetCustomFont() => customFont;
    }
}
