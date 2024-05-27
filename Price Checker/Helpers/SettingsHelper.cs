using System.Collections.Generic;
using System.Data;
using System;
using System.Windows.Forms;

namespace Price_Checker.SettingsHelpers
{
    internal class SettingsHelper
    {
        private readonly DatabaseHelper _databaseHelper;
        public SettingsHelper(string connectionString)
        {
            _databaseHelper = new DatabaseHelper(connectionString);
        }

        public void LoadSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime, TextBox tb_muldisptime, RadioButton rb_ipos, RadioButton rb_eipos)
        {
            string query = "SELECT * FROM settings";
            DataTable settingsTable = _databaseHelper.ExecuteQuery(query);

            if (settingsTable.Rows.Count > 0)
            {
                DataRow row = settingsTable.Rows[0];

                tb_appname.Text = row["set_appname"].ToString();
                tb_appname.Enter += TextBox_Enter;
                tb_appname.Click += TextBox_Click;

                tb_adpictime.Text = row["set_adpictime"].ToString();
                tb_adpictime.Enter += TextBox_Enter;
                tb_adpictime.Click += TextBox_Click;

                tb_adpicpath.Text = row["set_adpic"].ToString();
                tb_adpicpath.Enter += TextBox_Enter;
                tb_adpicpath.Click += TextBox_Click;

                tb_advidtime.Text = row["set_advidtime"].ToString();
                tb_advidtime.Enter += TextBox_Enter;
                tb_advidtime.Click += TextBox_Click;

                tb_advidpath.Text = row["set_advid"].ToString();
                tb_advidpath.Enter += TextBox_Enter;
                tb_advidpath.Click += TextBox_Click;

                tb_disptime.Text = row["set_disptime"].ToString();
                tb_disptime.Enter += TextBox_Enter;
                tb_disptime.Click += TextBox_Click;

                tb_muldisptime.Text = row["set_muldisptime"].ToString();
                tb_muldisptime.Enter += TextBox_Enter;
                tb_muldisptime.Click += TextBox_Click;

                int setCode = Convert.ToInt32(row["set_code"]);
                rb_ipos.Checked = setCode == 1;
                rb_eipos.Checked = setCode != 1;
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BeginInvoke(new Action(() => textBox.SelectAll()));
            }
        }
        private bool textBoxJustEntered = false;

        private void TextBox_Click(object sender, EventArgs e)
        {
            if (textBoxJustEntered)
            {
                (sender as TextBox).SelectAll();
            }
            textBoxJustEntered = false;
        }

        public void SaveSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime, TextBox tb_muldisptime)
        {
            if (string.IsNullOrEmpty(tb_appname.Text) ||
                string.IsNullOrEmpty(tb_adpictime.Text) ||
                string.IsNullOrEmpty(tb_advidtime.Text) ||
                string.IsNullOrEmpty(tb_disptime.Text) ||
                string.IsNullOrEmpty(tb_muldisptime.Text) ||

                !int.TryParse(tb_adpictime.Text, out int adpictime) ||
                !int.TryParse(tb_advidtime.Text, out int advidtime) ||
                !int.TryParse(tb_disptime.Text, out int disptime) ||
                !int.TryParse(tb_muldisptime.Text, out int muldisptime))
            {
                MessageBox.Show("Please enter values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if any of the time values exceed 7 digits
            if (tb_adpictime.Text.Length > 7 || tb_advidtime.Text.Length > 7 || tb_disptime.Text.Length > 7 || tb_muldisptime.Text.Length > 7)
            {
                MessageBox.Show("Input values for Ad Picture Time, Ad Video Time, and Display Time must not exceed 7 digits.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (adpictime == 0 || advidtime == 0 || disptime == 0 || muldisptime == 0)
            {
                MessageBox.Show("One of the fields is zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "UPDATE settings SET set_appname = @appname, set_adpictime = @adpictime, set_adpic = @adpicpath, set_advidtime = @advidtime, set_advid = @advidpath, set_disptime = @disptime, set_muldisptime = @muldisptime";
            var parameters = new Dictionary<string, object>
    {
        { "@appname", tb_appname.Text },
        { "@adpictime", tb_adpictime.Text },
        { "@adpicpath", tb_adpicpath.Text.Replace("\\", "$") },
        { "@advidtime", tb_advidtime.Text },
        { "@advidpath", tb_advidpath.Text.Replace("\\", "$") },
        { "@disptime", tb_disptime.Text },
        { "@muldisptime", tb_muldisptime.Text }
    };

            _databaseHelper.ExecuteNonQuery(query, parameters);
            MessageBox.Show("Settings successfully saved.");
        }

        public void UpdateRadioButton(RadioButton rb_ipos, RadioButton rb_eipos)
        {
            string checkedRadioButton = rb_ipos.Checked ? "1" : "2";
            string query = "UPDATE settings SET set_code = @setcode";
            var parameters = new Dictionary<string, object>
            {
                { "@setcode", checkedRadioButton }
            };

            _databaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}