using FitMate.Models;
using Microsoft.EntityFrameworkCore;

namespace FitMate.DataBase;

public class FitMateDB : DbContext
{
    public DbSet<User> Users { get; set; }
    private readonly IServerSettings serverSettings;

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Unidirectional relationships. 
        builder.Entity<User>().HasOne(u => u.Gender).WithMany().HasForeignKey(u => u.GenderID).IsRequired();
        builder.Entity<ExerciseType>().HasOne(et => et.MuscleGroup).WithMany().HasForeignKey(et => et.MuscleGroupID)
            .IsRequired();
        builder.Entity<ExerciseType>().HasOne(et => et.Measurement).WithMany().HasForeignKey(et => et.MeasurementTypeID)
            .IsRequired();
        builder.Entity<Exercise>().HasOne(e => e.ExerciseType).WithMany().HasForeignKey(et => et.ExerciseTypeName)
            .IsRequired();
        
        base.OnModelCreating(builder);
    }
}