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

namespace AirClub.ViewModels
{
    public class EmployeesViewModel : ElementsBaseViewModel<Employee>
    {

        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Сотрудики";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Employee> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Surname"] = "Фамилия",
            ["Name"] = "Имя",
            ["Phone"] = "Номер телефона",
            ["DateBirth"] = "Дата рождения",
            ["Special.Name"] = "Специальность",
        };

        public EmployeesViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        { 
            currentPage.DataContext = this;

        }



        public override IEditItemBase<Employee> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditEmployeeViewModel>();



        #region Фильтр     

        private IList<IFilterParam<Employee>> filterParams;
        public override IList<IFilterParam<Employee>> FilterParams => filterParams ?? ( filterParams = new IFilterParam<Employee>[]
        {
            
            new FilterParam<Employee, TextBox>
            {
                Name = "Фамилия",
                FilterPredicate = (item, c) =>
                item.Surname.ToLower().StartsWith(c.ToString().ToLower()),
            },
            new FilterParam<Employee, TextBox>
            {
                Name = "Имя",
                FilterPredicate = (item, c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },

            new FilterParam<Employee, ComboBox>(filterGenderCombo)
            {
                Name = "Пол",
                ValuePredicate = c => c.SelectedIndex,
                FilterPredicate = (item, c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.Gender == !Convert.ToBoolean(res);
                    }
                    return true;
                },
            },

            //new FilterParam<Employee, DatePicker>
            //{
            //    Name = "Дата рождения с",
            //    FilterPredicate = (item, c) =>
            //    item.DateBirth >= c.SelectedDate,
            //},

            //new FilterParam<Employee, DatePicker>
            //{
            //    Name = "Дата рождения до",
            //    FilterPredicate = (item, c) =>
            //    item.DateBirth <= c.SelectedDate,
            //},

            new FilterParam<Employee,ComboBox>(filterSpecialCombo)
            {
                Name = "Должность",
                FilterPredicate = (item, c) =>
                item.Special.Id == (c as Special).Id,
            },
        } );


        ComboBox filterSpecialCombo = new ComboBox();


        ComboBox filterGenderCombo = new ComboBox()
        {
            ItemsSource = new[] { "Мужской", "Женский" }
        };





        #endregion

        public override Employee ItemCopy => new Employee();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddEmployee;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditEmployee;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {

            await _dbContext.Employees.LoadAsync();
            await _dbContext.Specials.LoadAsync();

            Elements = _dbContext.Employees.Local.ToObservableCollection();


            if (this.filterSpecialCombo.ItemsSource == null)
            {
                this.filterSpecialCombo.ItemsSource = _dbContext.Specials.Local.ToObservableCollection();
            }
        }


        public override async Task<ObservableCollection<Employee>> GetCollectionDefault()
        {
            return _dbContext.Employees.Local.ToObservableCollection();
        }
    }

}
