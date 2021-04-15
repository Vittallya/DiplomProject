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

namespace AirClub.ViewModels
{
    public class SpecialsViewModel : ElementsBaseViewModel<Special>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Специальности";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Special> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
        };

        public SpecialsViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        { 
            currentPage.DataContext = this;
        }
        public override IEditItemBase<Special> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditSpecialViewModel>();



        #region Фильтр

        private IList<IFilterParam<Special>> filterParams;
        public override IList<IFilterParam<Special>> FilterParams => filterParams ?? ( filterParams = new IFilterParam<Special>[]
        {
            
            new FilterParam<Special, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item, c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },
        } );

        #endregion

        public override Special ItemCopy => new Special();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddSpecial;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditSpecial;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            //using (var db = new AirClubDbContext())
            //{
            await _dbContext.Specials.LoadAsync();

            Elements = _dbContext.Specials.Local.ToObservableCollection();
           //}
        }


        public override async Task RemoveElementsProcess(Special[] elements, AirClubDbContext db)
        {                            
            _dbContext.Specials.RemoveRange(elements);             
        }

        public override async Task EditElementProcess(Special element)
        {
            await Task.Run(() =>
            {
                _dbContext.Specials.Update(element);
            });
        }

        public override async Task AddElementProcess(Special element, AirClubDbContext db)
        {
            await _dbContext.Specials.AddAsync(element);            
        }

        public override async Task<ObservableCollection<Special>> GetCollectionDefault()
        {
            return _dbContext.Specials.Local.ToObservableCollection();
        }
    }

}
