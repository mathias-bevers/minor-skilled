namespace FitMate.DataBase;

public interface IServerSettings
{
    public string Server { get; }
    public string UserName { get; }
    public string Password { get; }
}