    using Hospital.DAL.Entities;
    using Hospital.DAL.Entities.config;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace Hospital.DAL.DataBase
    {
        public class HospitalDbContext : IdentityDbContext<ApplicationUser>
        {
            public HospitalDbContext(DbContextOptions<HospitalDbContext> opt) : base(opt) { }

            public DbSet<Doctor> Doctors { get; set; }

            public DbSet<Admin> Admins { get; set; }
            public DbSet<Patient> Patients { get; set; }
            public DbSet<MedicalRecord> MedicalRecords { get; set; }
            public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                //modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatientConfig).Assembly);
                   
          
            }

        }
    }
