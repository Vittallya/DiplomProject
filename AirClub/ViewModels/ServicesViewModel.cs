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
    /*
    public class ServicesCoursesViewModel<TService> : ElementsBaseViewModel<Service> where TService : Service, new() 
    {
        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Услуги";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<Service> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
            ["AgeFrom"] = "Возраст от",
            ["AgeBefore"] = "Возраст до",
            ["Cost"] = "Стоимость",
        };

        public ServicesCoursesViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;

            if(typeof(TService) == typeof(ServiceCourse))
            {
                BindingHeaders.Add("CourseDuration", "Продолжительность курса (мес.)");
                BindingHeaders.Add("ExersiceDuration", "Продолжительность занятия (мин.)");

                FilterParams.Add(new FilterParam<Service>
                {
                    Name = "Продолжительность курса (мес.)",
                    Control = filterCourseDuration,
                    FilterPredicate = item =>
                    (item as ServiceCourse)?.CourseDuration.ToString().StartsWith(filterCourseDuration.Text) ?? false,

                    FilterChanged = (sender, e) =>
                    {
                        RefreshFilter();
                    }
                });

                FilterParams.Add(new FilterParam<Service>
                {
                    Name = "Продолжительность занятия (мин.)",
                    Control = filterExDuration,
                    FilterPredicate = item =>
                    (item as ServiceCourse)?.ExersiceDuration.ToString().StartsWith(filterExDuration.Text) ?? false,

                    FilterChanged = (sender, e) =>
                    {
                        RefreshFilter();
                    }
                });
            }
            if (typeof(TService) == typeof(ServiceCompetition))
            {
                BindingHeaders.Add("DateBegin", "Дата начала");
                BindingHeaders.Add("DateEnd", "Дата окончания");
            }
            GenerateDataView();
        }



        public override IEditItemBase<Service> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditServiceViewModel<TService>>();




        #region Фильтр

        private IList<FilterParam<Service>> filterParams;
        public override IList<FilterParam<Service>> FilterParams => filterParams ?? (filterParams = new List<FilterParam<Service>>
        {


        });


        TextBox filterCourseDuration = new TextBox();

        TextBox filterExDuration = new TextBox();




        #endregion


        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddService;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditService;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        public override Service ItemCopy => new TService();
        #endregion

        public override async Task Reload()
        {
            await _dbContext.Services.LoadAsync();

            if (typeof(TService) == typeof(ServiceCourse))
            {
                await _dbContext.ServiceCourses.LoadAsync();
            }
            else if (typeof(TService) == typeof(ServiceCompetition))
            {
                await _dbContext.ServiceCompetitions.LoadAsync();
            }
            Elements = await GetCollectionDefault();
        }

        public override async Task AddElementProcess(Service element, AirClubDbContext db)
        {
            var type = element.GetType();

            if (typeof(TService) == typeof(ServiceCourse))
            {
                await _dbContext.ServiceCourses.AddAsync(element as ServiceCourse);
            }
            else if (typeof(TService) == typeof(ServiceCompetition))
            {
                await _dbContext.ServiceCompetitions.AddAsync(element as ServiceCompetition);
            }
        }

        public override async Task EditElementProcess(Service element)
        {
            if (typeof(TService) == typeof(ServiceCourse))
            {
                _dbContext.ServiceCourses.Update(element as ServiceCourse);
            }
            else if (typeof(TService) == typeof(ServiceCompetition))
            {
                _dbContext.ServiceCompetitions.Update(element as ServiceCompetition);
            }
        }

        public override async Task RemoveElementsProcess(Service[] elements, AirClubDbContext db)
        {
            if (typeof(TService) == typeof(ServiceCourse))
            {
                _dbContext.ServiceCourses.RemoveRange(elements.OfType<ServiceCourse>());
            }
            else if (typeof(TService) == typeof(ServiceCompetition))
            {
                _dbContext.ServiceCompetitions.RemoveRange(elements.OfType<ServiceCompetition>());
            }
        }

        public override async Task<ObservableCollection<Service>> GetCollectionDefault()
        {
            if (typeof(TService) == typeof(ServiceCourse))
            {
                var list = await _dbContext.ServiceCourses.Select(x => x as Service).ToListAsync();
                return new ObservableCollection<Service>(list);
            }
            else if (typeof(TService) == typeof(ServiceCompetition))
            {
                var list = await _dbContext.ServiceCompetitions.Select(x => x as Service).ToListAsync();
                return new ObservableCollection<Service>(list);
            }

            return _dbContext.Services.Local.ToObservableCollection();
        }
    }
    */

    public class ServicesCoursesViewModel : ElementsBaseViewModel<ServiceCourse>
    {
        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Услуги по организации обучения в летной школе";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<ServiceCourse> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
            ["AgeFrom"] = "Возраст от",
            ["AgeBefore"] = "Возраст до",
            ["Cost"] = "Стоимость",
            ["CourseDuration"] = "Продолжительность курса (мес.)",
            ["ExersiceDuration"] = "Продолжительность занятия (мин.)"
        };

        public ServicesCoursesViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;
        }
        public override IEditItemBase<ServiceCourse> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditServiceViewModel<ServiceCourse>>();



        #region Фильтр

        private IList<IFilterParam<ServiceCourse>> filterParams;
        public override IList<IFilterParam<ServiceCourse>> FilterParams => filterParams ?? (filterParams = new IFilterParam<ServiceCourse>[]
        {

            new FilterParam<ServiceCourse, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item,c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },

            new FilterParam<ServiceCourse, TextBox>
            {
                Name = "Возраст от",
                FilterPredicate = (item,c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom >= res;
                    }
                    return false;
                },
            },

            new FilterParam<ServiceCourse, TextBox>
            {
                Name = "Возраст до",
                FilterPredicate = (item,c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom <= res;
                    }
                    return false;
                },
            },
            new FilterParam<ServiceCourse, TextBox>
            {
                Name = "Стоимость",
                FilterPredicate = (item,c) =>
                item.Cost.ToString().StartsWith(c.ToString()),
            },
        });

        #endregion

        public override ServiceCourse ItemCopy => new ServiceCourse();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddService;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditService;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.ServiceCourses.LoadAsync();
            Elements = await GetCollectionDefault();
        }


        public override async Task<ObservableCollection<ServiceCourse>> GetCollectionDefault()
        {
            return _dbContext.ServiceCourses.Local.ToObservableCollection();
        }
    }

    public class ServicesCompsViewModel : ElementsBaseViewModel<ServiceCompetition>
    {
        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Услуги по организации сорвенований";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<ServiceCompetition> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
            ["AgeFrom"] = "Возраст от",
            ["AgeBefore"] = "Возраст до",
            ["Cost"] = "Стоимость",
            ["DateBegin"] = "Дата начала",
            ["DateEnd"] = "Дата окончания"
        };

        public ServicesCompsViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;
        }
        public override IEditItemBase<ServiceCompetition> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditServiceViewModel<ServiceCompetition>>();



        #region Фильтр

        private IList<IFilterParam<ServiceCompetition>> filterParams;
        public override IList<IFilterParam<ServiceCompetition>> FilterParams => filterParams ?? (filterParams = new IFilterParam<ServiceCompetition>[]
        {

            new FilterParam<ServiceCompetition, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item,c) =>
                item.Name.ToLower().StartsWith(c.ToString().ToLower()),
            },

            new FilterParam<ServiceCompetition, TextBox>
            {
                Name = "Возраст от",
                FilterPredicate = (item,c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom >= res;
                    }
                    return false;
                },
            },

            new FilterParam<ServiceCompetition, TextBox>
            {
                Name = "Возраст до",
                FilterPredicate = (item,c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom <= res;
                    }
                    return false;
                },
            },
            new FilterParam<ServiceCompetition, TextBox>
            {
                Name = "Стоимость",
                FilterPredicate = (item,c) =>
                item.Cost.ToString().StartsWith(c.ToString()),
            },
        });
        #endregion

        public override ServiceCompetition ItemCopy => new ServiceCompetition();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddService;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditService;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.ServiceCompetitions.LoadAsync();
            Elements = await GetCollectionDefault();
        }


        public override async Task<ObservableCollection<ServiceCompetition>> GetCollectionDefault()
        {
            return _dbContext.ServiceCompetitions.Local.ToObservableCollection();
        }
    }

    public class ServicesActiveRestViewModel : ElementsBaseViewModel<ServiceActiveRest>
    {
        private Page currentPage = new ElementsPage();
        public override Page CurrentView => currentPage;
        public override string Title => "Услуги по активному отдыху";


        public override int DataTemplateIndex => 1;

        public override ObservableCollection<ServiceActiveRest> Elements { get; set; }

        public override Dictionary<string, string> BindingHeaders { get; } = new Dictionary<string, string>
        {
            ["Id"] = "Идендификатор",
            ["Name"] = "Название",
            ["AgeFrom"] = "Возраст от",
            ["AgeBefore"] = "Возраст до",
            ["Cost"] = "Стоимость",
            ["MinCountPeople"] = "Мин. кол-во чел.",
            ["MaxCountPeople"] = "Макс. кол-во чел.",
            ["Tour.Name"] = "Тур"
        };

        public ServicesActiveRestViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext) :
            base(_pageService, _eventBus, _currentUser, dbContext)
        {
            currentPage.DataContext = this;
        }
        public override IEditItemBase<ServiceActiveRest> EditElementViewModel =>
            AirClubLocator.ServiceProvider.GetRequiredService<EditServiceViewModel<ServiceActiveRest>>();



        #region Фильтр

        private IList<IFilterParam<ServiceActiveRest>> filterParams;
        public override IList<IFilterParam<ServiceActiveRest>> FilterParams => filterParams ?? (filterParams = new IFilterParam<ServiceActiveRest>[]
        {

            new FilterParam<ServiceActiveRest, TextBox>
            {
                Name = "Название",
                FilterPredicate = (item, tb) =>
                item.Name.ToLower().StartsWith(tb.ToString().ToLower()),
            },

            new FilterParam<ServiceActiveRest, TextBox>
            {
                Name = "Возраст от",
                FilterPredicate = (item, c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom >= res;
                    }
                    return false;
                },
            },

            new FilterParam<ServiceActiveRest, TextBox>
            {
                Name = "Возраст до",
                FilterPredicate = (item, c) =>
                {
                    if(int.TryParse(c.ToString(), out int res))
                    {
                        return item.AgeFrom <= res;
                    }
                    return false;
                },
            },
            new FilterParam<ServiceActiveRest, TextBox>
            {
                Name = "Стоимость",
                FilterPredicate = (item, c) =>
                item.Cost.ToString().StartsWith(c.ToString()),
            },
            new FilterParam<ServiceActiveRest, ComboBox>(_filterTours)
            {
                Name = "Тур",
                ValuePredicate = c => c.SelectedItem as Tour,
                FilterPredicate = (item, c) =>
                item.TourId == (c as Tour).Id,
            },
        });

        ComboBox _filterTours = new ComboBox()
        {
            DisplayMemberPath = "Name",
        };

        #endregion

        public override ServiceActiveRest ItemCopy => new ServiceActiveRest();

        #region Параметры доступа

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddService;
        public override AccessSectionsIndex CanReadEditIndex => AccessSectionsIndex.ReadEditService;

        public override bool HasAccessHere => _currentUser.GetUserPossibility(CanAddIndex) > 0
            || _currentUser.GetUserPossibility(CanReadEditIndex) > 0;

        #endregion

        public override async Task Reload()
        {
            await _dbContext.ServiceActiveRests.LoadAsync();
            await _dbContext.Tours.LoadAsync();

            Elements = await GetCollectionDefault();

            if(_filterTours.ItemsSource == null)
            {
                _filterTours.ItemsSource = _dbContext.Tours.Local.ToObservableCollection();
            }
        }


        public override async Task<ObservableCollection<ServiceActiveRest>> GetCollectionDefault()
        {
            return _dbContext.ServiceActiveRests.Local.ToObservableCollection();
        }
    }
}
