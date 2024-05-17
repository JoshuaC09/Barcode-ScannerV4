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

        public void UpdateStatusLabel(Label lbl_status, Panel bottomPanel)
        {
            string connstring = ConnectionStringService.ConnectionString;
            string status = "Server Offline"; // Default status
            Color panelColor = Color.Red;

            try
            {
                using (var con = new MySqlConnection(connstring))
                {
                    con.Open();
                    using (var cmd = new MySqlCommand("SELECT set_status FROM settings", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && reader.GetInt32(0) == 1)
                        {
                            status = "Server Online";
                            panelColor = Color.FromArgb(22, 113, 192);
                            lastOnlineTime = DateTime.Now;
                            wasOnlinePreviously = true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                status = "Error";
                MessageBox.Show(ex.Message);
            }

            lbl_status.Text = $"{status} as of {(status == "Server Offline" ? lastOnlineTime : DateTime.Now)}";
            bottomPanel.BackColor = panelColor;
        }

        public void Appname(Label lbl_appname)
        {
            string connstring = ConnectionStringService.ConnectionString;
            try
            {
                using (var con = new MySqlConnection(connstring))
                {
                    con.Open();
                    using (var cmd = new MySqlCommand("SELECT set_appname FROM settings", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        lbl_appname.Text = reader.Read() ? reader.GetString(0) : "No app name found";
                    }
                }
            }
            catch (MySqlException ex)
            {
                lbl_appname.Text = "Error retrieving app name";
                MessageBox.Show(ex.Message);
            }
        }
    }
}
