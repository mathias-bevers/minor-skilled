using System.Text;

namespace FitMate.DataBase;

public static class Settings
{
    private const string SERVER = "localhost";
    private const string DATABASE_NAME = "MinorTest";
    private const string USER_ID = "sa";
    private static readonly string PASSWORD = ReadPassword();

    public static string Connection
    {
        get
        {
            StringBuilder builder = new();
            builder.Append("Server=");
            builder.Append(SERVER);
            builder.Append(";Database=");
            builder.Append(DATABASE_NAME);
            builder.Append(";User Id=");
            builder.Append(USER_ID);
            builder.Append(";Password=");
            builder.Append(PASSWORD);
            builder.Append(";MultipleActiveResultSets=true");
            builder.Append(";Encrypt=false");
            builder.Append(';');
            return builder.ToString();
        }
    }
    
    private static string ReadPassword()
    {
        if (!File.Exists(".password")) { File.Create(".password").Close(); }

        return File.ReadAllText(".password");
    }
}