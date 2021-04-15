using System.Collections.Generic;
using AirClub.Services;
using AirClub.Model;
using AirClub.Model.Access;
using System.Windows.Controls;
using AirClub.Pages;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AirClub.Model.Db;
using System.Linq;

namespace AirClub.ViewModels
{
    public class TransfersViewModel : ElementsBaseViewModel<Transfer>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Трансферы";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Transfer> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["TransferAPoint"] = "Откуда",
            ["TransferBPoint"] = "Куда",
            ["Transport"] = "Транспорт",
            ["Partner.Name"] = "Партнер",
        };

        public TransfersViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;

        }

        public override IEditItemBase<Transfer> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditTransferViewModel>();

        #region Фильтр

        private IList<IFilterParam<Transfer>> filterParams;
        public override IList<IFilterParam<Transfer>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Transfer>[]
        {
            new FilterParam<Transfer, TextBox>
            {
                Name = "Откуда",
                FilterPredicate = (item, tb) =>
                item.TransferAPoint.ToLower().Contains(tb.ToString().ToLower()),
            },

            new FilterParam<Transfer, TextBox>
            {
                Name = "Куда",
                FilterPredicate = (item, c) =>
                item.TransferBPoint.ToLower().Contains(c.ToString().ToLower()),
            },
            new FilterParam<Transfer, TextBox>
            {
                Name = "Транспорт",
                FilterPredicate = (item, c) =>
                item.Transport.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Transfer, ComboBox>(filterParnter)
            {
                Name = "Транспортная компания",
                FilterPredicate = (item, c) =>
                {
                    return item.PartnerId == (c as Partner).Id;
                },
            },
        });

        ComboBox filterParnter = new ComboBox()
        {
            DisplayMemberPath = "Name"
        };



        #endregion

        public override Transfer ItemCopy => new Transfer();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddTransfer;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditTransfer;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.Transfers.LoadAsync();
            await _dbContext.Partners.
                Where(x => x.FieldOfActivity == "Транспорт" || x.FieldOfActivity == "Авиаперевозки").
                LoadAsync();

            Elements = await GetCollectionDefault();

            if(filterParnter.ItemsSource == null)
            {
                filterParnter.ItemsSource = _dbContext.Partners.Local.ToObservableCollection();
            }
        }


        public override async Task<ObservableCollection<Transfer>> GetCollectionDefault()
        {
            return _dbContext.Transfers.Local.ToObservableCollection();
        }
    }
}
