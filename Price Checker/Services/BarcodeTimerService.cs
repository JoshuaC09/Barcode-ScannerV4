using System;
using System.Windows.Forms;

namespace Price_Checker.Configuration
{
    public class BarcodeTimer
    {
        private Timer timer;
        private TextBox barcodeLabel;

        public BarcodeTimer(TextBox barcodeLabel)
        {
            this.barcodeLabel = barcodeLabel;
            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;
        }

        public void StartTimer()
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            barcodeLabel.Text = "";
            timer.Stop();
        }
    }
}