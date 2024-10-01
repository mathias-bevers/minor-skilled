using System.Text;

namespace FitMate.DataBase;

public class ServerSettings : IServerSettings
{
    private static string ReadPassword()
    {
        if (!File.Exists(".password")) { File.Create(".password").Close(); }

        return File.ReadAllText(".password");
    }

    public string Server => "localhost";
    public string UserName => "mathias";
    public string Password => ReadPassword();
}