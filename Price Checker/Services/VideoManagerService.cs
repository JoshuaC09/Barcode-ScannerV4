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
    private string assetsFolder;
    private string appDirectory;
    private PictureBox pictureBox;
    private readonly DatabaseHelper dbHelper;

    public VideoManagerService(AxWindowsMediaPlayer player, PictureBox pictureBox)
    {
        mediaPlayer = player;
        this.pictureBox = pictureBox;
        dbHelper = new DatabaseHelper(ConnectionStringService.ConnectionString);

        mediaPlayer.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(AxWindowsMediaPlayer1_PlayStateChange);
        LoadVideoFilePaths();

        playbackTimer = new System.Timers.Timer();
        playbackTimer.Elapsed += PlaybackTimer_Elapsed;

        PlayNextVideoAsync().ConfigureAwait(false); // Fire and forget

        System.Timers.Timer updateTimer = new System.Timers.Timer
        {
            Interval = 500 // Check for updates every 1 second
        };
        updateTimer.Elapsed += async (sender, e) => await CheckAndUpdateFilePathAsync(sender, e);
        updateTimer.Start();
    }

    private async Task CheckAndUpdateFilePathAsync(object sender, System.Timers.ElapsedEventArgs e)
    {
        string updatedAssetsFolder = GetAssetsFolder();

        if (updatedAssetsFolder != assetsFolder)
        {
            assetsFolder = updatedAssetsFolder;
            videoQueue.Clear();
            videoFilePaths.Clear();

            string videosFolder = GetVideosFolder(assetsFolder);
            List<string> allVideoPaths = GetAllVideoPaths(videosFolder);

            List<string> invalidVideoPaths = allVideoPaths.Where(file => !IsValidFileName(file)).ToList();
            allVideoPaths = allVideoPaths.Except(invalidVideoPaths).ToList();

            videoFilePaths = SortVideoFilePaths(allVideoPaths, invalidVideoPaths);
            videoQueue = new Queue<string>(videoFilePaths);

            await PlayNextVideoAsync();
        }
    }

    private string GetAssetsFolder()
    {
        string query = "SELECT set_advid FROM settings";
        object result = dbHelper.ExecuteScalar(query);
        return result?.ToString().Replace("$", "\\");
    }

    private string GetVideosFolder(string assetsFolder)
    {
        if (string.IsNullOrEmpty(assetsFolder) || !Directory.Exists(assetsFolder) || !Directory.EnumerateFiles(assetsFolder).Any())
        {
            appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string defaultVideosFolder = Path.Combine(appDirectory, "b1_3#G@8yts", "st8y@G#3_1b");

            // Ensure the default directory exists
            if (!Directory.Exists(defaultVideosFolder))
            {
                return string.Empty;
            }

            return defaultVideosFolder;
        }
        return assetsFolder;
    }

    private List<string> GetAllVideoPaths(string videosFolder)
    {
        if (string.IsNullOrEmpty(videosFolder) || !Directory.Exists(videosFolder))
        {
            // Handle the case when videosFolder is empty or invalid
            // e.g., return an empty list, log an error, etc.
            return new List<string>();
        }
        var videoExtensions = new List<string> { "*.mp4", "*.avi", "*.mov", "*.mkv", "*.flv", "*.wmv", "*.m4v", "*.3gp", "*.ogv", "*.webm", "*.mpeg" };
        return videoExtensions.SelectMany(ext => Directory.EnumerateFiles(videosFolder, ext)).ToList();
    }

    private bool IsValidFileName(string file)
    {
        var fileNameParts = Path.GetFileNameWithoutExtension(file).Split('_');
        return fileNameParts.Length > 1 && int.TryParse(fileNameParts[0], out _);
    }

    private List<string> SortVideoFilePaths(List<string> validVideoPaths, List<string> invalidVideoPaths)
    {
        var sortedValidPaths = validVideoPaths.OrderBy(x => int.Parse(Path.GetFileNameWithoutExtension(x).Split('_')[0])).ToList();
        sortedValidPaths.AddRange(invalidVideoPaths);
        return sortedValidPaths;
    }

    public void LoadVideoFilePaths()
    {
        assetsFolder = GetAssetsFolder();

        string videosFolder = GetVideosFolder(assetsFolder);
        List<string> allVideoPaths = GetAllVideoPaths(videosFolder);

        List<string> validVideoPaths = allVideoPaths.Where(file => IsValidFileName(file)).ToList();
        List<string> invalidVideoPaths = allVideoPaths.Except(validVideoPaths).ToList();

        videoFilePaths = SortVideoFilePaths(validVideoPaths, invalidVideoPaths);
        videoQueue = new Queue<string>(videoFilePaths);
    }

    public async Task PlayNextVideoAsync()
    {

        while (videoQueue.Count == 0)
        {
            // Queue is empty, repopulate it
            videoQueue = new Queue<string>(videoFilePaths);
            if (videoQueue.Count == 0)
            {
                // No videos available, wait for a while before checking again
                await Task.Delay(5000); // Wait for 5 seconds
            }
        }

        if (videoQueue.Count > 0)
        {
            string videoPath = videoQueue.Dequeue();
            mediaPlayer.URL = videoPath;
            mediaPlayer.Ctlcontrols.play();
            mediaPlayer.uiMode = "none";
            mediaPlayer.stretchToFit = true;

            double duration = mediaPlayer.currentMedia?.duration ?? 0;
            if (duration > 0)
            {
                playbackTimer.Interval = (duration + 1) * 1000;
                playbackTimer.Start();
            }
            else
            {
                playbackTimer.Interval = GetAdvidTimeFromDatabase() ?? 100000;
                playbackTimer.Start();
            }
        }
    }

    private int? GetAdvidTimeFromDatabase()
    {
        string query = "SELECT set_advidtime FROM settings";
        object result = dbHelper.ExecuteScalar(query);

        if (result != null && int.TryParse(result.ToString(), out int seconds))
        {
            return ConvertSecondsToValue(seconds);
        }
        return 100000;
    }

    internal int ConvertSecondsToValue(int seconds)
    {
        return seconds >= 60 ? (seconds / 60) * 100000 : seconds * 1000;
    }

    private async void AxWindowsMediaPlayer1_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
    {
        if (e.newState == 8) // 8 represents MediaEnded state
        {
            await Task.Delay(100); // Wait for 100 milliseconds
            await PlayNextVideoAsync();
        }
        else if (e.newState == 0 || e.newState == 1) // 0 is Undefined, 1 is Stopped
        {
            pictureBox.Invoke((MethodInvoker)(() => pictureBox.Visible = string.IsNullOrEmpty(assetsFolder)));
        }
        else if (e.newState == 3) // 3 represents PlayingState
        {
            pictureBox.Invoke((MethodInvoker)(() => pictureBox.Visible = false));
        }
    }

    private async void PlaybackTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        playbackTimer.Stop(); // Stop the timer
        await PlayNextVideoAsync(); // Play the next video
    }
}