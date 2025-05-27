using Microsoft.Data.SqlClient;

namespace FitMate.DataBase;

public class ServerSettings
{
    public string Server => "tcp:fit-mate.database.windows.net,1433";
    public string UserName => "mathias";
    public string InitialCatalog => "FitMate";
    public int ConnectionTimeout => 30;

    public string Password
    {
        get
        {
            if (!File.Exists(".password")) { File.Create(".password").Close(); }

            return File.ReadAllText(".password");
        }
    }
}