using FitMate.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FitMate.DataBase;

public class FitMateDB : DbContext
{
    private readonly SqlConnectionStringBuilder connectionStringBuilder;

    public FitMateDB(ServerSettings? serverSettings = null)
    {
        serverSettings ??= new ServerSettings();

        connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = serverSettings.Server,
            InitialCatalog = serverSettings.InitialCatalog,
            UserID = serverSettings.UserName,
            Password = serverSettings.Password,
            ConnectTimeout = serverSettings.ConnectionTimeout,
            TrustServerCertificate = true,
            Encrypt = true
        };
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(connectionStringBuilder.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Unidirectional relationships. 
        builder.Entity<User>().HasOne(u => u.Gender).WithMany().HasForeignKey(u => u.GenderID).IsRequired();
        builder.Entity<ExerciseType>().HasOne(et => et.MuscleGroup).WithMany().HasForeignKey(et => et.MuscleGroupID)
            .IsRequired();
        builder.Entity<ExerciseType>().HasOne(et => et.Measurement).WithMany().HasForeignKey(et => et.MeasurementTypeID)
            .IsRequired();
        builder.Entity<Exercise>().HasOne(e => e.ExerciseType).WithMany().HasForeignKey(e => e.ExerciseTypeID)
            .IsRequired();
        
        // follow relation table
        builder.Entity<Follow>().HasKey(f => new { f.FollowerID, f.FolloweeID });
        builder.Entity<Follow>().HasOne(f => f.Follower).WithMany(u => u.Following).HasForeignKey(f => f.FollowerID)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Follow>().HasOne(f => f.Followee).WithMany().HasForeignKey(f => f.FolloweeID);

        base.OnModelCreating(builder);
    }
}