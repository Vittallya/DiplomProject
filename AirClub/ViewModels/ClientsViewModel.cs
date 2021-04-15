using System;
using System.Collections.Generic;
using System.Text;
using AirClub.Events;
using AirClub.Services;
using AirClub.Model;
using AirClub.Model.Access;
using System.Windows.Controls;
using AirClub.Pages;
using System.Windows.Input;
using DevExpress.Mvvm;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using AirClub.Model.Db;

namespace AirClub.ViewModels
{
    public class ClientsViewModel : ElementsBaseViewModel<Client>
    {

        public override Page CurrentView { get; } = new ElementsPage();
        public override string Title => "Клиенты";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Client> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Surname"] = "Фамилия",
            ["Name"] = "Имя",
            ["Phone"] = "Номер телефона",
            ["DateBirth"] = "Дата рождения",
        };

        public ClientsViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            CurrentView.DataContext = this;

        }



        public override IEditItemBase<Client> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditClientViewModel>();



        #region Фильтр

        private IList<IFilterParam<Client>> filterParams;
        public override IList<IFilterParam<Client>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Client>[]
        {

            new FilterParam<Client, TextBox>
            {
                Name = "Фамилия",
                FilterPredicate = (item,c) =>
                item.Surname.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Client, TextBox>
            {
                Name = "Имя",
                FilterPredicate = (item,c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },

            new FilterParam<Client, ComboBox>(filterGenderCombo)
            {
                Name = "Пол",
                ValuePredicate = c => c.SelectedIndex,
                FilterPredicate = (item,c) =>
                {
                    if(int.TryParse(c.ToString(), out int val))
                    {
                        return item.Gender == !Convert.ToBoolean(val);
                    }
                    return true;
                },
            },

            //new FilterParam<Client, DatePicker>
            //{
            //    Name = "Дата рождения с",
            //    FilterPredicate = (item,c) =>
            //    item.DateBirth >= c.SelectedDate,
            //},

            //new FilterParam<Client, DatePicker>
            //{
            //    Name = "Дата рождения до",
            //    FilterPredicate = (item,c) =>
            //    item.DateBirth <= c.SelectedDate,
            //},
        });



        ComboBox filterGenderCombo = new ComboBox()
        {
            ItemsSource = new[] { "Мужской", "Женский" }
        };





        #endregion

        public override Client ItemCopy => new Client();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddClients;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditClients;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {

            await _dbContext.Clients.LoadAsync();

            Elements = _dbContext.Clients.Local.ToObservableCollection();
        }


        public override async Task<ObservableCollection<Client>> GetCollectionDefault()
        {
            return _dbContext.Clients.Local.ToObservableCollection();
        }
    }

}
