using Price_Checker.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

public class DatabaseConfig
{
    private readonly SecurityService _securityService;

    #region Additional
    private const string encryptionKey = "In the eye of the beholder doth lie beauty's true essence, for each gaze doth fashion its own fair visage";
    private readonly byte[] _salt = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
    #endregion  

    public string Server { get; set; }
    public string Uid { get; set; }
    public string Port { get; set; }
    public string Pwd { get; set; }
    public string Database { get; set; }

    public DatabaseConfig()
    {
        // Initialize the SecurityService with the key and salt
        _securityService = new SecurityService(encryptionKey, _salt);

        //var enviroment = System.Environment.CurrentDirectory;
        //string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;
        //string appDirectory = projectDirectory;

        // Get the directory path of the currently executing assembly
        string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // Construct the config file path relative to the application directory
        string configFilePath = Path.Combine(appDirectory, "config.xml");

        try
        {
            if (File.Exists(configFilePath))
            {
                var doc = XDocument.Load(configFilePath);
                var databaseSettings = doc.Element("configuration")?.Element("databaseSettings");

                if (databaseSettings == null)
                {
                    throw new FormatException("Configuration section 'databaseSettings' is missing.");
                }

                Server = databaseSettings.Element("add")?.Attribute("server")?.Value;
                Uid = _securityService.Decrypt(databaseSettings.Element("add")?.Attribute("uid")?.Value);
                Port = databaseSettings.Element("add")?.Attribute("port")?.Value;
                Pwd = _securityService.Decrypt(databaseSettings.Element("add")?.Attribute("pwd")?.Value);
                Database = databaseSettings.Element("add")?.Attribute("database")?.Value;

                // Check if all values are correct
                if (string.IsNullOrEmpty(Server) || string.IsNullOrEmpty(Uid) || string.IsNullOrEmpty(Port) || string.IsNullOrEmpty(Pwd) || string.IsNullOrEmpty(Database))
                {
                    throw new FormatException("One or more configuration values are missing or incorrect.");
                }

                TryConnectToDatabase();
            }
            else
            {
                throw new FileNotFoundException($"The configuration file 'config.xml' was not found in the directory '{appDirectory}'.");
            }
        }
        catch (FileNotFoundException ex)
        {
            HandleException(ex, "The configuration file was not found.");
        }
        catch (CryptographicException ex)
        {
            HandleException(ex, "An error occurred while decrypting the data. Please check the encryption key and the encrypted data.");
        }
        catch (FormatException ex)
        {
            HandleException(ex, "One or more configuration values are missing or incorrect.");
        }
        catch (Exception ex)
        {
            HandleException(ex, "One or more configuration values are missing or incorrect");
        }
    }

    private static bool _hasConnected = false;
    private void TryConnectToDatabase()
    {
        if (_hasConnected)
        {
            return;
        }
        try
        {
            string connectionString = $"Server={Server};Port={Port};Database={Database};Uid={Uid};Pwd={Pwd};";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                if (!CheckDatabaseHasData(connection))
                {
                    string message = "There are no values in the database at this time.\nPress OK to proceed or Cancel to close.";
                    DialogResult result = MessageBox.Show(
                        message,
                        "No Data",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2 // Set Cancel as the default button
                    );
                    if (result == DialogResult.Cancel)
                    {
                        Environment.Exit(1);
                    }
                }
                _hasConnected = true;
            }
        }
        catch (MySqlException ex)
        {
            HandleException(ex, $"An error occurred while connecting to the MySQL server: {ex.Message}");
        }
    }

    private bool CheckDatabaseHasData(MySqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM prod_verifier";
        using (var command = new MySqlCommand(query, connection))
        {
            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }


    private void HandleException(Exception ex, string userMessage)
    {
        // Log the exception (you can replace this with your logging framework)
        Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");

        // Display the error message and exit the application
        MessageBox.Show(userMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // Exit the application
        Environment.Exit(1);
    }
}
