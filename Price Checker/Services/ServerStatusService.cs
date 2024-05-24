using System;
using System.Drawing;
using System.Windows.Forms;
using Price_Checker.Configuration;

namespace Price_Checker
{
    public class ServerStatusService
    {
        private DateTime lastOnlineTime = DateTime.MinValue;
        private bool wasOnlinePreviously = false;
        private readonly DatabaseHelper _dbHelper;

        public ServerStatusService()
        {
            _dbHelper = new DatabaseHelper(ConnectionStringService.ConnectionString);
        }

        public void UpdateStatusLabel(Label lbl_status, Panel bottomPanel)
        {
            string status = "Server Offline"; // Default status
            Color panelColor = Color.Red;

            try
            {
                var result = _dbHelper.ExecuteScalar("SELECT set_status FROM settings");
                if (result != null && Convert.ToInt32(result) == 1)
                {
                    status = "Server Online";
                    panelColor = Color.FromArgb(22, 113, 192);
                    lastOnlineTime = DateTime.Now;
                    wasOnlinePreviously = true;
                }
            }
            catch (Exception ex)
            {
                status = "Error";
                MessageBox.Show(ex.Message);
            }

            lbl_status.Text = $"{status} as of {(status == "Server Offline" ? lastOnlineTime : DateTime.Now)}";
            bottomPanel.BackColor = panelColor;
        }

        public void Appname(Label lbl_appname)
        {
            try
            {
                var result = _dbHelper.ExecuteScalar("SELECT set_appname FROM settings");
                lbl_appname.Text = result != null ? result.ToString() : "No app name found";
            }
            catch (Exception ex)
            {
                lbl_appname.Text = "Error retrieving app name";
                MessageBox.Show(ex.Message);
            }
        }
    }
}
