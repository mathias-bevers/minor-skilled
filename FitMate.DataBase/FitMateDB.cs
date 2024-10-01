using FitMate.Models;
using Microsoft.EntityFrameworkCore;

namespace FitMate.DataBase;

public class FitMateDB : DbContext
{
    public DbSet<User> Users { get; set; }
    private IServerSettings serverSettings;

    public FitMateDB(IServerSettings? serverSettings = null)
    {
        if (ReferenceEquals(null, serverSettings))
        {
            this.serverSettings = new ServerSettings();
            return;
        }
        
        this.serverSettings = serverSettings;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer($"Server={serverSettings.Server};Database=FitMate;User Id={serverSettings.UserName};" +
                             $"Password={serverSettings.Password};MultipleActiveResultSets=true;Encrypt=false");
    }
}