using Microsoft.EntityFrameworkCore;
using PlayZone.Models;

namespace PlayZone.Data;

public class PlayZoneDbContext : DbContext
{
    public PlayZoneDbContext(DbContextOptions<PlayZoneDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<BlockedSlot> BlockedSlots { get; set; } = null!;
    public DbSet<AdminUser> AdminUsers { get; set; } = null!;
    public DbSet<WhatsAppConfig> WhatsAppConfigs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Room configuration
        modelBuilder.Entity<Room>(entity =>
  {
            entity.HasKey(e => e.Id);
       entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
 entity.Property(e => e.Available).HasDefaultValue(true);
   entity.HasIndex(e => e.Name);
        });

   // Booking configuration
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
    
        entity.HasOne(e => e.Room)
   .WithMany(r => r.Bookings)
     .HasForeignKey(e => e.RoomId)
  .OnDelete(DeleteBehavior.Cascade);

            // Indexes for efficient querying
            entity.HasIndex(e => new { e.RoomId, e.Date });
    entity.HasIndex(e => new { e.Date, e.StartTime, e.EndTime });
entity.HasIndex(e => new { e.RoomId, e.Date, e.StartTime, e.EndTime });
   });

        // BlockedSlot configuration
      modelBuilder.Entity<BlockedSlot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            
     entity.HasOne(e => e.Room)
 .WithMany(r => r.BlockedSlots)
        .HasForeignKey(e => e.RoomId)
      .OnDelete(DeleteBehavior.Cascade);

      // Indexes for efficient querying
            entity.HasIndex(e => new { e.RoomId, e.Date });
   entity.HasIndex(e => new { e.RoomId, e.Date, e.StartTime, e.EndTime });
        });

        // AdminUser configuration
        modelBuilder.Entity<AdminUser>(entity =>
        {
  entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
 entity.HasIndex(e => e.Username).IsUnique();
        });

    // WhatsAppConfig configuration
        modelBuilder.Entity<WhatsAppConfig>(entity =>
  {
          entity.HasKey(e => e.Id);
        });

    // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
  // Seed 8 rooms
        var rooms = new List<Room>();
        for (int i = 1; i <= 8; i++)
    {
        rooms.Add(new Room
     {
   Id = i,
    Name = $"Room {i}",
        Description = $"PlayStation Gaming Room {i}",
       Available = true,
     CreatedAt = DateTime.UtcNow,
  UpdatedAt = DateTime.UtcNow
            });
        }
        modelBuilder.Entity<Room>().HasData(rooms);

  // Seed default admin user
        modelBuilder.Entity<AdminUser>().HasData(new AdminUser
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
     Username = "admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            CreatedAt = DateTime.UtcNow
        });

        // Seed WhatsApp config
    modelBuilder.Entity<WhatsAppConfig>().HasData(new WhatsAppConfig
 {
            Id = 1,
    BusinessNumber = "+201234567890",
            UpdatedAt = DateTime.UtcNow
     });
 }
}
