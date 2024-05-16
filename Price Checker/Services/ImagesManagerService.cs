using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Price_Checker.Configuration
{
    internal class ImagesManagerService
    {
        private Queue<string> imageQueue = new Queue<string>();
        private readonly System.Windows.Forms.Timer imageLoopTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.PictureBox pictureBox1;
        private string assetsFolder;
        private readonly string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly string connstring = ConnectionStringService.ConnectionString;

        public ImagesManagerService(System.Windows.Forms.PictureBox pictureBox)
        {
            this.pictureBox1 = pictureBox;
            InitializeImageSlideshow();
        }

        private void InitializeImageSlideshow()
        {
            imageLoopTimer.Tick += DisplayNextImage;
            imageLoopTimer.Interval = GetAdpicTimeFromDatabase();
            imageLoopTimer.Start();

            var updateTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            updateTimer.Tick += UpdateAdpicTimeInterval;
            updateTimer.Tick += CheckAndUpdateFilePath;
            updateTimer.Start();

            LoadImageFiles();
        }

        private void UpdateAdpicTimeInterval(object sender, EventArgs e)
        {
            int newInterval = GetAdpicTimeFromDatabase();
            if (newInterval != imageLoopTimer.Interval)
            {
                imageLoopTimer.Interval = newInterval;
            }
        }

        private void CheckAndUpdateFilePath(object sender, EventArgs e)
        {
            string updatedAssetsFolder = GetAssetsFolder(connstring);
            if (updatedAssetsFolder != assetsFolder)
            {
                assetsFolder = updatedAssetsFolder;
                LoadImageFiles();
            }
        }

        internal void LoadImageFiles()
        {
            imageQueue.Clear();
            string imagesFolder = !string.IsNullOrEmpty(assetsFolder) && Directory.Exists(assetsFolder) && Directory.EnumerateFiles(assetsFolder).Any()
                ? assetsFolder
                : Path.Combine(appDirectory, "assets", "Images");

            var validImageFiles = Directory.EnumerateFiles(imagesFolder, "*.*")
                .Where(IsImageFile)
                .Where(IsValidFileName)
                .OrderBy(ParseFileName)
                .ToList();

            var invalidImageFiles = Directory.EnumerateFiles(imagesFolder, "*.*")
                .Where(IsImageFile)
                .Where(file => !IsValidFileName(file))
                .OrderBy(ParseFileName)
                .ToList();

            var allImageFiles = validImageFiles.Concat(invalidImageFiles).ToList();


            if (allImageFiles.Count == 0)
            {
                pictureBox1.Image = Properties.Resources.ads_here;
            }
            else
            {
                foreach (string imagePath in allImageFiles)
                {
                    imageQueue.Enqueue(imagePath);
                }

                DisplayNextImage(null, EventArgs.Empty);
            }
        }

        private string GetAssetsFolder(string connstring)
        {
            using (var con = new MySqlConnection(connstring))
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT set_adpic FROM settings", con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string setAdPic = reader.IsDBNull(0) ? null : reader.GetString(0);
                        return !string.IsNullOrEmpty(setAdPic) ? setAdPic.Replace("$", "\\") : null;
                    }
                }
            }
            return null;
        }

        private int GetAdpicTimeFromDatabase()
        {
            using (var con = new MySqlConnection(connstring))
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT set_adpictime FROM settings", con);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int seconds))
                {
                    return ConvertSecondsToValue(seconds);
                }

                return 10000; // Default value
            }
        }

        private int ConvertSecondsToValue(int seconds)
        {
            return seconds >= 60 ? (seconds / 60) * 100000 : seconds * 1000;
        }

        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" }.Contains(extension);
        }

        private bool IsValidFileName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string[] parts = fileName.Split('_');

            return parts.Length <= 2 && int.TryParse(parts[0], out _);
        }

        private int ParseFileName(string filePath)
        {
            try
            {
                var parts = Path.GetFileNameWithoutExtension(filePath).Split('_');
                return int.Parse(parts[0]);
            }
            catch
            {
                // Return a large number to push invalid files to the end
                return int.MaxValue;
            }
        }

        private void DisplayNextImage(object sender, EventArgs e)
        {
            if (imageQueue.Count == 0)
            {
                LoadImageFiles();
            }
            if (imageQueue.Count > 0)
            {
                string imagePath = imageQueue.Dequeue();
                try
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);
                    imageQueue.Enqueue(imagePath); // Add the image back to the end of the queue
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show($"Skipping image: {imagePath} due to out of memory exception.", "Error: Out of Memory!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
