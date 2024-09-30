using FitMate.Models;
using Microsoft.EntityFrameworkCore;

namespace FitMate.DataBase;

public class FitMateDB : DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Settings.Connection);
    }
}