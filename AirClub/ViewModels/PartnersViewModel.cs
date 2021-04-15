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
    public class PartnersViewModel : ElementsBaseViewModel<Partner>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Партнеры";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Partner> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название организации",
            ["INN"] = "ИНН",
            ["Address"] = "Адрес",
            ["FieldOfActivity"] = "Сфера деятельности",
        };

        #region Фильтр

        private IList<IFilterParam<Partner>> filterParams;
        public override IList<IFilterParam<Partner>> FilterParams => filterParams ?? (filterParams = new IFilterParam<Partner>[]
        {

            new FilterParam<Partner, TextBox>
            {
                Name = "Название организации",
                ValuePredicate = c => c.Text,
                FilterPredicate = (item, value) =>
                item.Name.ToLower().StartsWith(value.ToString().ToLower()),
            },
            new FilterParam<Partner, TextBox>
            {
                Name = "ИНН",
                ValuePredicate = c => c.Text,
                FilterPredicate = (item, value) =>
                item.INN.ToLower().StartsWith(value.ToString().ToLower()),
            },
            new FilterParam<Partner, TextBox>
            {
                Name = "Адрес",
                ValuePredicate = c => c.Text,
                FilterPredicate = (item, value) =>
                item.Address.ToLower().StartsWith(value.ToString().ToLower()),
            },
            new FilterParam<Partner, TextBox>
            {
                Name = "Сфера деятельности",
                ValuePredicate = c => c.Text,
                FilterPredicate = (item, value) =>
                {
                   return item.FieldOfActivity.ToLower().StartsWith(value.ToString().ToLower());
                },
            },
        });

        #endregion
        public PartnersViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;

        }



        public override IEditItemBase<Partner> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditPartnerViewModel>();




        public override Partner ItemCopy => new Partner();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddParnters;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditParnters;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.Partners.LoadAsync();

            Elements = _dbContext.Partners.Local.ToObservableCollection();
        }


        public override async Task<ObservableCollection<Partner>> GetCollectionDefault()
        {
            return _dbContext.Partners.Local.ToObservableCollection();
        }
    }

}
