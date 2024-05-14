using AxWMPLib;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Price_Checker.Configuration
{
    internal class ImagesManagerService
    {
        private Queue<string> imageQueue = new Queue<string>();
        private readonly Dictionary<string, string> imagePaths = new Dictionary<string, string>();
        private System.Windows.Forms.Timer imageLoopTimer;
        private readonly System.Windows.Forms.PictureBox pictureBox1;

        const string invalidFileNameMessage = "Invalid filename(s)!\nRename to <number_name> (e.g. 1_image.jpg)";
        const string skippedFilesMessage = "Invalid names will be skipped";
        const string caption = "Invalid Image File Names";

        public ImagesManagerService(System.Windows.Forms.PictureBox pictureBox)
        {
            this.pictureBox1 = pictureBox;
        }

        private void UpdateAdpicTimeInterval(object sender, EventArgs e)
        {
            int newInterval = GetAdpicTimeFromDatabase();
            if (newInterval != imageLoopTimer.Interval)
            {
                imageLoopTimer.Interval = newInterval;
            }
        }

        private string assetsFolder = null; // Remove the = null part
        private string appDirectory;
        string enviroment = System.Environment.CurrentDirectory;
        
        public void ImageSlideshow()
        {
            imageLoopTimer = new System.Windows.Forms.Timer();
            imageLoopTimer.Tick += DisplayNextImage;

            // Set the initial interval to the default value or retrieve it from the database
            int initialInterval = GetAdpicTimeFromDatabase();
            if (initialInterval == 0)
            {
                initialInterval = 10000; // or any other default value you want to use
            }
            imageLoopTimer.Interval = initialInterval;

            imageLoopTimer.Start();

            Timer updateTimer = new Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += UpdateAdpicTimeInterval;
            updateTimer.Tick += CheckAndUpdateFilePath;
            updateTimer.Start();


            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;

            appDirectory = projectDirectory;

            // string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            DatabaseConfig _config = new DatabaseConfig();
            string connstring = ConnectionStringService.ConnectionString;

            // Initialize assetsFolder with the initial value from the database
            string assetsFolder = GetAssetsFolder(connstring);

            string imagesFolder;
            if (string.IsNullOrEmpty(assetsFolder) || !Directory.EnumerateFiles(assetsFolder).Any())
            {
                imagesFolder = Path.Combine(appDirectory, "assets", "Images");
            }
            else
            {
                imagesFolder = assetsFolder;
            }

            // Fetch all image files in the specified directory
            List<string> imageFiles = Directory.EnumerateFiles(imagesFolder, "*.*")
                .Where(file => IsImageFile(file))
                .ToList();

            bool hasInvalidFileNames = imageFiles.Any(file => !IsValidFileName(file));
            imageFiles = imageFiles.Where(file => IsValidFileName(file)).ToList();
            try
            {
                if (hasInvalidFileNames)
                {
                    MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Sort the image files based on the numeric prefix
                imageFiles.Sort((x, y) =>
                {
                    string xFileName = Path.GetFileNameWithoutExtension(x);
                    string yFileName = Path.GetFileNameWithoutExtension(y);

                    int xPrefix = int.Parse(xFileName.Split('_')[0]);
                    int yPrefix = int.Parse(yFileName.Split('_')[0]);

                    return xPrefix.CompareTo(yPrefix);
                });
            }
            catch (InvalidOperationException)
            {
                // Show a message box informing the user about the required naming convention
                MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Populate the imageQueue with the sorted image file paths
            foreach (string imagePath in imageFiles)
            {
                imageQueue.Enqueue(imagePath);
            }

            // Display the first image
            DisplayNextImage(null, EventArgs.Empty);
        }

        private void CheckAndUpdateFilePath(object sender, EventArgs e)
        {
            DatabaseConfig _config = new DatabaseConfig();
            string connstring = ConnectionStringService.ConnectionString;

            // Get the updated assetsFolder from the database
            string updatedAssetsFolder = GetAssetsFolder(connstring);

            if (updatedAssetsFolder != assetsFolder)
            {
                assetsFolder = updatedAssetsFolder;

                // Clear the existing imageQueue
                imageQueue.Clear();

                string imagesFolder;
                if (string.IsNullOrEmpty(assetsFolder) || !Directory.EnumerateFiles(assetsFolder).Any())
                {
                    imagesFolder = assetsFolder;
                }
                else
                {
                    imagesFolder = assetsFolder;
                }

                // Fetch all image files in the updated directory
                List<string> imageFiles = Directory.EnumerateFiles(imagesFolder, "*.*")
                    .Where(file => IsImageFile(file) && IsValidFileName(file))
                    .ToList();

                // Sort the image files based on the numeric prefix
                imageFiles.Sort((x, y) =>
                {
                    string xFileName = Path.GetFileNameWithoutExtension(x);
                    string yFileName = Path.GetFileNameWithoutExtension(y);

                    int xPrefix = int.Parse(xFileName.Split('_')[0]);
                    int yPrefix = int.Parse(yFileName.Split('_')[0]);

                    return xPrefix.CompareTo(yPrefix);
                });

                // Populate the imageQueue with the updated image file paths
                foreach (string imagePath in imageFiles)
                {
                    imageQueue.Enqueue(imagePath);
                }

                // Display the first image from the updated file path
                DisplayNextImage(null, EventArgs.Empty);
            }
        }

        private string GetAssetsFolder(string connstring)
        {
            string assetsFolder = null;

            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                con.Open();
                string sql = "SELECT set_adpic FROM settings";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string setAdPic = reader.IsDBNull(0) ? null : reader.GetString(0);
                    reader.Close();

                    if (!string.IsNullOrEmpty(setAdPic))
                    {
                        assetsFolder = setAdPic.Replace("$", "\\");
                    }
                }
            }

            return assetsFolder;
        }
        public int GetAdpicTimeFromDatabase()
        {
            DatabaseConfig _config = new DatabaseConfig();
            string connstring = ConnectionStringService.ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                con.Open();
                string sql = "SELECT set_adpictime FROM settings";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    if (int.TryParse(result.ToString(), out int seconds))
                    {

                        int convertedValue = ConvertSecondsToValue(seconds);
                        return convertedValue;
                    }
                }

                return 10000; // or any other default value you want to use
            }
        }


        internal int ConvertSecondsToValue(int seconds)
        {
            if (seconds >= 60)
            {
                int minutes = seconds / 60;
                return minutes * 100000;
            }
            else
            {
                return seconds * 1000;
            }
        }

        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" }.Contains(extension);
        }

        private bool IsValidFileName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath).ToLower();

            if (!IsImageFile(filePath))
            {
                return false;
            }

            string[] parts = fileName.Split('_');

            if (parts.Length == 1)
            {
                // File name does not contain an underscore, so check if it's a valid number
                return int.TryParse(parts[0], out _);
            }
            else if (parts.Length == 2)
            {
                // File name contains an underscore, check if the first part is a valid number
                return int.TryParse(parts[0], out _);
            }

            return false;
        }

        public void DisplayNextImage(object sender, EventArgs e)
        {
            if (imageQueue.Count == 0)
            {
                // Repopulate the imageQueue with the image file paths
                imageQueue = new Queue<string>(imagePaths.Values);
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
                    MessageBox.Show($"Skipping image: {imagePath} due to out of memory exception.", "Error: Out of Memory! ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
    }
}