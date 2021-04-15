using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AirClub.Services;
using AirClub.Windows;
using DevExpress.Mvvm;
using AirClub.Pages;
using AirClub.ResoursesXamlAC.SlideAnimations;
using AirClub.ViewModels.Interfaces;
using AirClub.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using AirClub.Pages.MainScreen;
using System.Linq;
using AirClub.Model.Db;

namespace AirClub.ViewModels
{
    public class MainViewModel : BindableBase, IWindowViewModel
    {
        private readonly EventBus eventBus;
        private readonly ICurrentUserService _currentUser;
        private readonly AirClubDbContext _dbContext;
        private readonly PageService _pageService;
        public Page CurrentPage { get; set; }

        private EditEmployeeViewModel _editEmployeeVm = AirClubLocator.ServiceProvider.GetRequiredService<EditEmployeeViewModel>();

        public MainViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService currentUser, AirClubDbContext dbContext)
        {
            eventBus = _eventBus;
            _currentUser = currentUser;
            _dbContext = dbContext;
            this._pageService = _pageService;

            this._pageService.Register(this, this.PageChange, typeof(PageStart));
            this._pageService.ChangePage(this, new PageStart());

            currentUser.UserEntered += CurrentUser_UserEntered;
            currentUser.UserExited += CurrentUser_UserExited;
        }
        private void CurrentUser_UserExited()
        {
            _pageService.ClearAllHistory(this);
            _pageService.ChangePage(this, new PageStart(), needBack: false);

            EnteredVisible = Visibility.Collapsed;
            UnautorizedVisible = Visibility.Visible;
            DbEmployee = null;
        }

        public Page HomePage { get; set; }

        private async void CurrentUser_UserEntered(Employee emp)
        {
            await _dbContext.Specials.LoadAsync();
            DbEmployee = emp;

            _pageService.ClearAllHistory(this);
            HomePage = SelectedPage = new PageUserStart();

            _pageService.ChangePage(this, SelectedPage, needBack: false);

            EnteredVisible = Visibility.Visible;
            UnautorizedVisible = Visibility.Collapsed;
        }

        public ICommand ToHomePage => new DelegateCommand(() =>
        {
            _pageService.ChangePage(this, HomePage);
            SideMenuExpanded = false;
        });

        public Visibility ButtonBackVisibility { get; set; } = Visibility.Hidden;

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.Back(this);           
        });

        public ICommand ShowCurrentProfile => new DelegateCommand(async () =>
        {
            ShowMode showMode = _currentUser.CurrentUser.SpecialId == 1 ? ShowMode.Edit : ShowMode.View;

            var copy = _currentUser.CurrentUser.Clone() as Employee;

            if (await _editEmployeeVm.ShowCurrent(this, copy, showMode))
            {
                _currentUser.CurrentUser.UpdateData(copy);
                (await _dbContext.Employees.FindAsync(copy.Id)).UpdateData(copy);
                await _dbContext.SaveChangesAsync();
            }
        });

        

        public void PageChange(Page page, bool needBack, ISlideAnimation slideAnimation)
        {
            ButtonBackVisibility = needBack ? Visibility.Visible : Visibility.Hidden;

            CurrentPage = page;

            if (Pages.Contains(page))
            {
                SelectedPage = page;
            }

            slideAnimation?.Execute(CurrentPage, Width, Height);            
        }

        public Visibility EnteredVisible { get; set; } = Visibility.Collapsed;
        public Visibility UnautorizedVisible { get; set; } = Visibility.Visible;

        public Employee DbEmployee { get; set; }
        public string SpecialName { get; set; }

        public ICommand BtnEnter => new DelegateCommand(() =>
        {
            //pageService.ChangePage(this, new PageMain());
            var window = new EnterWindow();
            window.ShowDialog();
        });


        public ICommand BtnExit => new DelegateCommand(() =>
        {
            _currentUser.OnUserExit();
        });

        public Window GetWindow(Page page)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _pageService.RemoveViewModel(this);
        }


        private async void test()
        {

        }


        #region Вид-модели

        private EmployeesViewModel _employeesVm => AirClubLocator.ServiceProvider.GetRequiredService<EmployeesViewModel>();
        private SpecialsViewModel _specialsVm => AirClubLocator.ServiceProvider.GetRequiredService<SpecialsViewModel>();
        private ClientsViewModel _clientsVm => AirClubLocator.ServiceProvider.GetRequiredService<ClientsViewModel>();

        void ChangePage(Page page)
        {
            SideMenuExpanded = false;
            if (page != null)
            {                
                _pageService.ChangePage(this, page, new SlideAnimationMove(AnimSlideDir.Back));
            }
        }

        #endregion

        public bool SideMenuExpanded { get; set; }


        #region Страницы

        public double Height { get; set; } = 450;
        public double Width { get; set; } = 850;


        public ICommand PageChangeCommand => new DelegateCommand(() => 
        {
            SideMenuExpanded = false;
            AnimSlideDir dir;

            if (oldPage1 != null)
            {
                var oldIndex = Pages.IndexOf(oldPage1);
                var newIndex = Pages.IndexOf(SelectedPage);

                dir = newIndex > oldIndex ? AnimSlideDir.Back : AnimSlideDir.Forw;

            }
            else
            {
                dir = AnimSlideDir.Forw;
            }

            oldPage1 = SelectedPage;
            _pageService.ChangePage(this, SelectedPage, new SlideAnimationMove(dir, 0.3, TranslateTransform.YProperty));

        });

        public ObservableCollection<Page> Pages { get; set; } = new ObservableCollection<Page>
        {
            new PageMenuServices()
            {
                Tag = "/Resources/service.png",
                Title = "Услуги",
            },
            new PageMenuTours()
            {
                Tag = "/Resources/Tour.png",
                Title = "Туры"
            },
            new PageMain()
            {
                Tag = "/Resources/employee.png",
                Title = "Кадры"
            },
            new ClientsPage()
            {
                Tag = "/Resources/client.png",
                Title = "Клиенты"
            },
            //new PageMenuStorage()
            //{
            //    Tag = "/Resources/warehouse.png",
            //    Title = "Склад"
            //},
            new PageMenuParnters()
            {
                Tag = "/Resources/partner.png",
                Title = "Партнеры"
            },
        };

        public Page SelectedPage { get; set; }

        private Page oldPage1;

        #endregion
    }
}
