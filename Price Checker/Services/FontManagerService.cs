using System.Drawing.Text;
using System.Drawing;
using System.IO;
using System;

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
            string fontFilePath = ""; Path.Combine(appDirectory, "b1_3#G@8yts", "st8y@G#3_1b", "Schibsted_Grotesk", "static", "SchibstedGrotesk-Regular.ttf");

            if (File.Exists(fontFilePath))
            {
                privateFontCollection.AddFontFile(fontFilePath);
                return new Font(privateFontCollection.Families[0], 28f);
            }
            else
            {
                // Log a warning message or take appropriate action
                Console.WriteLine($"Warning: The font file 'SchibstedGrotesk-Regular.ttf' was not found in the directory '{Path.GetFullPath(Path.Combine(appDirectory, "assets", "Fonts", "Schibsted_Grotesk", "static"))}'. Using default font.");

                // Use Consolas as the default fallback font
                return new Font("Consolas", 28f, FontStyle.Regular);
            }
        }

        public Font GetCustomFont() => customFont;
    }
}