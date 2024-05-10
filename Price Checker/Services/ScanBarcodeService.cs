using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Price_Checker.Configuration
{
    public class ScanBarcodeService

    {

        private DatabaseConfig _config;
        public event EventHandler<string> BarcodeScanned;


        public void HandleBarcodeInput(KeyEventArgs e, TextBox barcodeLabel, Panel scanPanel, mainForm mainForm)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = barcodeLabel.Text.Trim();
                if (!string.IsNullOrEmpty(barcode))
                {
                    if (IsBarcodeInDatabase(barcode))
                    {
                        // Create an instance of PriceCheckerForm and pass the barcode
                        PriceCheckerForm priceForm = new PriceCheckerForm(barcode);
                        // Call a method to prepare PriceCheckerForm for display
                        priceForm.Show();
                        // Set the location of PriceCheckerForm to match that the scanPanel
                        // Set the size of priceForm to match
                        //priceForm.Size = scanPanel.Size;

                        int offsetX = 230; // Adjust this value as needed
                        int offsetY = 115;

                        // Manually adjust the location of priceForm
                        // For example, you can set it to a specific position relative to scanPanel
                        priceForm.Location = new Point(scanPanel.Location.X + offsetX, scanPanel.Location.Y + offsetY);

                        // Display the barcode in tb_barcode in PriceCheckerForm
                        priceForm.SetBarcode(barcode);
                        OnBarcodeScanned(barcode);
                    }
                    else
                    {
                        // Show a message box that automatically disappears after a few seconds
                        ShowMessageBoxAndDisappear("Product not found", 3000, mainForm, barcodeLabel);

                    }
                }

            }
        }

        protected virtual void OnBarcodeScanned(string barcode)
        {
            BarcodeScanned?.Invoke(this, barcode);
        }
        private void ShowMessageBoxAndDisappear(string message, int milliseconds, mainForm mainForm, TextBox barcodeLabel)
        {
            var messageBox = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(22, 113, 192),
                Size = new Size(200, 100)
            };

            var label = new Label()
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(messageBox.Font.FontFamily, 15),
                ForeColor = Color.White,
                Padding = new Padding(10)
            };

            messageBox.Controls.Add(label);
            messageBox.TopMost = true;

            var timer = new System.Threading.Timer(
                e =>
                {
                    if (messageBox.InvokeRequired)
                    {
                        messageBox.BeginInvoke(new MethodInvoker(() =>
                        {
                            messageBox.Close();
                            barcodeLabel.Clear();
                            mainForm.Activate();
                        }));
                    }
                    else
                    {
                        messageBox.Close();
                        barcodeLabel.Clear();
                        mainForm.Activate();
                    }
                },
                null,
                milliseconds,
                Timeout.Infinite
            );

            messageBox.ShowDialog(mainForm);
        }

        private bool IsBarcodeInDatabase(string barcode)
        {
            bool barcodeExists = false;

            _config = new DatabaseConfig();
            string connString = ConnectionStringService.ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string sql = $"SELECT prod_itemcode FROM prod_verifier WHERE prod_barcode = '{barcode}'";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            barcodeExists = reader.HasRows;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle the exception appropriately
                Console.WriteLine("Error: " + ex.Message);
            }

            return barcodeExists;
        }

    }
}