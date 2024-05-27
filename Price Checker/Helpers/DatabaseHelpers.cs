using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

public class DatabaseHelper
{
    private readonly string _connectionString;
    private MySqlConnection _connection;
    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }


    public MySqlDataReader ExecuteReader(string query, Dictionary<string, object> parameters = null)
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        using (var command = new MySqlCommand(query, _connection))
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            return command.ExecuteReader();
        }
    }
    // Method to execute a query and return the result as a DataTable
    public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        DataTable dataTable = new DataTable();

        try
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            // Handle the exception as needed
            Console.WriteLine($"Database Error: {ex.Message}");
        }

        return dataTable;
    }

    // Method to execute a non-query command (INSERT, UPDATE, DELETE)
    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
    {
        int rowsAffected = 0;

        try
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            // Handle the exception as needed
            Console.WriteLine($"Database Error: {ex.Message}");
        }

        return rowsAffected;
    }

    // Method to execute a scalar query (e.g., SELECT COUNT(*))
    public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
    {
        object result = null;

        try
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    result = command.ExecuteScalar();
                }
            }
        }
        catch (MySqlException ex)
        {
            // Handle the exception as needed
            Console.WriteLine($"Database Error: {ex.Message}");
        }

        return result;
    }
}