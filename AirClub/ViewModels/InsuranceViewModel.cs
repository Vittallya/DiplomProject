using System;
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
    public class InsurancesViewModel : ElementsBaseViewModel<Insurance>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Страховка";

        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Insurance> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
            ["Cost"] = "Стоимость",
            ["PayementPeriod"] = "Период оплаты (дн.)",
            ["Compensation"] = "Тип компенсации",
            ["InsuranceType"] = "Тип страховки",
            ["Partner.Name"] = "Страховая компания",
            ["Validity"] = "Срок действия (дн.)",

        };
        //Типы страховок: страхование жизни и здоровья, имущества
        public InsurancesViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;

        }

        public override IEditItemBase<Insurance> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditInsuranceViewModel>();



        #region Фильтр     

        private IList<IFilterParam<Insurance>> filterParams;
        public override IList<IFilterParam<Insurance>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Insurance>[]
        {
            new FilterParam<Insurance, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item, c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Insurance, TextBox>
            {
                Name = "Стоимость",
                FilterPredicate = (item, c) =>
                item.Cost.ToString().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Insurance, TextBox>
            {
                Name = "Период оплаты",
                FilterPredicate = (item, c) =>
                item.PayementPeriod.ToString().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Insurance, TextBox>
            {
                Name = "Тип компенсации",
                FilterPredicate = (item, c) =>
                item.Compensation.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Insurance, TextBox>
            {
                Name = "Тип страховки",
                FilterPredicate = (item, c) =>
                item.InsuranceType.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Insurance, ComboBox>(filterPartner)
            {
                Name = "Страховая компания",
                ValuePredicate = c => c.SelectedItem as Partner,
                FilterPredicate = (item, c) =>
                item.PartnerId == (c as Partner).Id,
            },
        });

        ComboBox filterPartner = new ComboBox()
        {
            DisplayMemberPath = "Name",
        };


        #endregion

        public override Insurance ItemCopy => new Insurance();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddInsurance;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditInsurance;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {

            await _dbContext.Insurances.LoadAsync();
            await _dbContext.Partners.Where(x => x.FieldOfActivity == "Страхование").LoadAsync();

            Elements = _dbContext.Insurances.Local.ToObservableCollection();

            if (this.filterPartner.ItemsSource == null)
            {
                this.filterPartner.ItemsSource = _dbContext.Partners.Local.ToObservableCollection();
            }
        }


        public override async Task<ObservableCollection<Insurance>> GetCollectionDefault()
        {
            return _dbContext.Insurances.Local.ToObservableCollection();
        }
    }

}
