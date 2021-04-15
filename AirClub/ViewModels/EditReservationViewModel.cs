using AirClub.Model;
using AirClub.Services;
using System;
using System.Windows.Controls;
using AirClub.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AirClub.Model.Access;
using AirClub.Model.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using DevExpress.Mvvm.Native;

namespace AirClub.ViewModels
{
    public class EditReservationViewModel : EditItemBaseViewModel<Reservation>
    {

        public EditReservationViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }

        public ICommand ChooseClient => new DelegateCommand(() =>
        {
            var vm = AirClubLocator.ServiceProvider.GetRequiredService<ClientsViewModel>();
            if (vm.ShowSlectionMode(out Client client))
            {
                SelectedClient = client;
            }
        });

        public ICommand ChooseInsurance => new DelegateCommand(() =>
        {
            var vm = AirClubLocator.ServiceProvider.GetRequiredService<InsurancesViewModel>();
            if (vm.ShowSlectionMode(out Insurance insurance))
            {
                SelectedInsurance = insurance;
            }
        });

        public ICommand ChooseService => new DelegateCommand(() =>
        {
            if(SelectedServiceType == typeof(ServiceCourse))
            {
                var vm = AirClubLocator.ServiceProvider.GetRequiredService<ServicesCoursesViewModel>();

                if (vm.ShowSlectionMode(out ServiceCourse serv))
                {
                    SelectedService = serv;
                }
            }
            else if (SelectedServiceType == typeof(ServiceCompetition))
            {
                var vm = AirClubLocator.ServiceProvider.GetRequiredService<ServicesCompsViewModel>();

                if (vm.ShowSlectionMode(out ServiceCompetition serv))
                {
                    SelectedService = serv;
                }
            }
            else if (SelectedServiceType == typeof(ServiceActiveRest))
            {
                var vm = AirClubLocator.ServiceProvider.GetRequiredService<ServicesActiveRestViewModel>();

                if (vm.ShowSlectionMode(out ServiceActiveRest serv))
                {
                    SelectedService = serv;
                }
            }

        });

        public override Page View => new EditReseravationPage();
        public override string Title { get; protected set; } = "Бронирование";
        public ObservableCollection<Insurance> Insurances { get; set; }

        public Insurance SelectedInsurance { get; set; }

        public Dictionary<string, Type> ServiceTypes { get; set; } = new Dictionary<string, Type>
        {
            ["Услуги обучения в летной школе"] = typeof(ServiceCourse),
            ["Услуги организации соревнований"] = typeof(ServiceCompetition),
            ["Услуги активного отдыха"] = typeof(ServiceActiveRest),
        };

        public Type SelectedServiceType { get; set; }

        public ObservableCollection<Service> Services { get; set; }


        public Service SelectedService { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public Client SelectedClient { get; set; }

        public DateTime MinDate { get; set; }

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddReservation;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditReservation;

        public override void LoadDataToControlIfViewOrEdit(Reservation dbClass)
        {
            SelectedServiceType = dbClass.Service.GetType();
            OnChangeType();

            SelectedInsurance = Insurances.SingleOrDefault(x => x.Id == dbClass.InsuranceId);
            SelectedService = Services.SingleOrDefault(x => x.Id == dbClass.ServiceId);
            SelectedClient = Clients.SingleOrDefault(x => x.Id == dbClass.ClientId);
        }


        public ICommand ServiceTypeChanged => new DelegateCommand(() =>
        {
            OnChangeType();            
            //для комбобовкса DisplayMemberName - Key, MemberValue - Value
            //сюда присылается SelecteVALUE
        });

        void OnChangeType()
        {
            Type type = SelectedServiceType;

            if(type == typeof(ServiceCourse))
            {
                Services = _dbContext.Services.Where(x => x is ServiceCourse).ToObservableCollection();
            }
            else if(type == typeof(ServiceCompetition))
            {
                Services = _dbContext.Services.Where(x => x is ServiceCompetition).ToObservableCollection();
            }
            else if (type == typeof(ServiceActiveRest))
            {
                Services = _dbContext.Services.Where(x => x is ServiceActiveRest).ToObservableCollection();
            }
            SelectedService = Services.FirstOrDefault();
        }

        public override async Task InitIfAllModes()
        {
            await _dbContext.Insurances.LoadAsync();
            await _dbContext.Clients.LoadAsync();
            await _dbContext.Services.LoadAsync();

            Insurances = _dbContext.Insurances.Local.ToObservableCollection();
            Clients = _dbContext.Clients.Local.ToObservableCollection();
            MinDate = DateTime.Now;

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.DateReservation = DateTime.Now.AddDays(3);
                SelectedInsurance = Insurances.FirstOrDefault();
                SelectedClient = Clients.FirstOrDefault();
                SelectedServiceType = typeof(ServiceCourse);
                OnChangeType();
            }
        }



        public override async Task BeforeClosing()
        {
            _dbContext.Insurances.Attach(SelectedInsurance);
            _dbContext.Clients.Attach(SelectedClient);
            _dbContext.Services.Attach(SelectedService);

            CurrentElement.Insurance = SelectedInsurance;
            CurrentElement.Service = SelectedService;
            CurrentElement.Client = SelectedClient;

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.DateCreation = DateTime.Now;
            }
        }
    }
}
