using System;
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

        public mainForm()
        {
            InitializeComponent();
            lbl_barcode.KeyDown += Lbl_barcode_KeyDown;
            KeyPreview = true;
            this.Shown += MainForm_Shown;

            timer1.Start();
            timer1.Interval = 1000; // 1000 milliseconds = 1 second
            timer1.Tick += timer1_Tick;

            serverStatusManager = new ServerStatusService();
            serverStatusManager.UpdateStatusLabel(lbl_status, bottomPanel); // Call the UpdateStatusLabel method
            serverStatusManager.Appname(lbl_appname); // Call the Appname method

            scanBarcodeService = new ScanBarcodeService();
            scanBarcodeService.BarcodeScanned += ScanBarcodeService_BarcodeScanned;
            barcodeTimer = new BarcodeTimer(lbl_barcode);
            imageManager = new ImagesManagerService(pictureBox1);
            imageManager.ImageSlideshow();

            fontManager = new FontManagerService();
            lbl_barcode.Font = fontManager.GetCustomFont();

            videoManager = new VideoManagerService(axWindowsMediaPlayer1);

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            serverStatusManager.UpdateStatusLabel(lbl_status, bottomPanel);
        }


    }
}
