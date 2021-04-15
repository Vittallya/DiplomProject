using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AirClub.Services;
using AirClub.Windows;
using DevExpress.Mvvm;
using AirClub.ResoursesXamlAC.SlideAnimations;
using AirClub.ViewModels.Interfaces;

namespace AirClub.ViewModels
{
    public class AnyWindowViewModel: BindableBase, IWindowViewModel
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly ICurrentUserService _currentUser;

        public Page CurrentPage { get; set; }

        public AnyWindowViewModel(PageService pageService, EventBus eventBus, ICurrentUserService currentUser)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _currentUser = currentUser;

        }

        public void PageChange(Page page, bool needBack, ISlideAnimation slideAnimation)
        {
            ButtonBackVisibility = needBack ? Visibility.Visible : Visibility.Hidden;

            CurrentPage = page;
            slideAnimation?.Execute(CurrentPage, 1920, 1080);            
        }

        public Window GetWindow(Page page)
        {
            _pageService.Register(this, PageChange, page.GetType());


            var window = new OtherWindowPage();
            window.DataContext = this;
            _pageService.ChangePage(this, page);

            return window;
        }

        public void Dispose()
        {
            _pageService.RemoveViewModel(this);
        }

        public ICommand Back => new DelegateCommand(() =>
        {
            _pageService.Back(this);
        });

        public Visibility ButtonBackVisibility { get; set; } = Visibility.Hidden;
    }
}
