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

    public static async Task<int> Insert(SqlCommand command, string? error = null)
    {
        try
        {
            if (!command.CommandText.Contains(VALUES))
            {
                error = $"invalid query: {command}";
                throw new Exception();
            }

            command.CommandText = command.CommandText.Replace(VALUES, OUTPUT_VALUES);
            command.Connection = CONNECTION;

            CONNECTION.Open();
            int insertedID = (int)(await command.ExecuteScalarAsync() ?? -1);
            CONNECTION.Close();
            return insertedID;
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.Message);
            error = string.Concat(string.IsNullOrEmpty(error) ? DEFAULT_ERROR : error, Environment.NewLine, e.Message);
            CONNECTION.Close();
            throw new PopupException(error, "sql insert error");
        }
    }

    public static async Task Select(SqlCommand command, Action<SqlDataReader> callback, string? error = null)
    {
        try
        {
            command.Connection = CONNECTION;

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
            CONNECTION.Close();
            throw new PopupException(error, "SQL select error");
        }
    }
}