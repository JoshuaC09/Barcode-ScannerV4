using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Price_Checker.Configuration
{
    internal class ProductDetailService
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly Timer _timer;
        private readonly Form _formInstance;

        public ProductDetailService(Form form)
        {
            _dbHelper = new DatabaseHelper(ConnectionStringService.ConnectionString);
            _formInstance = form;
            _timer = new Timer();
            _timer.Tick += Timer_Tick;
            SetTimerInterval();
            _timer.Start();
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
                    var result = chooseProductForm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        SetLabelValues(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, chooseProductForm.SelectedProduct);
                    }
                    else
                    {
                        SetLabelValuesToNA(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic);
                    }
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

        private void SetLabelValuesToNA(Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic)
        {
            lbl_name.Text = "N/A";
            lbl_price.Text = "N/A";
            lbl_manufacturer.Text = "N/A";
            lbl_uom.Text = "N/A";
            lbl_generic.Text = "N/A";
        }

        private void SetTimerInterval()
        {
            const string sql = "SELECT set_disptime FROM settings";
            var result = _dbHelper.ExecuteScalar(sql);

            if (result != null && int.TryParse(result.ToString(), out int interval))
            {
                _timer.Interval = interval * 1000; // Convert to milliseconds
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _formInstance.Close();
            _timer.Stop(); // Stop the timer once form is closed
        }

        public List<Product> GetProductDetails(string barcode)
        {
            var products = new List<Product>();
            const string sql = "SELECT prod_description, prod_price, prod_pincipal, prod_uom, prod_generic FROM prod_verifier WHERE prod_barcode = @barcode";
            var parameters = new Dictionary<string, object> { { "@barcode", barcode } };
            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);

            foreach (DataRow row in dataTable.Rows)
            {
                var product = new Product
                {
                    Name = row["prod_description"].ToString(),
                    Price = "₱ " + Convert.ToDecimal(row["prod_price"]).ToString("N2"),
                    Manufacturer = "Manufacturer: " + row["prod_pincipal"].ToString(),
                    UOM = "per " + row["prod_uom"],
                    Generic = "Generic: " + row["prod_generic"].ToString()
                };
                products.Add(product);
            }

            return products;
        }
    }
}
