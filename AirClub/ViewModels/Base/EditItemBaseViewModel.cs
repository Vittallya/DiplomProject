using System;
using AirClub.Model;
using AirClub.Services;
using DevExpress.Mvvm;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using AirClub.ResoursesXamlAC.SlideAnimations;
using System.Windows.Media;
using AirClub.ViewModels.Interfaces;
using AirClub.Model.Db;
using AirClub.Model.Access;

namespace AirClub.ViewModels
{


    public abstract class EditItemBaseViewModel<TDbClass>: BindableBase, IEditItemBase<TDbClass> where TDbClass: class, IDbClass, new()
    {
        protected readonly PageService _pageService;
        protected readonly EventBus _eventBus;
        protected readonly ICurrentUserService _currentUser;
        protected readonly AirClubDbContext _dbContext;
        protected IWindowViewModel _windowViewModel;

        public bool NeedReload { get; protected set; }

        public EditItemBaseViewModel(PageService pageService, EventBus eventBus, ICurrentUserService currentUser, AirClubDbContext dbContext)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _currentUser = currentUser;
            _dbContext = dbContext;
        }

        public abstract Page View { get; }

        #region Уровни доступа

        public abstract AccessSectionsIndex CanAddIndex { get; }
        public abstract AccessSectionsIndex ReadEditIndex { get; }

        public bool CanAdd => _currentUser.GetUserPossibility(CanAddIndex) == 1;
        public bool CanRead => _currentUser.GetUserPossibility(ReadEditIndex) >= 1;
        public bool CanEdit => _currentUser.GetUserPossibility(ReadEditIndex) >= 2;


        protected virtual bool CheckAccess(ShowMode showMode)
        {
            if (showMode == ShowMode.Add && !CanAdd)
            {
               MessageBox.Show("Нет доступа к добавлению элементов в данном разделе","",MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (showMode == ShowMode.Edit && !CanEdit)
            {
                MessageBox.Show("Нет доступа к редактированию элементов в данном разделе", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (showMode == ShowMode.View && !CanRead)
            {
               MessageBox.Show("Нет доступа к чтению элементов в данном разделе", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        #endregion


        public virtual string Title { get; protected set; } = "Страница детального просмотра элемента";

        public TDbClass CurrentElement { get; set; }

        public virtual bool IsReadOnly { get; protected set; }

        protected ShowMode _showMode;
        public bool IsEdit { get; set; }

        protected virtual async void AcceptFunc()
        {
            LoadingVisibility = Visibility.Visible;
            var res = await Validate();
            LoadingVisibility = Visibility.Collapsed;

            if (res)
            {
                await BeforeClosing();
                Result = true;
            }

        }

        public virtual async Task<bool> Validate() { return true; }

        public virtual async Task BeforeClosing() { }

        protected virtual void BackFunc()
        {
            Result = false;
        }

        protected bool? Result;

        #region Команды
        public ICommand Accept => new DelegateCommand(() =>
        {
            if (_showMode == ShowMode.View)
            {
                BackFunc();
            }
            else
            {
                try
                {
                    AcceptFunc();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("EditItemBaseViewModel (ICommand Accept) -- 85 \n" + exc.Message);
                }
            }
        });

        public ICommand Back => new DelegateCommand(() =>
        {
            BackFunc();
        });

        #endregion

        #region инициалиция

        public async Task<bool> ShowCurrent(IWindowViewModel vm, TDbClass dbClass, ShowMode showMode)
        {

            if (!CheckAccess(showMode))
            {
                return false;
            }

            IsReadOnly = showMode == ShowMode.View;

            _showMode = showMode;

            if(showMode == ShowMode.Edit)
            {
                IsEdit = true;
            }

            await OnInit(dbClass);

            if (showMode == ShowMode.Edit || showMode == ShowMode.View)
            {
                LoadDataToControlIfViewOrEdit(dbClass);
            }
            ShowPage(vm);


            while ( !Result.HasValue )
            {
                await Task.Delay(10);
            }

            _pageService.Back(_windowViewModel);
            return Result.Value;
        }



        public virtual async Task InitIfAllModes() { }

        public virtual void LoadDataToControlIfViewOrEdit(TDbClass dbClass) { }

        async Task OnInit(TDbClass dbClass)
        {
            Result = null;
            CurrentElement = dbClass;
            await InitIfAllModes();
        }
        protected virtual void ShowPage(IWindowViewModel vm)
        {
            _windowViewModel = vm;
            var view = View;
            view.DataContext = this;

            _pageService.ChangePage(vm, view, new SlideAnimationMove(AnimSlideDir.Back, 0.3, TranslateTransform.XProperty));
        }
        #endregion


        #region Видимость 
        public Visibility IsReadOnlyVisibility => IsReadOnly ? Visibility.Visible : Visibility.Collapsed;

        public Visibility LoadingVisibility { get; set; } = Visibility.Collapsed;

        #endregion

    }

    public enum ShowMode
    {
        View, Add, Edit
    }

    public interface IEditItemBase<TDbClass> where TDbClass: class, IDbClass, new()
    {
        Page View { get; }
        string Title { get; }

        bool NeedReload { get; }

        TDbClass CurrentElement { get; }

        Task<bool> ShowCurrent(IWindowViewModel vm, TDbClass dbClass, ShowMode showMode);
        Task InitIfAllModes();
        void LoadDataToControlIfViewOrEdit(TDbClass dbClass);
    }

}
