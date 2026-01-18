using System.Text.Json;
using System.Text.Json.Nodes;

namespace FitMate.DataBase;

public class ServerSettings
{
    public int ConnectionTimeout => 30;
    public string InitialCatalog => "fitmate";
    public string Password { get; }
    public string Server { get; }
    public string UserName => "mathias";

    public ServerSettings()
    {
        string filePath = "server-info.json";

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            throw new FileNotFoundException("server-info.json not found, creating...");
        }

        JsonNode node = JsonNode.Parse(File.ReadAllText(filePath)) ??
                        throw new FileLoadException("could not read file as json");

        Server = Convert.ToString(node["ip-address"]) ?? throw new JsonException("could not read node: ip-address");
        Password = Convert.ToString(node["password"]) ?? throw new JsonException("could not read node: password");
    }
}