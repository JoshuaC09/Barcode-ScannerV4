using MySql.Data.MySqlClient;
using Price_Checker.Configuration;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace Price_Checker
{
    public class ServerStatusService
    {
        private DateTime lastOnlineTime = DateTime.MinValue;
        private bool wasOnlinePreviously = false;

        public void UpdateStatusLabel(Label lbl_status, Panel bottomPanel, Label lbl_appname)
        {
            DatabaseConfig _config = new DatabaseConfig();
            string connstring = ConnectionStringService.ConnectionString;
            string status = "Server Offline"; // Default status
            Color panelColor = Color.Red;

            try
            {
                using (MySqlConnection con = new MySqlConnection(connstring))
                {
                    con.Open();
                    string sql = "SELECT set_status FROM settings";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int statusValue = reader.GetInt32(0);
                        if (statusValue == 1)
                        {
                            lastOnlineTime = DateTime.Now;
                            wasOnlinePreviously = true;
                            status = "Server Online";
                            panelColor = Color.FromArgb(22, 113, 192);
                        }
                        else
                        {
                            if (wasOnlinePreviously)
                            {
                                status = "Server Offline";
                            }
                            else
                            {
                                status = "Server Offline";
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                status = "Error";
                panelColor = Color.Red; // Set panel color to red in case of error
                MessageBox.Show(ex.Message);
            }

            lbl_status.Text = $"{status} as of {(status == "Server Offline" ? lastOnlineTime.ToString() : DateTime.Now.ToString())}";
            bottomPanel.BackColor = panelColor; // Set bottom panel's back color
        }

        public void Appname(Label lbl_appname)
        {
            DatabaseConfig _config = new DatabaseConfig();
            string connstring = ConnectionStringService.ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connstring))
            {
                con.Open();
                string sql = "SELECT set_appname FROM settings";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string appName = reader.GetString(0);

                    reader.Close();

                    lbl_appname.Text = appName;
                }
                else
                {
                    lbl_appname.Text = "No app name found";
                }
            }
        }

    }
}
