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

        public pop(List<Product> products)
        {
            InitializeComponent();
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
            autoCloseTimer.Interval = 10000; // Set timer interval to 5 seconds (5000 milliseconds)
            autoCloseTimer.Tick += AutoCloseTimer_Tick;
            autoCloseTimer.Start();
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




//    public partial class pop : Form
//    {
//        private List<Product> products;
//        public Product SelectedProduct { get; private set; }

//        public pop(List<Product> products)
//        {
//            InitializeComponent();
//            this.products = products;
//            LoadProducts();
//            listBox1.KeyDown += listBox1_KeyDown;
//            listBox1.Click += listBox1_Click;
//        }

//        private void LoadProducts()
//        {
//            if (products.Count > 1)
//            {
//                listBox1.DataSource = products;
//                listBox1.DisplayMember = "Name";
//            }
//            else
//            {
//                MessageBox.Show("Product not found.");
//                Close();
//            }
//        }
//        private void listBox1_Click(object sender, EventArgs e)
//        {
//            SelectProduct();
//        }

//        private void listBox1_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                SelectProduct();
//            }
//        }

//        private void SelectProduct()
//        {
//            if (listBox1.SelectedItem != null)
//            {
//                SelectedProduct = (Product)listBox1.SelectedItem;
//                DialogResult = DialogResult.OK;
//            }
//            else
//            {
//                MessageBox.Show("Please select a product.");
//            }
//        }
//    }
//}
