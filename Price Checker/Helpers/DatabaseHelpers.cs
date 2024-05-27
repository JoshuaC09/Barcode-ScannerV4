using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}