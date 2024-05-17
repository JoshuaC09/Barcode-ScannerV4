using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

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
                        DisplayPriceForm(barcode, detailPanel);
                        OnBarcodeScanned(barcode);

                        // Refocus the barcodeLabel
                        barcodeLabel.Focus();

                        // Update the display price form
                        UpdateDisplayPriceForm(barcode, detailPanel);
                    }
                    else
                    {
                        ShowMessageBoxAndDisappear("Product not found", 3000, mainForm, barcodeLabel);

                        // Refocus the barcodeLabel
                        barcodeLabel.Focus();
                    }
                }
            }
        }
        private void UpdateDisplayPriceForm(string barcode, Control detailPanel)
        {
            foreach (Control control in detailPanel.Controls)
            {
                if (control is PriceCheckerForm priceForm)
                {
                    priceForm.SetBarcode(barcode);
                    break;
                }
            }
        }


        protected virtual void OnBarcodeScanned(string barcode)
        {
            BarcodeScanned?.Invoke(this, barcode);
        }

        private void DisplayPriceForm(string barcode, Panel detailPanel)
        {
            var priceForm = new PriceCheckerForm(barcode)
            {
                Dock = DockStyle.Fill,
                Size = detailPanel.Size,
                FormBorderStyle = FormBorderStyle.None,
                TopLevel = false
            };
            detailPanel.Controls.Add(priceForm);
            priceForm.BringToFront();
            priceForm.TabIndex = 0;
            priceForm.SetBarcode(barcode);
            priceForm.Show();
        }

        private void ShowMessageBoxAndDisappear(string message, int milliseconds, mainForm mainForm, TextBox barcodeLabel)
        {
            using (var messageBox = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(22, 113, 192),
                Size = new Size(200, 100)
            })
            {
                var label = new Label
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
        }

        private bool IsBarcodeInDatabase(string barcode)
        {
            const string query = "SELECT prod_itemcode FROM prod_verifier WHERE prod_barcode = @barcode";
            using (var conn = new MySqlConnection(ConnectionStringService.ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }
    }
}
