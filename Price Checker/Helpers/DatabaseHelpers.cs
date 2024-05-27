using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

public class DatabaseHelper : IDisposable
{
    private readonly string _connectionString;
    private MySqlConnection _connection;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new MySqlConnection(_connectionString);
        _connection.Open(); // Open the connection in the constructor
    }

    private void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
    {
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
        }
    }

    public MySqlDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
    {
        using (var command = new MySqlCommand(query, _connection))
        {
            AddParameters(command, parameters);
            return command.ExecuteReader();
        try
        {
            _connection.Open();
        }
        catch (Exception ex)
        {
            // Display error message in a message box
            MessageBox.Show("Error opening connection: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw; // Re-throw the exception if you want it to propagate
        }
    }

    private void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
    {
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                command.Parameters.AddWithValue(param.Key, param.Value);
            }
        }
    }

    public MySqlDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
    {
        try
        {
            var command = new MySqlCommand(query, _connection);
            AddParameters(command, parameters);
            return command.ExecuteReader(); // Note: Caller is responsible for closing the reader
        }
        catch (Exception ex)
        {
            // Display error message in a message box
            MessageBox.Show("Error executing reader: " + ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw; // Re-throw the exception if you want it to propagate
        }
    }

    public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        DataTable dataTable = new DataTable();
        try
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                AddParameters(command, parameters);
                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
        }
        catch (Exception ex)
        {
            // Display error message in a message box
            MessageBox.Show("Error executing query: " + ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw; // Re-throw the exception if you want it to propagate
        }
    }

    public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        DataTable dataTable = new DataTable();

        using (var command = new MySqlCommand(query, _connection))
        {
            AddParameters(command, parameters);

            using (var reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }
        }

        return dataTable;
    }

    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        using (var command = new MySqlCommand(query, _connection))
        {
            AddParameters(command, parameters);
            return command.ExecuteNonQuery();
        }
    }

    public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        using (var command = new MySqlCommand(query, _connection))
        {
            AddParameters(command, parameters);
            return command.ExecuteScalar();
    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        try
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                AddParameters(command, parameters);
                return command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Display error message in a message box
            MessageBox.Show("Error executing non-query: " + ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw; // Re-throw the exception if you want it to propagate
        }
    }

    public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        try
        {
            using (var command = new MySqlCommand(query, _connection))
            {
                AddParameters(command, parameters);
                return command.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            // Display error message in a message box
            MessageBox.Show("Error executing scalar: " + ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            throw; // Re-throw the exception if you want it to propagate
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
        if (_connection != null)
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                // Display error message in a message box
                MessageBox.Show("Error closing connection: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _connection.Dispose();
            }
        }
    }
}