using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Price_Checker.Configuration;

namespace Price_Checker.Managers
{
    public class SettingsManager
    {
       
        public void LoadSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime, RadioButton rb_ipos, RadioButton rb_eipos)
        {
            string connString = ConnectionStringService.ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM settings";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tb_appname.Text = reader["set_appname"].ToString();
                        tb_adpictime.Text = reader["set_adpictime"].ToString();
                        tb_adpicpath.Text = reader["set_adpic"].ToString();
                        tb_advidtime.Text = reader["set_advidtime"].ToString();
                        tb_advidpath.Text = reader["set_advid"].ToString();
                        tb_disptime.Text = reader["set_disptime"].ToString();

                        int setCode = reader.GetInt32(reader.GetOrdinal("set_code"));
                        rb_ipos.Checked = setCode == 1;
                        rb_eipos.Checked = setCode != 1;
                    }
                }
            }
        }

        public void SaveSettings(TextBox tb_appname, TextBox tb_adpictime, TextBox tb_adpicpath, TextBox tb_advidtime, TextBox tb_advidpath, TextBox tb_disptime)
        {
            string connString = ConnectionStringService.ConnectionString;
            int adpictime, advidtime, disptime;

            // Check if any required fields are empty
            if (string.IsNullOrEmpty(tb_appname.Text) || string.IsNullOrEmpty(tb_adpictime.Text) || string.IsNullOrEmpty(tb_advidtime.Text) || string.IsNullOrEmpty(tb_disptime.Text) || !int.TryParse(tb_adpictime.Text, out adpictime) || !int.TryParse(tb_advidtime.Text, out advidtime) || !int.TryParse(tb_disptime.Text, out disptime))
            {
                MessageBox.Show("Please enter values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (adpictime == 0 || advidtime == 0 || disptime == 0)
            {
                MessageBox.Show("One of the fields is zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string query = $"UPDATE settings SET set_appname = '{tb_appname.Text}', set_adpictime = '{tb_adpictime.Text}', set_adpic = '{tb_adpicpath.Text.Replace("\\", "$")}', set_advidtime = '{tb_advidtime.Text}', set_advid = '{tb_advidpath.Text.Replace("\\", "$")}', set_disptime = '{tb_disptime.Text}'";
                MySqlCommand command = new MySqlCommand(query, conn);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Settings successfully saved.");
            }
        }

        public void UpdateRadioButton(RadioButton rb_ipos, RadioButton rb_eipos)
        {
            string connString = ConnectionStringService.ConnectionString;

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
    }
}