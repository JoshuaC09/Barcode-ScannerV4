﻿using System;
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

        public mainForm()
        {
            InitializeComponent();
            lbl_barcode.KeyDown += Lbl_barcode_KeyDown;
            KeyPreview = true;
            this.Shown += MainForm_Shown;

            // Create an instance of the SettingsForm
            settingsForm = new settingsForm();

            // Add the KeyDown event handler to the main form
            this.KeyDown += MainForm_KeyDown;

            serverStatusManager = new ServerStatusService();
            serverStatusManager.UpdateStatusLabel(lbl_status, bottomPanel, lbl_appname); // Call the UpdateStatusLabel method
            serverStatusManager.Appname(lbl_appname); // Call the Appname method
            // ... other code ...
            UpdateStatusLabelPeriodically(); // Start the periodic status label update
            scanBarcodeService = new ScanBarcodeService();
            scanBarcodeService.BarcodeScanned += ScanBarcodeService_BarcodeScanned;
            barcodeTimer = new BarcodeTimer(lbl_barcode);
            imageManager = new ImagesManagerService(pictureBox1);
            imageManager.ImageSlideshow();
            fontManager = new FontManagerService();
            lbl_barcode.Font = fontManager.GetCustomFont();
            videoManager = new VideoManagerService(axWindowsMediaPlayer1);
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Shift && e.KeyCode == Keys.Enter)
            {
                if (settingsForm == null || settingsForm.IsDisposed)
                {
                    settingsForm = new settingsForm();
                }

                settingsForm.Show();
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
                    serverStatusManager.UpdateStatusLabel(lbl_status, bottomPanel, lbl_appname);
                    serverStatusManager.Appname(lbl_appname);
                }
                catch (Exception ex)
                {
                    HandleError(ex.Message);
                    break; // Stop the loop if an error occurs
                }
                await Task.Delay(1000); //1sec
            }
        }
    }
}