using System;
using System.Windows.Forms;

namespace Price_Checker.Configuration
{
    public class BarcodeTimer
    {
        private readonly Timer timer;
        private readonly TextBox barcodeLabel;

        public BarcodeTimer(TextBox barcodeLabel)
        {
            this.barcodeLabel = barcodeLabel;
            timer = new Timer { Interval = 100 };
            timer = new Timer();
            timer.Interval = 100;

            timer.Tick += Timer_Tick;
        }

        public void StartTimer() => timer.Start();

        private void Timer_Tick(object sender, EventArgs e)
        {
            barcodeLabel.Text = string.Empty;
            timer.Stop();
        }


    }
}
