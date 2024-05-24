using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
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

        public void HandleProductDetails(string barcode, Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic)
        {
            var products = GetProductDetails(barcode);

            if (products.Count == 1)
            {
                SetLabelValues(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, products[0]);
            }
            else if (products.Count > 1)
            {
                using (var chooseProductForm = new pop(products))
                {
                    chooseProductForm.ShowDialog();
                }

            }
            else
            {
                MessageBox.Show("Product Not Found");
            }
        }

        private void SetLabelValues(Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Product product)
        {
            lbl_name.Text = product.Name;
            lbl_price.Text = product.Price;
            lbl_manufacturer.Text = product.Manufacturer;
            lbl_uom.Text = product.UOM;
            lbl_generic.Text = product.Generic;
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
            if (formInstance != null)
            {
                formInstance.Close();
            }
            timer.Stop(); // Stop the timer once form is closed
        }

        public List<Product> GetProductDetails(string barcode)
        {
            var products = new List<Product>();
            try
            {
                using (var con = new MySqlConnection(connstring))
                {
                    con.Open();
                    const string sql = "SELECT prod_description, prod_price, prod_pincipal, prod_uom, prod_generic FROM prod_verifier WHERE prod_barcode = @barcode";
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@barcode", barcode);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new Product
                                {
                                    Name = reader["prod_description"].ToString(),
                                    Price = "₱ " + Convert.ToDecimal(reader["prod_price"]).ToString("N2"),
                                    Manufacturer = reader["prod_pincipal"].ToString(),
                                    UOM = "per " + reader["prod_uom"],
                                    Generic = reader["prod_generic"].ToString()
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
