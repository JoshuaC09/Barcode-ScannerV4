using Price_Checker.Configuration;
using Price_Checker.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Price_Checker
{
    public partial class pop : Form
    {
        private List<Product> products;
        private Timer autoCloseTimer;
        private readonly string connstring;
        private readonly DatabaseHelper _dbHelper;

        public pop(List<Product> products)
        {
            InitializeComponent();
            connstring = ConnectionStringService.ConnectionString;
            _dbHelper = new DatabaseHelper(connstring);

            this.products = products;
            LoadProducts();
            StartAutoCloseTimer();
                       
        }

        private void LoadProducts()
        {
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    dataGridViewpop.Rows.Add(product.Name, product.Price, product.UOM, product.Manufacturer);
                }
            }
            else
            {
                MessageBox.Show("No products found.");
                Close();
            }
        }

        private void StartAutoCloseTimer()
        {
            autoCloseTimer = new Timer();
            autoCloseTimer.Interval = SetTimerInterval(); // Set timer interval to 5 seconds (5000 milliseconds)
            autoCloseTimer.Tick += AutoCloseTimer_Tick;
            autoCloseTimer.Start();
        }


        private int SetTimerInterval()
        {
            const string sql = "SELECT set_muldisptime FROM settings";
            var result = _dbHelper.ExecuteScalar(sql);

            if (result != null)
            {
                return (int)result * 1000;
            }
            else
            {
                return 6 * 1000;
            }
        }



        private void AutoCloseTimer_Tick(object sender, EventArgs e)
        {
            autoCloseTimer.Stop();
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            autoCloseTimer?.Stop();
            autoCloseTimer?.Dispose();
        }
    }
}