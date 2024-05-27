using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.Common;

namespace Price_Checker.Configuration
{
    internal class ProductDetailService
    {
        private readonly string connstring;
        private readonly DatabaseHelper _dbHelper;
        private readonly Timer _timer;
        private readonly Form _formInstance;

        public ProductDetailService(Form form)
        {
            connstring = ConnectionStringService.ConnectionString;
            _dbHelper = new DatabaseHelper(connstring);
            _formInstance = form;
            _timer = new Timer();
            _timer.Tick += Timer_Tick;
            SetTimerInterval();
            _timer.Start();
        }

        // Assuming this is part of ProductDetailService class
        public void HandleProductDetails(string barcode, Label lbl_name, Label lbl_price, Label lbl_manufacturer, Label lbl_uom, Label lbl_generic, Panel detailPanel)
        {
            var products = GetProductDetails(barcode);

            if (products.Count == 1)
            {
                SetLabelValues(lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, products[0]);
            }
            else if (products.Count > 1)
            {
                var displaypopform = new pop(products);
                displaypopform.TopLevel = false;
                displaypopform.FormBorderStyle = FormBorderStyle.None;
                displaypopform.Dock = DockStyle.Fill;
                displaypopform.Size = detailPanel.Size;
                detailPanel.Controls.Add(displaypopform);
                detailPanel.Tag = displaypopform;
                displaypopform.BringToFront();
                displaypopform.Show();

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
            const string sql = "SELECT set_disptime FROM settings";
            var result = _dbHelper.ExecuteScalar(sql);

            if (result != null && int.TryParse(result.ToString(), out int interval))
            {
                _timer.Interval = interval * 1000; // Convert to milliseconds
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_formInstance != null)
            {
                _formInstance.Close();
            }

            _timer.Stop(); // Stop the timer once form is closed
        }

        public List<Product> GetProductDetails(string barcode)
        {
            var products = new List<Product>();
            const string sql = "SELECT prod_description, prod_price, prod_pincipal, prod_uom, prod_generic FROM prod_verifier WHERE prod_barcode = @barcode";
            var parameters = new Dictionary<string, object>
    {
        { "@barcode", barcode }
    };

            using (var dataReader = _dbHelper.ExecuteReader(sql, parameters))
            {
                foreach (DbDataRecord record in dataReader)
                {
                    var product = new Product
                    {
                        Name = record["prod_description"].ToString(),
                        Price = "₱ " + Convert.ToDecimal(record["prod_price"]).ToString("N2"),
                        Manufacturer = record["prod_pincipal"].ToString(),
                        UOM = "per " + record["prod_uom"],
                        Generic = record["prod_generic"].ToString()
                    };
                    products.Add(product);
                }
            }
            return products;
        }
    }
}