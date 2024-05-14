using System.Drawing.Text;
using System.Drawing;
using System.IO;

namespace Price_Checker.Services
{
    internal class FontManagerService
    {
        private PrivateFontCollection privateFontCollection;
        private Font customFont;

        public FontManagerService()
        {
            privateFontCollection = new PrivateFontCollection();
            LoadFont();
        }

        private void LoadFont()
        {

            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
            // Get the directory path of the currently executing assembly
               string appDirectory = projectDirectory;
            //string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Construct the font file path relative to the application directory
            string fontFilePath = Path.Combine(appDirectory, "assets", "Fonts", "Schibsted_Grotesk", "static", "SchibstedGrotesk-Regular.ttf");

            if (File.Exists(fontFilePath))
            {
                privateFontCollection.AddFontFile(fontFilePath);
                customFont = new Font(privateFontCollection.Families[0], 28f);
            }
            else
            {
                // Handle the case where the font file is not found
                throw new FileNotFoundException($"The font file 'SchibstedGrotesk-Regular.ttf' was not found in the directory '{Path.GetFullPath(Path.Combine(appDirectory, "assets", "Fonts", "Schibsted_Grotesk", "static"))}'.");
            }
        }

        public Font GetCustomFont()
        {
            return customFont;
        }
    }
}