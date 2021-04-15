using Microsoft.EntityFrameworkCore;
using AirClub.Model.Db;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace AirClub.Model
{
    public class AirClubDbContext : DbContext
    {
        public AirClubDbContext() : base()
        {
            
            try
            {
                Database.EnsureCreated();
            }
            catch (SqlException)
            {
                //не удаляй здесь ничего, с одной строчкой приложение его пропускает сразу, а с двумя работает почему-то, хз
                MessageBox.Show("Произошла какая-то ошибка с MS SQL сервером. Хрен его знает че там пошло не так, поэтому я просто закрою это приложение, ну его нахер, а то вдруг там че-то еще какая-то херня возникнет и мне потом огребать ");
                MessageBox.Show("Произошла какая-то ошибка с MS SQL сервером. Хрен его знает че там пошло не так, поэтому я просто лучше закрою это приложение, ну его нахер, а то вдруг там че-то еще какая-то херня возникнет и мне потом огребать за это", "Фиксик", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransferTour>().
                HasKey(t => new { t.TransferId, t.TourId });

            modelBuilder.Entity<TransferTour>().
                HasOne(o => o.Tour).
                WithMany(m => m.TransferTours).
                HasForeignKey(x => x.TourId);

            modelBuilder.Entity<TransferTour>().
                HasOne(o => o.Transfer).
                WithMany(m => m.TransferTours).
                HasForeignKey(f => f.TransferId);

            modelBuilder.Entity<Transfer>().HasIndex(i => i.InvertedForId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConectionSettings.ConnectionStr);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Special> Specials { get; set; }
        public DbSet<Human> Humen { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCourse> ServiceCourses { get; set; }
        public DbSet<ServiceCompetition> ServiceCompetitions { get; set; }
        public DbSet<ServiceActiveRest> ServiceActiveRests { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<PlaceOld> Places { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<UserEnterAction> UserEnterActions { get; set; }
        public DbSet<UserTableOpenAction> UserTableOpenActions { get; set; }
        public DbSet<UserDbEditAction> UserDbEditActions { get; set; }

    }
}
