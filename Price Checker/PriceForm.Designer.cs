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
            this.pricePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pricePanel
            // 
            this.pricePanel.Controls.Add(this.panel1);
            this.pricePanel.Controls.Add(this.lbl_generic);
            this.pricePanel.Controls.Add(this.label3);
            this.pricePanel.Controls.Add(this.panel3);
            this.pricePanel.Controls.Add(this.lbl_manufacturer);
            this.pricePanel.Controls.Add(this.label2);
            this.pricePanel.Controls.Add(this.lbl_barcode);
            this.pricePanel.Controls.Add(this.lbl_name);
            this.pricePanel.Controls.Add(this.name_barcode);
            this.pricePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pricePanel.Location = new System.Drawing.Point(0, 0);
            this.pricePanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pricePanel.Name = "pricePanel";
            this.pricePanel.Size = new System.Drawing.Size(1431, 367);
            this.pricePanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(113)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.lbl_uom);
            this.panel1.Location = new System.Drawing.Point(752, 272);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 79);
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
            this.lbl_uom.Size = new System.Drawing.Size(645, 79);
            this.lbl_uom.TabIndex = 23;
            this.lbl_uom.Text = "*";
            this.lbl_uom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_generic
            // 
            this.lbl_generic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_generic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_generic.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_generic.ForeColor = System.Drawing.Color.Black;
            this.lbl_generic.Location = new System.Drawing.Point(182, 202);
            this.lbl_generic.Name = "lbl_generic";
            this.lbl_generic.Size = new System.Drawing.Size(550, 34);
            this.lbl_generic.TabIndex = 28;
            this.lbl_generic.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(30, 202);
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
            this.panel3.Location = new System.Drawing.Point(752, 151);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(645, 122);
            this.panel3.TabIndex = 26;
            // 
            // lbl_price
            // 
            this.lbl_price.BackColor = System.Drawing.Color.Transparent;
            this.lbl_price.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_price.Font = new System.Drawing.Font("Arial Rounded MT Bold", 49.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_price.ForeColor = System.Drawing.Color.White;
            this.lbl_price.Location = new System.Drawing.Point(0, 0);
            this.lbl_price.Name = "lbl_price";
            this.lbl_price.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_price.Size = new System.Drawing.Size(645, 122);
            this.lbl_price.TabIndex = 11;
            this.lbl_price.Text = "*";
            this.lbl_price.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_manufacturer
            // 
            this.lbl_manufacturer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_manufacturer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_manufacturer.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_manufacturer.ForeColor = System.Drawing.Color.Black;
            this.lbl_manufacturer.Location = new System.Drawing.Point(255, 254);
            this.lbl_manufacturer.Name = "lbl_manufacturer";
            this.lbl_manufacturer.Size = new System.Drawing.Size(477, 34);
            this.lbl_manufacturer.TabIndex = 22;
            this.lbl_manufacturer.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(27, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 34);
            this.label2.TabIndex = 21;
            this.label2.Text = "Manufacturer:";
            // 
            // lbl_barcode
            // 
            this.lbl_barcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_barcode.Font = new System.Drawing.Font("Arial Rounded MT Bold", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_barcode.ForeColor = System.Drawing.Color.Black;
            this.lbl_barcode.Location = new System.Drawing.Point(188, 310);
            this.lbl_barcode.Name = "lbl_barcode";
            this.lbl_barcode.Size = new System.Drawing.Size(607, 34);
            this.lbl_barcode.TabIndex = 19;
            this.lbl_barcode.Text = "*";
            // 
            // lbl_name
            // 
            this.lbl_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_name.Font = new System.Drawing.Font("Arial Rounded MT Bold", 49.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.ForeColor = System.Drawing.Color.Black;
            this.lbl_name.Location = new System.Drawing.Point(38, 23);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(1359, 116);
            this.lbl_name.TabIndex = 20;
            this.lbl_name.Text = "*";
            this.lbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.Text = "Price Checker Form";
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
        private Label lbl_manufacturer;
        private Label label2;
        private Panel panel3;
        private Label lbl_name;
        private Label lbl_generic;
        private Label label3;
        private Label lbl_uom;
        private Label lbl_price;
        private Panel panel1;
    }
}

