using Microsoft.EntityFrameworkCore;
using HahnDroneAPI.Db.Entities;


namespace HahnDroneAPI.Db
{
    public class HahnDroneDBContext : DbContext
    {
        public HahnDroneDBContext(DbContextOptions<HahnDroneDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<DroneMedicationMaster>()
                        .HasIndex(x => new { x.DroneID, x.CapturedDate })
                        .IsUnique(true);

        }


        #region ========================================== Database Models ==========================================
        public virtual DbSet<Drone> Drones { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<Model> DroneModels { get; set; }
        public virtual DbSet<DroneMedicationMaster> DroneMedicationMaster { get; set; }
        public virtual DbSet<DroneMedicationDetail> DroneMedicationDetails { get; set; }
        public virtual DbSet<AuditEventLog> AuditEventLogs { get; set; }
        #endregion
    }
}
