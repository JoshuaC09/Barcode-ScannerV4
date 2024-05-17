using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
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

        public void LoadSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime, RadioButton rb_ipos, RadioButton rb_eipos)
        {
            string query = "SELECT * FROM settings";
            DataTable settingsTable = _databaseHelper.ExecuteQuery(query);

            if (settingsTable.Rows.Count > 0)
            {
                DataRow row = settingsTable.Rows[0];

                tb_appname.Text = row["set_appname"].ToString();
                tb_appname.GotFocus += TextBox_GotFocus;

                tb_adpictime.Text = row["set_adpictime"].ToString();
                tb_adpictime.GotFocus += TextBox_GotFocus;

                tb_adpicpath.Text = row["set_adpic"].ToString();
                tb_adpicpath.GotFocus += TextBox_GotFocus;

                tb_advidtime.Text = row["set_advidtime"].ToString();
                tb_advidtime.GotFocus += TextBox_GotFocus;

                tb_advidpath.Text = row["set_advid"].ToString();
                tb_advidpath.GotFocus += TextBox_GotFocus;

                tb_disptime.Text = row["set_disptime"].ToString();
                tb_disptime.GotFocus += TextBox_GotFocus;

                int setCode = Convert.ToInt32(row["set_code"]);
                rb_ipos.Checked = setCode == 1;
                rb_eipos.Checked = setCode != 1;
            }
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = string.Empty;
            textBox.Tag = null;
        }


        public void SaveSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime)
        {
            if (string.IsNullOrEmpty(tb_appname.Text) || string.IsNullOrEmpty(tb_adpictime.Text) || string.IsNullOrEmpty(tb_advidtime.Text) || string.IsNullOrEmpty(tb_disptime.Text) ||
                !int.TryParse(tb_adpictime.Text, out int adpictime) || !int.TryParse(tb_advidtime.Text, out int advidtime) || !int.TryParse(tb_disptime.Text, out int disptime))
            {
                MessageBox.Show("Please enter values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (adpictime == 0 || advidtime == 0 || disptime == 0)
            {
                MessageBox.Show("One of the fields is zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "UPDATE settings SET set_appname = @appname, set_adpictime = @adpictime, set_adpic = @adpicpath, set_advidtime = @advidtime, set_advid = @advidpath, set_disptime = @disptime";
            var parameters = new Dictionary<string, object>
            {
                { "@appname", tb_appname.Text },
                { "@adpictime", tb_adpictime.Text },
                { "@adpicpath", tb_adpicpath.Text.Replace("\\", "$") },
                { "@advidtime", tb_advidtime.Text },
                { "@advidpath", tb_advidpath.Text.Replace("\\", "$") },
                { "@disptime", tb_disptime.Text }
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
