using System.Drawing;
using System.Windows.Forms;

namespace Price_Checker
{
    partial class PriceCheckerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.pricePanel = new System.Windows.Forms.Panel();
            this.lbl_vendor = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_uom = new System.Windows.Forms.Label();
            this.lbl_generic = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_price = new System.Windows.Forms.Label();
            this.lbl_manufacturer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_barcode = new System.Windows.Forms.Label();
            this.lbl_name = new System.Windows.Forms.Label();
            this.name_barcode = new System.Windows.Forms.Label();
            this.name_name = new System.Windows.Forms.Label();
            this.lbl_details = new System.Windows.Forms.Label();
            this.pricePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pricePanel
            // 
            this.pricePanel.Controls.Add(this.lbl_vendor);
            this.pricePanel.Controls.Add(this.label4);
            this.pricePanel.Controls.Add(this.panel1);
            this.pricePanel.Controls.Add(this.lbl_generic);
            this.pricePanel.Controls.Add(this.label3);
            this.pricePanel.Controls.Add(this.panel3);
            this.pricePanel.Controls.Add(this.lbl_manufacturer);
            this.pricePanel.Controls.Add(this.label2);
            this.pricePanel.Controls.Add(this.lbl_barcode);
            this.pricePanel.Controls.Add(this.lbl_name);
            this.pricePanel.Controls.Add(this.name_barcode);
            this.pricePanel.Controls.Add(this.name_name);
            this.pricePanel.Controls.Add(this.lbl_details);
            this.pricePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pricePanel.Location = new System.Drawing.Point(0, 0);
            this.pricePanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pricePanel.Name = "pricePanel";
            this.pricePanel.Size = new System.Drawing.Size(1431, 367);
            this.pricePanel.TabIndex = 0;
            // 
            // lbl_vendor
            // 
            this.lbl_vendor.AutoSize = true;
            this.lbl_vendor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_vendor.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_vendor.ForeColor = System.Drawing.Color.Black;
            this.lbl_vendor.Location = new System.Drawing.Point(169, 261);
            this.lbl_vendor.Name = "lbl_vendor";
            this.lbl_vendor.Size = new System.Drawing.Size(28, 34);
            this.lbl_vendor.TabIndex = 31;
            this.lbl_vendor.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(27, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 34);
            this.label4.TabIndex = 30;
            this.label4.Text = "Vendor:";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(113)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.lbl_uom);
            this.panel1.Location = new System.Drawing.Point(816, 272);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(581, 79);
            this.panel1.TabIndex = 29;
            // 
            // lbl_uom
            // 
            this.lbl_uom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_uom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_uom.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_uom.ForeColor = System.Drawing.Color.White;
            this.lbl_uom.Location = new System.Drawing.Point(0, 0);
            this.lbl_uom.Name = "lbl_uom";
            this.lbl_uom.Size = new System.Drawing.Size(581, 79);
            this.lbl_uom.TabIndex = 23;
            this.lbl_uom.Text = "*";
            this.lbl_uom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_generic
            // 
            this.lbl_generic.AutoSize = true;
            this.lbl_generic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_generic.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_generic.ForeColor = System.Drawing.Color.Black;
            this.lbl_generic.Location = new System.Drawing.Point(188, 160);
            this.lbl_generic.Name = "lbl_generic";
            this.lbl_generic.Size = new System.Drawing.Size(28, 34);
            this.lbl_generic.TabIndex = 28;
            this.lbl_generic.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(27, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 34);
            this.label3.TabIndex = 27;
            this.label3.Text = "Generic:";
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(113)))), ((int)(((byte)(192)))));
            this.panel3.Controls.Add(this.lbl_price);
            this.panel3.Location = new System.Drawing.Point(816, 151);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(581, 122);
            this.panel3.TabIndex = 26;
            // 
            // lbl_price
            // 
            this.lbl_price.BackColor = System.Drawing.Color.Transparent;
            this.lbl_price.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_price.Font = new System.Drawing.Font("Arial Rounded MT Bold", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_price.ForeColor = System.Drawing.Color.White;
            this.lbl_price.Location = new System.Drawing.Point(0, 0);
            this.lbl_price.Name = "lbl_price";
            this.lbl_price.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_price.Size = new System.Drawing.Size(581, 122);
            this.lbl_price.TabIndex = 11;
            this.lbl_price.Text = "*";
            this.lbl_price.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_manufacturer
            // 
            this.lbl_manufacturer.AutoSize = true;
            this.lbl_manufacturer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_manufacturer.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_manufacturer.ForeColor = System.Drawing.Color.Black;
            this.lbl_manufacturer.Location = new System.Drawing.Point(264, 212);
            this.lbl_manufacturer.Name = "lbl_manufacturer";
            this.lbl_manufacturer.Size = new System.Drawing.Size(28, 34);
            this.lbl_manufacturer.TabIndex = 22;
            this.lbl_manufacturer.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(27, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 34);
            this.label2.TabIndex = 21;
            this.label2.Text = "Manufacturer:";
            // 
            // lbl_barcode
            // 
            this.lbl_barcode.AutoSize = true;
            this.lbl_barcode.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_barcode.ForeColor = System.Drawing.Color.Black;
            this.lbl_barcode.Location = new System.Drawing.Point(188, 310);
            this.lbl_barcode.Name = "lbl_barcode";
            this.lbl_barcode.Size = new System.Drawing.Size(28, 34);
            this.lbl_barcode.TabIndex = 19;
            this.lbl_barcode.Text = "*";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.ForeColor = System.Drawing.Color.Black;
            this.lbl_name.Location = new System.Drawing.Point(46, 84);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(38, 46);
            this.lbl_name.TabIndex = 20;
            this.lbl_name.Text = "*";
            // 
            // name_barcode
            // 
            this.name_barcode.AutoSize = true;
            this.name_barcode.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_barcode.ForeColor = System.Drawing.Color.Black;
            this.name_barcode.Location = new System.Drawing.Point(27, 310);
            this.name_barcode.Name = "name_barcode";
            this.name_barcode.Size = new System.Drawing.Size(150, 34);
            this.name_barcode.TabIndex = 17;
            this.name_barcode.Text = "Barcode:";
            // 
            // name_name
            // 
            this.name_name.AutoSize = true;
            this.name_name.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name_name.ForeColor = System.Drawing.Color.Black;
            this.name_name.Location = new System.Drawing.Point(33, 22);
            this.name_name.Name = "name_name";
            this.name_name.Size = new System.Drawing.Size(337, 50);
            this.name_name.TabIndex = 7;
            this.name_name.Text = "Product Name:";
            // 
            // lbl_details
            // 
            this.lbl_details.AutoSize = true;
            this.lbl_details.Font = new System.Drawing.Font("Arial Rounded MT Bold", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_details.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(113)))), ((int)(((byte)(192)))));
            this.lbl_details.Location = new System.Drawing.Point(480, 7);
            this.lbl_details.Name = "lbl_details";
            this.lbl_details.Size = new System.Drawing.Size(506, 58);
            this.lbl_details.TabIndex = 6;
            this.lbl_details.Text = "PRODUCT DETAILS";
            this.lbl_details.Visible = false;
            // 
            // PriceCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1431, 367);
            this.Controls.Add(this.pricePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PriceCheckerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "s";
            this.pricePanel.ResumeLayout(false);
            this.pricePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private Panel pricePanel;
        private Label lbl_barcode;
        private Label name_barcode;
        private Label name_name;
        private Label lbl_details;
        private Label lbl_manufacturer;
        private Label label2;
        private Panel panel3;
        private Label lbl_name;
        private Label lbl_generic;
        private Label label3;
        private Label lbl_uom;
        private Label lbl_price;
        private Panel panel1;
        private Label lbl_vendor;
        private Label label4;
    }
}

