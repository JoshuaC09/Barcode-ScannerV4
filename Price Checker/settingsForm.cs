using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
using Price_Checker.Services;


namespace Price_Checker
{
    public partial class settingsForm : Form
    {
        private DatabaseConfig _config;

        public settingsForm()
        {
            InitializeComponent();
            LoadSettings(tb_appname, tb_adpictime, tb_adpicpath, tb_advidtime, tb_advidpath, tb_disptime, rb_ipos, rb_eipos);
            btn_clear.Click += btn_clear_Click;

            this.KeyDown += SettingsForm_KeyDown;

            rb_ipos.CheckedChanged += RadioButton_CheckedChanged;
            rb_eipos.CheckedChanged += RadioButton_CheckedChanged;

            // Disable text boxes initially
            SetTextBoxesEnabled(false);

            btnEdit.Click += btnEdit_Click;

        }
        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Alt + Shift + Enter
            if (e.Alt && e.Shift && e.KeyCode == Keys.Enter)
            {
                settingsForm newSettingsForm = new settingsForm();
                newSettingsForm.Show();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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
            _config = new DatabaseConfig();
            string connString = $"server={_config.Server};port={_config.Port};uid={_config.Uid};pwd={_config.Pwd};database={_config.Database}";

            // Check if any required fields are empty
            if (string.IsNullOrEmpty(tb_appname.Text) || string.IsNullOrEmpty(tb_adpictime.Text) || string.IsNullOrEmpty(tb_advidtime.Text) || string.IsNullOrEmpty(tb_disptime.Text))
            {
                MessageBox.Show("Please enter values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if any required fields are empty
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "UPDATE settings SET set_appname = @appName, set_adpictime = @adPicTime, set_adpic = @adPicPath, set_advidtime = @adVidTime, set_advid = @adVidPath, set_disptime = @dispTime";

                MySqlCommand command = new MySqlCommand(query, conn);

                command.Parameters.AddWithValue("@appName", tb_appname.Text);
                command.Parameters.AddWithValue("@adPicTime", tb_adpictime.Text);
                command.Parameters.AddWithValue("@adPicPath", tb_adpicpath.Text.Replace("\\", "$"));
                command.Parameters.AddWithValue("@adVidTime", tb_advidtime.Text);
                command.Parameters.AddWithValue("@adVidPath", tb_advidpath.Text.Replace("\\", "$"));
                command.Parameters.AddWithValue("@dispTime", tb_disptime.Text);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Settings successfully saved.");
            }
        }

        private void LoadSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime, RadioButton rb_ipos, RadioButton rb_eipos)
        {
            _config = new DatabaseConfig();
            string connString = $"server={_config.Server};port={_config.Port};uid={_config.Uid};pwd={_config.Pwd};database={_config.Database}";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = "SELECT * FROM settings";

                MySqlCommand command = new MySqlCommand(query, conn);

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    tb_appname.Text = reader["set_appname"].ToString();
                    tb_adpictime.Text = reader["set_adpictime"].ToString();
                    tb_adpicpath.Text = reader["set_adpic"].ToString();
                    tb_advidtime.Text = reader["set_advidtime"].ToString();
                    tb_advidpath.Text = reader["set_advid"].ToString();
                    tb_disptime.Text = reader["set_disptime"].ToString();
                    // Retrieve the set_code value from the reader
                    int setCode = reader.GetInt32("set_code");

                    // Set the radio button based on the set_code value
                    if (setCode == 1)
                    {
                        rb_ipos.Checked = true;
                        rb_eipos.Checked = false;
                    }
                    else if (setCode == 2)
                    {
                        rb_ipos.Checked = false;
                        rb_eipos.Checked = true;
                    }
                }

                reader.Close();
                conn.Close();
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            _config = new DatabaseConfig();
            string connString = $"server={_config.Server};port={_config.Port};uid={_config.Uid};pwd={_config.Pwd};database={_config.Database}";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string checkedRadioButton = string.Empty;
                if (rb_ipos.Checked)
                    checkedRadioButton = "rb_ipos";
                else if (rb_eipos.Checked)
                    checkedRadioButton = "rb_eipos";

                string updateQuery = $"UPDATE settings SET set_code = CASE WHEN '{checkedRadioButton}' = 'rb_ipos' THEN 1 WHEN '{checkedRadioButton}' = 'rb_eipos' THEN 2 ELSE 0 END";
                MySqlCommand command = new MySqlCommand(updateQuery, conn);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            Controls.OfType<TextBox>().ToList().ForEach(textBox => textBox.Clear());
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Enable text boxes
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

  
    }
}