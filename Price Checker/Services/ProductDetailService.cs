using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Price_Checker.Configuration
{
    internal class ProductDetailService
    {
        private readonly string connstring;
      

        //
        private Timer timer = new Timer();
        private int intervalInSeconds = 0;


        private Form formInstance;

        public ProductDetailService(Form form)
        {    
            connstring = ConnectionStringService.ConnectionString;
            formInstance = form;

            timer.Tick += Timer_Tick;
            SetTimerInterval();
            timer.Start();
        }
        public void HandleProductDetails(string barcode, Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Label lbl_vendor)
        {
            List<Product> products = GetProductDetails(barcode);

            if (products.Count == 1)
            {
                lbl_name.Text = products[0].Name;
                lbl_price.Text = products[0].Price;
                lbl_manufacturer.Text = products[0].Manufacturer;
                lbl_uom.Text = products[0].UOM;
                lbl_generic.Text = products[0].Generic;
                lbl_vendor.Text = products[0].Vendor;
            }
            else if (products.Count > 1)
            {
                // Prompt user to choose the product
                using (pop chooseProductForm = new pop(products))
                {
                    DialogResult result = chooseProductForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        lbl_name.Text = chooseProductForm.SelectedProduct.Name;
                        lbl_price.Text = chooseProductForm.SelectedProduct.Price;
                        lbl_manufacturer.Text = chooseProductForm.SelectedProduct.Manufacturer;
                        lbl_uom.Text = chooseProductForm.SelectedProduct.UOM;
                        lbl_generic.Text = chooseProductForm.SelectedProduct.Generic;
                        lbl_vendor.Text = chooseProductForm.SelectedProduct.Vendor;
                    }
                    else
                    {
                        lbl_name.Text = "N/A";
                        lbl_price.Text = "N/A";
                        lbl_manufacturer.Text = "N/A";
                        lbl_uom.Text = "N/A";
                        lbl_generic.Text = "N/A";
                        lbl_vendor.Text = "N/A";

                    }
                }
            }
            else
            {
                MessageBox.Show("Product Not Found");
            }

            //// Timer logic
            //Timer timer = new Timer();
            //timer.Interval = 5000; // Form interval to 3 seconds
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void SetTimerInterval()
        {
            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                con.Open();
                string sql = "SELECT set_disptime FROM settings";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        intervalInSeconds = reader.GetInt32(0);
                        timer.Interval = intervalInSeconds * 1000; // Multiply by 1000 to convert to milliseconds
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            formInstance.Close();
            timer.Stop(); // Stop the timer once form is closed
        }

        public List<Product> GetProductDetails(string barcode)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    string sql = $"SELECT * FROM prod_verifier WHERE prod_barcode = '{barcode}'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.Name = reader["prod_description"].ToString();
                        product.Price = "₱ " + reader["prod_price"].ToString();
                        product.Manufacturer = reader["prod_pincipal"].ToString();
                        product.UOM = "per " + reader["prod_uom"].ToString();
                        product.Generic = reader["prod_generic"].ToString();
                        product.Vendor = reader["prod_vendor"].ToString();
                        products.Add(product);
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