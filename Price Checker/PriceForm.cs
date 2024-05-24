using System.Collections.Generic;
using System.Drawing;
using System;
using System.Windows.Forms;
using Price_Checker.Configuration;

namespace Price_Checker
{
    public partial class PriceCheckerForm : Form
    {
        private readonly ProductDetailService productDetailService;

        public PriceCheckerForm(string barcode)
        {
            InitializeComponent();

            productDetailService = new ProductDetailService(this);
            productDetailService.HandleProductDetails(barcode, lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic);


            this.Load += Form_Load;
            this.Resize += Form_Resize;


        }

        public void SetBarcode(string barcode)
        {
            lbl_barcode.Text = barcode;
        }

        private readonly float originalFormWidth = 1431f;
        private readonly float originalFormHeight = 367f;

        private Dictionary<Control, float> originalFontSizes = new Dictionary<Control, float>();

        private void Form_Load(object sender, EventArgs e)
        {
            StoreOriginalSizesAndPositions(pricePanel);

            if (!originalFontSizes.ContainsKey(lbl_name))
            {
                originalFontSizes[lbl_name] = lbl_name.Font.Size;
            }

            AdjustFontSizes();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            AdjustFontSizes();
        }

        private void StoreOriginalSizesAndPositions(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    if (!originalFontSizes.ContainsKey(label))
                    {
                        originalFontSizes[label] = label.Font.Size;
                    }
                }
            }
        }

        private void AdjustFontSizes()
        {
            float currentFormWidth = this.ClientSize.Width;
            float currentFormHeight = this.ClientSize.Height;

            float widthRatio = currentFormWidth / originalFormWidth;
            float heightRatio = currentFormHeight / originalFormHeight;

            float formRatio = Math.Max(widthRatio, heightRatio);

            AdjustPanelFontSizes(pricePanel, widthRatio, heightRatio, formRatio);

            if (originalFontSizes.ContainsKey(lbl_name))
            {
                float originalFontSizeAppName = originalFontSizes[lbl_name];
                lbl_name.Font = new Font(lbl_name.Font.FontFamily, originalFontSizeAppName * formRatio);
            }
        }

        private void AdjustPanelFontSizes(Panel panel, float widthRatio, float heightRatio, float formRatio)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    if (originalFontSizes.ContainsKey(label))
                    {
                        float originalFontSize = originalFontSizes[label];
                        label.Font = new Font(label.Font.FontFamily, originalFontSize * formRatio);
                    }
                }
            }
        }

    }
}