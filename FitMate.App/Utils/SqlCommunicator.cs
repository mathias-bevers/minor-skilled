using Microsoft.Data.SqlClient;

namespace FitMate.Utils;

public static class SqlCommunicator
{
    private const string DEFAULT_ERROR = "An error occured while trying to communicate to the database";
    private const string VALUES = "VALUES(";
    private const string OUTPUT_VALUES = "OUTPUT INSERTED.Id VALUES(";
    
    private static readonly SqlConnection CONNECTION;

    static SqlCommunicator()
    {
        CONNECTION = new SqlConnection(App.SETTINGS.Server.ConnectionString);
    }

    
    public static async Task<int> Insert(string query, string? error = null)
    {
        await using SqlCommand command = new(string.Empty, CONNECTION);
        
        try
        {
            if (!query.Contains(VALUES))
            {
                error = $"invalid query: {query}";
                throw new Exception();
            }

            query = query.Replace(VALUES, OUTPUT_VALUES);
            command.CommandText = query;
            
            CONNECTION.Open();
            int insertedID = (int)command.ExecuteScalar();
            CONNECTION.Close();
            return insertedID;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            error = string.Concat(string.IsNullOrEmpty(error) ? DEFAULT_ERROR : error, Environment.NewLine, e.Message);
            CONNECTION.Close();
            throw new PopupException(error, "DATABASE ERROR");
        }
    }

    public static async Task Select(string query, Action<SqlDataReader> callback, string? error = null)
    {
        await using SqlCommand command = new(query, CONNECTION);
        
        try
        {
            CONNECTION.Open();
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    callback(reader);
                }
            }
            CONNECTION.Close();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            error = string.Concat(string.IsNullOrEmpty(error) ? DEFAULT_ERROR : error, Environment.NewLine, e.Message);
            throw new PopupException(error, "SQL select error");
        }
    }
}