using AirClub.Model;
using AirClub.Model.Access;
using AirClub.Model.Db;
using AirClub.Pages;
using AirClub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirClub.ViewModels
{
    public class ToursViewModel : ElementsBaseViewModel<Tour>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Туры";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Tour> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
        };

        public ToursViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;
        }
        public override IEditItemBase<Tour> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditTourViewModel>();



        #region Фильтр


        private IList<IFilterParam<Tour>> filterParams;
        public override IList<IFilterParam<Tour>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Tour>[]
        {

            new FilterParam<Tour, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item, c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },
        });


        TextBox filterName = new TextBox();

        #endregion

        public override Tour ItemCopy => new Tour();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddTour;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditTour;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.Tours.Include(x => x.TransferTours).ThenInclude(x => x.Transfer).LoadAsync();
            Elements = _dbContext.Tours.Local.ToObservableCollection();
        }

        public override async Task<ObservableCollection<Tour>> GetCollectionDefault()
        {
            return _dbContext.Tours.Local.ToObservableCollection();
        }
    }
}
