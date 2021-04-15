using AirClub.Model;
using AirClub.Services;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using AirClub.Events;
using System.Windows.Controls;
using AirClub.Pages;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using AirClub.ResoursesXamlAC.SlideAnimations;
using System.Windows.Media;
using AirClub.Model.Db;
using System.Windows;
using System.Collections.ObjectModel;
using AirClub.Pages.MainScreen;
using System.Threading.Tasks;

namespace AirClub.ViewModels
{
    public class MenuViewModel: BindableBase
    {

        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly ICurrentUserService _userService;

        private MainViewModel _mainViewModel => AirClubLocator.ServiceProvider.GetRequiredService<MainViewModel>();



        public MenuViewModel(PageService pageService, EventBus eventBus, ICurrentUserService userService)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _userService = userService;

            eventBus.Subscribe<ChangePage>(async x => await Task.Run(() => ChangePage(x.Page)));
        }

        void ChangePage(Page page)
        {
            if (page != null)
            {
                _pageService.ChangePage(_mainViewModel, page, new SlideAnimationMove(AnimSlideDir.Back, 0.3, TranslateTransform.XProperty));
            }
        }

        #region Вид-модели

        private EmployeesViewModel _employeesVm => AirClubLocator.ServiceProvider.GetRequiredService<EmployeesViewModel>();
        private SpecialsViewModel _specialsVm => AirClubLocator.ServiceProvider.GetRequiredService<SpecialsViewModel>();

        private ClientsViewModel _clientsVm => AirClubLocator.ServiceProvider.GetRequiredService<ClientsViewModel>();

        private ServicesCompsViewModel _serviceComps => AirClubLocator.ServiceProvider.GetRequiredService<ServicesCompsViewModel>();
        private ServicesCoursesViewModel _serviceCourses => AirClubLocator.ServiceProvider.GetRequiredService<ServicesCoursesViewModel>();

        private ServicesActiveRestViewModel _serviceRest => AirClubLocator.ServiceProvider.GetRequiredService<ServicesActiveRestViewModel>();

        private TransfersViewModel _transfers => AirClubLocator.ServiceProvider.GetRequiredService<TransfersViewModel>();
        private ToursViewModel _tours => AirClubLocator.ServiceProvider.GetRequiredService<ToursViewModel>();

        private PartnersViewModel _partners => AirClubLocator.ServiceProvider.GetRequiredService<PartnersViewModel>();

        private InsurancesViewModel _insurance => AirClubLocator.ServiceProvider.GetRequiredService<InsurancesViewModel>();

        private ReservationsViewModel _reservation => AirClubLocator.ServiceProvider.GetRequiredService<ReservationsViewModel>();


        #endregion

        #region Команды
        // ---------------------------------------------------------------
        public ICommand ShowEmployees => new DelegateCommand(() =>
        {
            ChangePage(_employeesVm.GetPage());
        });

        public ICommand ShowSpecials => new DelegateCommand(() =>
        {
            ChangePage(_specialsVm.GetPage());
        });
        // ---------------------------------------------------------------
        public ICommand ShowServiceCourses => new DelegateCommand(() =>
        {
            ChangePage(_serviceCourses.GetPage());
        });

        public ICommand ShowServiceComps => new DelegateCommand(() =>
        {
            ChangePage(_serviceComps.GetPage());
        });

        public ICommand ShowServiceActiveRest => new DelegateCommand(() =>
        {
            ChangePage(_serviceRest.GetPage());
        });

        // ---------------------------------------------------------------
        public ICommand ShowTours => new DelegateCommand(() =>
        {
            ChangePage(_tours.GetPage());
        });

        public ICommand ShowTransfers => new DelegateCommand(() =>
        {
            ChangePage(_transfers.GetPage());
        });

        // ---------------------------------------------------------------
        public ICommand ShowClients => new DelegateCommand(() =>
        {
            ChangePage(_clientsVm.GetPage());
        });
        public ICommand ShowReservation => new DelegateCommand(() =>
        {
            ChangePage(_reservation.GetPage());
        });


        public ICommand ShowInsurance => new DelegateCommand(() =>
        {
            ChangePage(_insurance.GetPage());
        });
        // ---------------------------------------------------------------
        public ICommand ShowStorages => new DelegateCommand(() =>
        {
            MessageBox.Show("Склады и аэродромы :)");
        });
        public ICommand ShowStorageItemTypes => new DelegateCommand(() =>
        {
            MessageBox.Show("Типы предметов склада :)");
        });

        public ICommand ShowJetModels => new DelegateCommand(() =>
        {
            MessageBox.Show("Модели самолетов :)");
        });

        public ICommand ShowJets => new DelegateCommand(() =>
        {
            MessageBox.Show("Самолеты :)");
        });

        public ICommand ShowStorage => new DelegateCommand(() =>
        {
            MessageBox.Show("Хранение :)");
        });
        // ---------------------------------------------------------------

        public ICommand ShowPartners=> new DelegateCommand(() =>
        {
            ChangePage(_partners.GetPage());
        });

        #endregion


    }
}
