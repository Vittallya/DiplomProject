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

namespace AirClub.ViewModels
{
    public class PageMainViewModel: BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly ICurrentUserService _userService;



        private EmployeesViewModel _employeesVm => AirClubLocator.ServiceProvider.GetRequiredService<EmployeesViewModel>();
        private SpecialsViewModel _specialsVm => AirClubLocator.ServiceProvider.GetRequiredService<SpecialsViewModel>();

        private MainViewModel _mainViewModel => AirClubLocator.ServiceProvider.GetRequiredService<MainViewModel>();
        private ClientsViewModel _clientsVm => AirClubLocator.ServiceProvider.GetRequiredService<ClientsViewModel>();

        private EditEmployeeViewModel _editEmployeeVm => AirClubLocator.ServiceProvider.GetRequiredService<EditEmployeeViewModel>();


        public PageMainViewModel(PageService pageService, EventBus eventBus, ICurrentUserService userService)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _userService = userService;

            //_employeesVm = AirClubLocator.ServiceProvider.GetRequiredService<EmployeesViewModel>();
            //_specialsVm = AirClubLocator.ServiceProvider.GetRequiredService<SpecialsViewModel>();
            //_mainViewModel = AirClubLocator.ServiceProvider.GetRequiredService<MainViewModel>();
            //_clientsVm = AirClubLocator.ServiceProvider.GetRequiredService<ClientsViewModel>();

        }


        void ChangePage(Page page)
        {
            if (page != null)
            { 
                _pageService.ChangePage(_mainViewModel, page, new SlideAnimationMove(AnimSlideDir.Back, 0.3, TranslateTransform.XProperty));
            }
        }

        public ICommand ShowEmployees => new DelegateCommand(() =>
        {
            ChangePage(_employeesVm.GetPage());            
        });

        public ICommand ShowSpecials=> new DelegateCommand(() =>
        {
            ChangePage(_specialsVm.GetPage());
        });

        public ICommand ShowClients => new DelegateCommand(() =>
        {
            ChangePage(_clientsVm.GetPage());
        });

        
    }
}
