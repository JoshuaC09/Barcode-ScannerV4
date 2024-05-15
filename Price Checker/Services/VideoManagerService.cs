using AxWMPLib;
using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class VideoManagerService
{
    private System.Timers.Timer playbackTimer;
    private Queue<string> videoQueue = new Queue<string>();
    private List<string> videoFilePaths = new List<string>();
    private AxWindowsMediaPlayer mediaPlayer;

    const string invalidFileNameMessage = "Invalid filename(s)!\nRename to <number_name> (e.g. 1_video.jpg)";
    const string skippedFilesMessage = "Invalid names will be skipped";
    const string caption = "Invalid Video File Names";
    public VideoManagerService(AxWindowsMediaPlayer player)
    {
        mediaPlayer = player;
        mediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(AxWindowsMediaPlayer1_PlayStateChange);
        LoadVideoFilePaths();

        playbackTimer = new System.Timers.Timer();
        playbackTimer.Elapsed += PlaybackTimer_Elapsed;

        PlayNextVideo();
        System.Timers.Timer updateTimer = new System.Timers.Timer();
        updateTimer.Interval = 1000; // Check for updates every 1 second
        updateTimer.Elapsed += CheckAndUpdateFilePath;
        updateTimer.Start();
    }
    private string assetsFolder;
    private string appDirectory;
    string environment = System.Environment.CurrentDirectory;

    private void CheckAndUpdateFilePath(object sender, System.Timers.ElapsedEventArgs e)
    {
        string connstring = ConnectionStringService.ConnectionString;

        // Get the updated assetsFolder from the database
        string updatedAssetsFolder = GetAssetsFolder(connstring);

        if (updatedAssetsFolder != assetsFolder)
        {
            assetsFolder = updatedAssetsFolder;

            // Clear the existing videoQueue and videoFilePaths
            videoQueue.Clear();
            videoFilePaths.Clear();

            string videosFolder;
            if (string.IsNullOrEmpty(assetsFolder) || !Directory.EnumerateFiles(assetsFolder).Any())
            {
                string projectDirectory = Directory.GetParent(environment)?.Parent?.FullName;
                if (string.IsNullOrEmpty(projectDirectory))
                {
                    // Handle the case where the projectDirectory is null or empty
                    return;
                }

                appDirectory = projectDirectory;
                videosFolder = Path.Combine(appDirectory, "assets", "Videos");
            }
            else
            {
                videosFolder = assetsFolder;
            }


            // Fetch all video files in the updated directory
            List<string> videoExtensions = new List<string> { "*.mp4", "*.avi", "*.mov", "*.mkv", "*.flv", "*.wmv", "*.m4v", "*.3gp", "*.ogv", "*.webm", "*.mpeg" };
            List<string> allVideoPaths = new List<string>();

            foreach (string extension in videoExtensions)
            {
                allVideoPaths.AddRange(Directory.EnumerateFiles(videosFolder, extension));
            }

            // Filter out the files with invalid names
            List<string> invalidVideoPaths = allVideoPaths.Where(file => Path.GetFileNameWithoutExtension(file).Split('_').Length <= 1 || !int.TryParse(Path.GetFileNameWithoutExtension(file).Split('_')[0], out _)).ToList();
            allVideoPaths = allVideoPaths.Except(invalidVideoPaths).ToList();

            if (invalidVideoPaths.Any())
            {
                MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                // Sort the valid video file paths
                videoFilePaths = allVideoPaths.OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x).Split('_')[0])).ToList();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Populate the videoQueue with the updated video file paths
            videoQueue = new Queue<string>(videoFilePaths);

            // Play the next video from the updated file path
            PlayNextVideo();
        }
    }

    private string GetAssetsFolder(string connstring)
    {
        string assetsFolder = null;

        using (MySqlConnection con = new MySqlConnection(connstring))
        {
            con.Open();
            string sql = "SELECT set_advid FROM settings";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string setAdvid = reader.IsDBNull(0) ? null : reader.GetString(0);
                reader.Close();

                if (!string.IsNullOrEmpty(setAdvid))
                {
                    assetsFolder = setAdvid.Replace("$", "\\");
                }
            }
        }

        return assetsFolder;
    }
   
    public void LoadVideoFilePaths()
    {
        //string projectDirectory = Directory.GetParent(environment).Parent.FullName;
        // appDirectory = projectDirectory;

        string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string connstring = ConnectionStringService.ConnectionString;

        string assetsFolder = null;

        using (MySqlConnection con = new MySqlConnection(connstring))
        {
            con.Open();
            string sql = "SELECT set_advid FROM settings";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string setAdvid = reader.IsDBNull(0) ? null : reader.GetString(0);
                reader.Close();

                if (!string.IsNullOrEmpty(setAdvid))
                {

                    assetsFolder = setAdvid.Replace("$", "\\");
                }
            }
        }

        string videosFolder;
        if (string.IsNullOrEmpty(assetsFolder) || !Directory.EnumerateFiles(assetsFolder).Any())
        {
            // projectDirectory = Directory.GetParent(environment).Parent.FullName;
            //appDirectory = projectDirectory;
            videosFolder = Path.Combine(appDirectory, "assets", "Videos");
        }
        else
        {
            videosFolder = assetsFolder;
        }

        // Define the video file extensions you want to include
        List<string> videoExtensions = new List<string> { "*.mp4", "*.avi", "*.mov", "*.mkv", "*.flv", "*.wmv", "*.m4v", "*.3gp", "*.ogv", "*.webm", "*.mpeg" }; // Add or remove extensions as needed

        List<string> allVideoPaths = new List<string>();

        // Loop through each video extension and add the corresponding file paths to the list
        foreach (string extension in videoExtensions)
        {
            allVideoPaths.AddRange(Directory.EnumerateFiles(videosFolder, extension));
        }

        // Filter out the files with invalid names
        List<string> invalidVideoPaths = allVideoPaths.Where(file => Path.GetFileNameWithoutExtension(file).Split('_').Length <= 1 || !int.TryParse(Path.GetFileNameWithoutExtension(file).Split('_')[0], out _)).ToList();
        allVideoPaths = allVideoPaths.Except(invalidVideoPaths).ToList();

        if (invalidVideoPaths.Any())
        {
            // Show a message box informing the user about the required naming convention and skipped files
            MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        try
        {
            // Sort the valid video file paths
            videoFilePaths = allVideoPaths.OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x).Split('_')[0])).ToList();
        }
        catch (InvalidOperationException)
        {
            // This should not happen since we already filtered out invalid file names
            MessageBox.Show($"{invalidFileNameMessage}\n{skippedFilesMessage}", caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        videoQueue = new Queue<string>(videoFilePaths);
    }



    public void PlayNextVideo()
    {
        if (videoQueue.Count > 0)
        {
            string videoPath = videoQueue.Dequeue();
            mediaPlayer.URL = videoPath;
            mediaPlayer.Ctlcontrols.play();
            mediaPlayer.uiMode = "none";
            mediaPlayer.stretchToFit = true;

            // Check if the video duration is greater than 0
            if (mediaPlayer.currentMedia.duration > 0)
            {
                // Set the timer interval to the duration of the current video
                playbackTimer.Interval = (mediaPlayer.currentMedia.duration + 1) * 1000;
                playbackTimer.Start();
            }
            else
            {
                int? intervalFromDatabase = GetAdvidTimeFromDatabase();

                if (intervalFromDatabase == 0 || intervalFromDatabase == null)
                {
                    intervalFromDatabase = 100000; // or any other default value you want to use
                }

                playbackTimer.Interval = (double)intervalFromDatabase;
                playbackTimer.Start();
            }
        }
        else
        {
            // Instead of recursively calling PlayNextVideo(), reload the video file paths
            LoadVideoFilePaths();

            // If videoFilePaths is empty after reloading, play the current or default videos
            if (videoFilePaths.Count == 0)
            {
                // Play the current or default videos
                int? intervalFromDatabase = GetAdvidTimeFromDatabase();

                if (intervalFromDatabase == 0 || intervalFromDatabase == null)
                {
                    intervalFromDatabase = 100000; // or any other default value you want to use
                }

                playbackTimer.Interval = (double)intervalFromDatabase;
                playbackTimer.Start();
                return;
            }

            // Repopulate the queue with the video file paths
            videoQueue = new Queue<string>(videoFilePaths);
            PlayNextVideo();
        }
    }

    public int GetAdvidTimeFromDatabase()
    {
       
        string connstring = ConnectionStringService.ConnectionString;

        using (MySqlConnection con = new MySqlConnection(connstring))
        {
            con.Open();
            string sql = "SELECT set_advidtime FROM settings";
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

           
            return 100000; // or any other default value you want to use
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


    private async void AxWindowsMediaPlayer1_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
    {
        if (e.newState == 8) // 8 represents MediaEnded state
        {
            await Task.Delay(200); // Wait for 200 milliseconds
            PlayNextVideo();
        }
    }

    private void PlaybackTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        playbackTimer.Stop(); // Stop the timer
        PlayNextVideo(); // Play the next video
    }
}
