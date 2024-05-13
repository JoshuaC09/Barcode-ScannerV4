using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        public void ImageSlideshow()
        {
            imageLoopTimer = new System.Windows.Forms.Timer();
            imageLoopTimer.Interval = 2000; // 5 seconds
            imageLoopTimer.Tick += DisplayNextImage;
            imageLoopTimer.Start();


            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;

            // Get the directory path of the currently executing assembly
            string appDirectory = projectDirectory;




            string imagesFolder = Path.Combine(appDirectory, "assets", "Images");

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
                    pictureBox1.Image = Image.FromFile(imagePath);
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