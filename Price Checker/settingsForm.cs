using Price_Checker.Configuration;
using Price_Checker.SettingsHelpers;
using System;
using System.Windows.Forms;

namespace Price_Checker
{
    public partial class settingsForm : Form
    {
        private readonly SettingsHelper _settingsManager;
        private string connString = ConnectionStringService.ConnectionString;

        public settingsForm()
        {
            InitializeComponent();
            _settingsManager = new SettingsHelper(connString);
            _settingsManager.LoadSettings(tb_appname, tb_adpictime, tb_adpicpath, tb_advidtime, tb_advidpath, tb_disptime, rb_ipos, rb_eipos);
            btn_close.Click += btn_close_Click;

            rb_ipos.CheckedChanged += RadioButton_CheckedChanged;
            rb_eipos.CheckedChanged += RadioButton_CheckedChanged;

            // Disable text boxes initially
            SetTextBoxesEnabled(false);

            btnEdit.Click += btnEdit_Click;
        }

        private void btnBrowseImages_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select a folder containing images";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tb_adpicpath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnBrowseVideos_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select a folder containing videos";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tb_advidpath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            _settingsManager.SaveSettings(tb_appname, tb_adpictime, tb_adpicpath, tb_advidtime, tb_advidpath, tb_disptime);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _settingsManager.UpdateRadioButton(rb_ipos, rb_eipos);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Enable text boxes when Edit button is clicked
            SetTextBoxesEnabled(true);
        }

        private void SetTextBoxesEnabled(bool enabled)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox || control is RadioButton || control is Button)
                {
                    if (control != btnEdit)
                    {
                        control.Enabled = enabled;
                    }

                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}