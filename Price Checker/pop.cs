using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Price_Checker
{
    public partial class pop : Form
    {
        private List<Product> products;
        public Product SelectedProduct { get; private set; }

        public pop(List<Product> products)
        {
            InitializeComponent();
            this.products = products;
            LoadProducts();
            listBox1.KeyDown += listBox1_KeyDown;
            listBox1.Click += listBox1_Click;
        }

        private void LoadProducts()
        {
            if (products.Count > 1)
            {
                listBox1.DataSource = products;
                listBox1.DisplayMember = "Name";
            }
            else
            {
                MessageBox.Show("Product not found.");
                Close();
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            SelectProduct();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectProduct();
            }
        }

        private void SelectProduct()
        {
            if (listBox1.SelectedItem != null)
            {
                SelectedProduct = (Product)listBox1.SelectedItem;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please select a product.");
            }
        }
    }
}
