using AirClub.Model;
using AirClub.Model.Access;
using AirClub.Model.Db;
using AirClub.Pages;
using AirClub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirClub.ViewModels
{
    public class ReservationsViewModel : ElementsBaseViewModel<Reservation>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Бронирование";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Reservation> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Service.Name"] = "Услуга",
            ["Client.Name"] = "Имя клиента",
            ["Client.Surname"] = "Фамилия клиента",
            ["DateCreation"] = "Дата создания",
            ["DateReservation"] = "Дата бронирования",
        };

        public ReservationsViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;

        }

        public override IEditItemBase<Reservation> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditReservationViewModel>();

        #region Фильтр

        private IList<IFilterParam<Reservation>> filterParams;
        public override IList<IFilterParam<Reservation>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Reservation>[]
        {
            new FilterParam<Reservation, ComboBox>(filterClient)
            {
                Name = "Клиент",
                ValuePredicate = c => c.SelectedItem as Client,
                FilterPredicate = (item, control) =>
                {
                    return item.ClientId == (control as Client).Id;
                },
            },
            new FilterParam<Reservation, ComboBox>(filterInsurance)
            {
                Name = "Страховка",
                ValuePredicate = c => c.SelectedItem as Insurance,
                FilterPredicate = (item, control) =>
                {
                    return item.InsuranceId == (control as Insurance).Id;
                },
            },
            new FilterParam<Reservation, ComboBox> (filterService)
            {
                Name = "Услуга",
                ValuePredicate = c => c.SelectedItem as Service,
                FilterPredicate = (item, c) =>
                {
                    return item.ServiceId == (c as Service).Id;
                },
            },

            new FilterParam<Reservation, DatePicker>
            {
                Name = "Дата создания",
                FilterPredicate = (item, c) =>
                {
                    if(c != null && c is DateTime dt)
                    {
                        return item.DateCreation.Date == dt.Date;
                    }
                    return true;
                },
            },

            new FilterParam<Reservation, DatePicker>
            {
                Name = "Дата бронирования",
                FilterPredicate = (item, c) =>
                {
                    if(c != null && c is DateTime dt)
                        return item.DateReservation.Date == dt.Date;
                    return true;
                },
            },
        });


        ComboBox filterInsurance = new ComboBox()
        {
            DisplayMemberPath = "Name"
        };
        ComboBox filterClient = new ComboBox()
        {
            DisplayMemberPath = "Name"
        };
        ComboBox filterService= new ComboBox()
        {
            DisplayMemberPath = "Name"
        };

        #endregion

        public override Reservation ItemCopy => new Reservation();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddReservation;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditReservation;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.Insurances.LoadAsync();
            await _dbContext.Clients.LoadAsync();
            await _dbContext.Services.LoadAsync();

            await _dbContext.Reservations.LoadAsync();

            Elements = await GetCollectionDefault();

            if (filterInsurance.ItemsSource == null)
            {
                filterInsurance.ItemsSource = _dbContext.Insurances.Local.ToObservableCollection();
                filterClient.ItemsSource = _dbContext.Clients.Local.ToObservableCollection();
                filterService.ItemsSource = _dbContext.Services.Local.ToObservableCollection();
            }
        }


        public override async Task<ObservableCollection<Reservation>> GetCollectionDefault()
        {
            return _dbContext.Reservations.Local.ToObservableCollection();
        }
    }
}
