namespace FitMate.DataBase;

internal class ServerSettings : IServerSettings
{
    public string Server => "localhost";
    public string UserName => "mathias";
    
    public string Password
    {
        get
        {
            if (!File.Exists(".password")) { File.Create(".password").Close(); }

            return File.ReadAllText(".password");
        }
    }
}