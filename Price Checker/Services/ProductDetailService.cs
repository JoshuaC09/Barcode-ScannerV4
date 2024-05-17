using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Price_Checker.Configuration
{
    internal class ProductDetailService 
    {
        private readonly string connstring;
        private readonly Timer timer;
        private readonly Form formInstance;

        public ProductDetailService(Form form)
        {
            connstring = ConnectionStringService.ConnectionString;
            formInstance = form;
            timer = new Timer();
            timer.Tick += Timer_Tick;
            SetTimerInterval();
            timer.Start();
        }

        public void HandleProductDetails(string barcode, Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Label lbl_vendor)
        {
            var products = GetProductDetails(barcode);

            if (products.Count == 1)
            {
                SetLabelValues(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, lbl_vendor, products[0]);
            }
            else if (products.Count > 1)
            {
                using (var chooseProductForm = new pop(products))
                {
                    var result = chooseProductForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        SetLabelValues(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, lbl_vendor, chooseProductForm.SelectedProduct);
                    }
                    else
                    {
                        SetLabelValuesToNA(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, lbl_vendor);
                    }
                }
            }
            else
            {
                MessageBox.Show("Product Not Found");
            }
        }

        private void SetLabelValues(Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Label lbl_vendor, Product product)
        {
            lbl_name.Text = product.Name;
            lbl_price.Text = product.Price;
            lbl_manufacturer.Text = product.Manufacturer;
            lbl_uom.Text = product.UOM;
            lbl_generic.Text = product.Generic;
            lbl_vendor.Text = product.Vendor;
        }

        private void SetLabelValuesToNA(Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Label lbl_vendor)
        {
            lbl_name.Text = "N/A";
            lbl_price.Text = "N/A";
            lbl_manufacturer.Text = "N/A";
            lbl_uom.Text = "N/A";
            lbl_generic.Text = "N/A";
            lbl_vendor.Text = "N/A";
        }

        private void SetTimerInterval()
        {
            using (var con = new MySqlConnection(connstring))
            {
                con.Open();
                const string sql = "SELECT set_disptime FROM settings";
                using (var cmd = new MySqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        timer.Interval = reader.GetInt32(0) * 1000; // Convert to milliseconds
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            formInstance.Close();

            timer.Stop();

            timer.Stop(); // Stop the timer once form sis closed

        }

        public List<Product> GetProductDetails(string barcode)
        {
            var products = new List<Product>();
            try
            {
                using (var con = new MySqlConnection(connstring))
                {
                    con.Open();

                    const string sql = "SELECT * FROM prod_verifier WHERE prod_barcode = @barcode";
                    using (var cmd = new MySqlCommand(sql, con))

                    string sql = $"SELECT prod_description, CAST(prod_price AS DECIMAL(6,2)) as prod_price, prod_pincipal, prod_uom, prod_generic, prod_vendor  FROM prod_verifier WHERE prod_barcode = '{barcode}'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())

                    {
                        cmd.Parameters.AddWithValue("@barcode", barcode);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Product
                                {
                                    Name = reader["prod_description"].ToString(),
                                    Price = "₱ " + reader["prod_price"],
                                    Manufacturer = reader["prod_pincipal"].ToString(),
                                    UOM = "per " + reader["prod_uom"],
                                    Generic = reader["prod_generic"].ToString(),
                                    Vendor = reader["prod_vendor"].ToString()
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex);
            }
            return products;
        }
    }
}
