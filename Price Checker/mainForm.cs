using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Price_Checker.Configuration;
using Price_Checker.Services;

namespace Price_Checker
{
    public partial class mainForm : Form
    {
        private readonly ImagesManagerService imageManager;
        private readonly ScanBarcodeService scanBarcodeService;
        private readonly BarcodeTimer barcodeTimer;
        private readonly FontManagerService fontManager;
        private readonly VideoManagerService videoManager;
        private readonly ServerStatusService serverStatusManager;
        private settingsForm settingsForm;
        private bool isDefaultPictureBoxShown = false;

        private string connString = ConnectionStringService.ConnectionString;
        public mainForm()
        {
            InitializeComponent();
            lbl_barcode.KeyDown += Lbl_barcode_KeyDown;
            KeyPreview = true;
            this.Shown += MainForm_Shown;

            // Create an instance of the SettingsForm
            settingsForm = new settingsForm();

            // Open Settings
            this.KeyDown += SettingsForm_KeyDown;

            // Close main form
            this.KeyDown += MainForm_KeyDown;

            serverStatusManager = new ServerStatusService();

            this.Load += mainForm_Load;
            this.Resize += mainForm_Resize;
            UpdateStatusLabelPeriodically(); // Start the periodic status label update
            scanBarcodeService = new ScanBarcodeService();
            scanBarcodeService.BarcodeScanned += ScanBarcodeService_BarcodeScanned;
            barcodeTimer = new BarcodeTimer(lbl_barcode);
            imageManager = new ImagesManagerService(pictureBox1,connString);
            imageManager.LoadImageFiles();
            fontManager = new FontManagerService();
            lbl_barcode.Font = fontManager.GetCustomFont();
            videoManager = new VideoManagerService(axWindowsMediaPlayer1,pictureBox2);
        }
      
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Alt + Conrol + Backspace
            if (e.KeyData == (Keys.Alt | Keys.Control | Keys.Back))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to close the program?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Alt + Shift + Enter
            if (e.KeyData == (Keys.Alt | Keys.Shift | Keys.Enter))
            {
                settingsForm newSettingsForm = new settingsForm();
                newSettingsForm.Show();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Set focus to lbl_barcode when the form is shown
            lbl_barcode.Focus();
        }

        private void Lbl_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            scanBarcodeService.HandleBarcodeInput(e, lbl_barcode, scanPanel, this);
        }

        private void ScanBarcodeService_BarcodeScanned(object sender, string barcode)
        {
            barcodeTimer.StartTimer();
        }

      
        internal void HandleError(string errorMessage)
        {
            DialogResult result = MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private async void UpdateStatusLabelPeriodically()
        {
            while (true)
            {
                try
                {
                    serverStatusManager.Appname(lbl_appname);
                    serverStatusManager.UpdateStatusLabel(lbl_status, bottomPanel);
                  
                }
                catch (Exception ex)
                {
                    HandleError(ex.Message);
                    break; // Stop the loop if an error occurs
                }
                await Task.Delay(1000); //1sec
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            AdjustFontSizes();
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            AdjustFontSizes();
        }

        private void AdjustFontSizes()
        {
            // Get the screen resolution
            float screenWidth = Screen.PrimaryScreen.Bounds.Width;
            float screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calculate the screen ratio
            float screenRatio = screenWidth / screenHeight;

            // Adjust the font sizes based on the screen ratio
            // You can tweak the multiplication factors to suit your needs
            label3.Font = new Font(label3.Font.FontFamily, label3.Font.Size * screenRatio / 1.8f);
            label3.Location = new Point((int)(screenWidth / 2 - label3.Width / 2), 320);
            lbl_appname.Font = new Font(lbl_appname.Font.FontFamily, lbl_appname.Font.Size * screenRatio / 1.8f);
        }
    }
}