using AirClub.Events;
using AirClub.Model;
using AirClub.Model.Db;
using AirClub.Services;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AirClub.ViewModels
{
    public class UserHomeViewModel: BindableBase
    {
        private readonly PageService pageService;
        private readonly EventBus eventBus;
        private readonly ICurrentUserService _currentUser;
        private readonly AirClubDbContext _dbContext;

        public ObservableCollection<UserActionTableControl> OpenActions { get; set; }

        public ObservableCollection<UserActionTableControl> Possibilities { get; set; }

        public bool CaptionVis { get; set; }
        public bool CaptionActionsVis { get; set; }
        public bool ActionsVis { get; set; }


        public ObservableCollection<bool> animStack { get; set; } = new ObservableCollection<bool>();

        public ICommand TableOpen => new AsyncCommand<object>(async item =>
        {
            if(item is FrameworkElement element && element.DataContext is UserActionTableControl contr)
            {
                var vm = contr.ItemsBase;
                await eventBus.Publish(new ChangePage(vm.GetPage()));
            }
        });

        public int AnimValue { get; set; }

        public ICommand Loaded => new AsyncCommand(async () =>
        {
            await Task.Delay(250);
            CaptionVis = true;

            OpenActions = new ObservableCollection<UserActionTableControl>(await GetCorrect());

            if(OpenActions.Count > 0)
            {
                await Task.Delay(250);
                CaptionActionsVis = true;

                await Task.Delay(100);
                ActionsVis = true;
            }



        });


        private async Task NextAnimation(int milliseconds, int? value = null)
        {
            await Task.Delay(milliseconds);

            AnimValue = value.HasValue ? value.Value : ++AnimValue;
            
        }

        async Task<IEnumerable<UserActionTableControl>> GetCorrect()
        {
            var list = await _currentUser.GetUserTableActions();

            list = list.Reverse().Take(5);

            return list.Select(x => new UserActionTableControl(x)).Where(x => x.IsCorrect);
        }

        public UserHomeViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService currentUser, AirClubDbContext dbContext)
        {
            pageService = _pageService;
            eventBus = _eventBus;
            _currentUser = currentUser;
            _dbContext = dbContext;
        }
    }

    public class UserActionTableControl: BindableBase
    {
        public UserTableOpenAction Original { get; set; }
        public IEditItemsBaseViewModel ItemsBase { get; set; }
        public string Name { get; set; } = "Просто строка";

        public bool IsCorrect { get; set; }

        public string Date => Original.DateAction.ToString();

        public UserActionTableControl(UserTableOpenAction original)
        {
            Original = original;
            TryGetViewModel(original);
        }

        private void TryGetViewModel(UserTableOpenAction original)
        {
            try
            {
                var type = Type.GetType(original.VmType);
                //type.MakeGenericType(Type.GetType(original.DbClassType));
                ItemsBase = AirClubLocator.ServiceProvider.GetRequiredService(type) as IEditItemsBaseViewModel;

                Name = ItemsBase.Title;
                IsCorrect = true;
            }
            catch (Exception exc) { IsCorrect = false; Debug.WriteLine(exc.Message); }
        }

    }
}
