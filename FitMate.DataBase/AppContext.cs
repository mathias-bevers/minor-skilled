using Microsoft.EntityFrameworkCore;

namespace FitMate.DataBase;

public class AppContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Settings.Connection);
    }
}