using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Security.Policy;
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
        private  settingsForm settingsForm;
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

        
        private readonly float originalFormWidth = 1432f;  
        private readonly float originalFormHeight = 1055f; 

        private Dictionary<Control, float> originalFontSizes = new Dictionary<Control, float>();

        private void mainForm_Load(object sender, EventArgs e)
        {
            StoreOriginalSizesAndPositions(scanPanel);
            StoreOriginalSizesAndPositions(panel2);
            StoreOriginalSizesAndPositions(panel3);
   

            //if (!originalFontSizes.ContainsKey(lbl_appname))
            //{
            //    originalFontSizes[lbl_appname] = lbl_appname.Font.Size;
            //}

            AdjustFontSizes();
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            AdjustFontSizes();
        }

        private void StoreOriginalSizesAndPositions(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    if (!originalFontSizes.ContainsKey(label))
                    {
                        originalFontSizes[label] = label.Font.Size;
                    }
                }
            }
        }

        private void AdjustFontSizes()
        {
            float currentFormWidth = this.ClientSize.Width;
            float currentFormHeight = this.ClientSize.Height;

            
            float widthRatio = currentFormWidth / originalFormWidth;
            float heightRatio = currentFormHeight / originalFormHeight;

            float formRatio = Math.Max(widthRatio, heightRatio);

            AdjustPanelFontSizes(scanPanel, widthRatio, heightRatio, formRatio);
            AdjustPanelFontSizes(panel2, widthRatio, heightRatio, formRatio);
            AdjustPanelFontSizes(panel3, widthRatio, heightRatio, formRatio);
         

            if (originalFontSizes.ContainsKey(lbl_appname))
            {
                float originalFontSizeAppName = originalFontSizes[lbl_appname];
                //lbl_appname.Font = new Font(lbl_appname.Font.FontFamily, originalFontSizeAppName * formRatio);
                label3.Font = new Font(label3.Font.FontFamily, originalFontSizeAppName * formRatio);
            }
        }

        private void AdjustPanelFontSizes(Panel panel, float widthRatio, float heightRatio, float formRatio)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label3)
                {
                    if (originalFontSizes.ContainsKey(label3))
                    {
                        float originalFontSize = originalFontSizes[label3];
                        label3.Font = new Font(label3.Font.FontFamily, originalFontSize * formRatio);;
                    }

                    //{
                    //    if (control is Label lbl_appname)
                    //    {
                    //        if (originalFontSizes.ContainsKey(lbl_appname))
                    //        {
                    //            float originalFontSize = originalFontSizes[lbl_appname];
                    //            lbl_appname.Font = new Font(lbl_appname.Font.FontFamily, originalFontSize * formRatio); ;
                    //        }
                    //    }
                    //}


                }
            }
        }

    }

}