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
            productDetailService.HandleProductDetails(barcode, lbl_name, lbl_price, lbl_manufacturer, lbl_uom, lbl_generic, lbl_vendor);

        }

        public void SetBarcode(string barcode)
        {
            lbl_barcode.Text = barcode;
        }
    }
}