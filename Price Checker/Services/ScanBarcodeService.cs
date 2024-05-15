using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Price_Checker.Services
{
    public class ScanBarcodeService
        
    {
        public event EventHandler<string> BarcodeScanned;


        public void HandleBarcodeInput(KeyEventArgs e, TextBox barcodeLabel, Panel detailPanel, mainForm mainForm)
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
                        priceForm.Dock = DockStyle.Fill; // Fill the panel
                        priceForm.Size = detailPanel.Size;

                        priceForm.FormBorderStyle = FormBorderStyle.None; // Remove border
                        priceForm.TopLevel = false; // Set as non-top-level form

                        detailPanel.Controls.Add(priceForm); // Add PriceForm to a panel on the main form
                                                             // Display the barcode in tb_barcode in PriceCheckerForm

                        // Move the priceForm to the front
                        priceForm.BringToFront();

                        // Set TabIndex to ensure focus ordering
                        priceForm.TabIndex = 0;

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

            string connString = ConnectionStringService.ConnectionString; 

            // SQL query (modified)
            string sql = $"SELECT prod_itemcode FROM prod_verifier WHERE prod_barcode = '{barcode}'";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            barcodeExists = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                Console.WriteLine("Error: " + ex.Message);
            }

            return barcodeExists;
        }

    }
}