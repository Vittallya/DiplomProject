using AirClub.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using AirClub.Services;
using AirClub.ViewModels.Interfaces;
using AirClub.ViewModels.Validators;
using Microsoft.EntityFrameworkCore;
using AirClub.Model;
using AirClub.Model.Db;
using System.Windows;
using System;
using Microsoft.Data.SqlClient;

namespace AirClub
{
    public class AirClubLocator
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        //public static string DbConnection { get; } = $@"Data Source=.\SQLEXPRESS;AttachDbFilename='{Environment.CurrentDirectory}\AirClubDb.mdf';Integrated Security=True;Connect Timeout=30";

        public static void Init(Application app = null)
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<Model.AirClubDbContext>();


            services.AddScoped<MainViewModel>();
            services.AddTransient<EnterViewModel>();
            services.AddScoped<PageMainViewModel>();

            services.AddScoped<MenuViewModel>();


            services.AddTransient<EmployeesViewModel>();
            services.AddTransient<SpecialsViewModel>();
            services.AddTransient<ClientsViewModel>();


            services.AddTransient<EditSpecialViewModel>();
            services.AddTransient<EditClientViewModel>();
            services.AddTransient<EditEmployeeViewModel>();

            services.AddTransient<ServicesCompsViewModel>();
            services.AddTransient<ServicesCoursesViewModel>();
            services.AddTransient<ServicesActiveRestViewModel>();

            services.AddTransient<EditServiceViewModel<ServiceCourse>>();
            services.AddTransient<EditServiceViewModel<ServiceActiveRest>>();
            services.AddTransient<EditServiceViewModel<ServiceCompetition>>();

            services.AddTransient<ToursViewModel>();
            services.AddTransient<EditTourViewModel>();

            services.AddTransient<TransfersViewModel>();
            services.AddTransient<EditTransferViewModel>();


            services.AddTransient<PartnersViewModel>();
            services.AddTransient<EditPartnerViewModel>();

            services.AddTransient<InsurancesViewModel>();
            services.AddTransient<EditInsuranceViewModel>();


            services.AddTransient<ReservationsViewModel>();
            services.AddTransient<EditReservationViewModel>();

            services.AddTransient<AccessCodeViewModel>();

            services.AddTransient<EditPlaceViewModel>();

            services.AddTransient<UserHomeViewModel>();

            services.AddTransient<LoginRule>();

            services.AddTransient<IWindowViewModel, AnyWindowViewModel>();

            services.AddSingleton<PageService>();
            services.AddSingleton<WindowService>();
            services.AddSingleton<EventBus>();
            

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            ServiceProvider = services.BuildServiceProvider();

            foreach (var serv in services)
            {                
                ServiceProvider.GetRequiredService(serv.ServiceType);
            }
        }
        public MainViewModel MainViewModel => ServiceProvider.GetRequiredService<MainViewModel>();
        public EnterViewModel EnterViewModel => ServiceProvider.GetRequiredService<EnterViewModel>();
        public PageMainViewModel PageMainViewModel => ServiceProvider.GetRequiredService<PageMainViewModel>();
        public EditEmployeeViewModel EditEmployeeViewModel => ServiceProvider.GetRequiredService<EditEmployeeViewModel>();
        public EditSpecialViewModel EditSpecialViewModel => ServiceProvider.GetRequiredService<EditSpecialViewModel>();
        public EditClientViewModel EditClientViewModel => ServiceProvider.GetRequiredService<EditClientViewModel>();
        public IWindowViewModel AnyWindowViewModel => ServiceProvider.GetRequiredService<IWindowViewModel>();
        public AccessCodeViewModel AccessCodeViewModel => ServiceProvider.GetRequiredService<AccessCodeViewModel>();
        public MenuViewModel MenuViewModel { get; } = ServiceProvider.GetRequiredService<MenuViewModel>();
        public EditServiceViewModel<ServiceCompetition> EditServiceCourseViewModel => ServiceProvider.GetRequiredService<EditServiceViewModel<ServiceCompetition>>();
        public EditTourViewModel EditTourViewModel => ServiceProvider.GetRequiredService<EditTourViewModel>();

        public EditTransferViewModel EditTransferViewModel => ServiceProvider.GetRequiredService<EditTransferViewModel>();
        public EditPartnerViewModel EditPartnerViewModel => ServiceProvider.GetRequiredService<EditPartnerViewModel>();
        public EditInsuranceViewModel EditInsuranceViewModel => ServiceProvider.GetRequiredService<EditInsuranceViewModel>();

        public EditReservationViewModel EditReservationViewModel => ServiceProvider.GetRequiredService<EditReservationViewModel>();

        public EditPlaceViewModel EditPlaceViewModel => ServiceProvider.GetRequiredService<EditPlaceViewModel>();

        public UserHomeViewModel UserHomeViewModel => ServiceProvider.GetRequiredService<UserHomeViewModel>();
    }
}
