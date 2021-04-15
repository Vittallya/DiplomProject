using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AirClub.Model;
using AirClub.Model.Access;
using AirClub.Model.Db;
using AirClub.Pages;
using AirClub.Services;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AirClub.ViewModels
{
    public class EditServiceViewModel<TService> : EditItemBaseViewModel<TService> where TService: Service, IDbClass, new()
    {
        public override Page View => new EditServceCoursePage();

        //public override string Title { get; private set; } = "Услуга";

        public override string Title { get; protected set; }

        public bool IsCourse { get; set; } = typeof(TService) == typeof(ServiceCourse);

        public bool IsComp { get; set; } = typeof(TService) == typeof(ServiceCompetition);

        public bool IsRest { get; set; } = typeof(TService) == typeof(ServiceActiveRest);

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddService;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditService;

        public ICommand ChooseTour => new DelegateCommand(() =>
        {
            var spVm = AirClubLocator.ServiceProvider.GetRequiredService<ToursViewModel>();
            if (spVm.ShowSlectionMode(out Tour tour))
            {
                SelectedTour = tour;
            }

        });

        public EditServiceViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }

        public ObservableCollection<Tour> Tours { get; set; }
        public Tour SelectedTour { get; set; }
        public override void LoadDataToControlIfViewOrEdit(TService dbClass)
        {
            if(dbClass is ServiceActiveRest rest)
            {
                var id = rest?.Tour?.Id;

                if (id.HasValue)
                {
                    SelectedTour = _dbContext.Tours.Find(id);
                }
            }

        }

        public override async Task<bool> Validate()
        {
            
            if (CurrentElement is ServiceActiveRest rest)
            {
                if (rest.MinCountPeople > rest.MaxCountPeople)
                {
                    LoadingVisibility = Visibility.Collapsed;
                    MessageBox.Show("Проверьте правильность заполнения минамального и максимального количества человек", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            else if (CurrentElement is ServiceCompetition comp)
            {
                if (comp.DateBegin > comp.DateEnd)
                {
                    LoadingVisibility = Visibility.Collapsed;
                    MessageBox.Show("Проверьте правильность заполнения даты начала и даты окончания", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            if(CurrentElement.AgeFrom > CurrentElement.AgeBefore)
            {
                LoadingVisibility = Visibility.Collapsed;
                MessageBox.Show("Проверьте правильность заполнения полей 'Возраст от' и 'Возраст до'", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }

        public override async Task BeforeClosing()
        {
            if(CurrentElement is ServiceActiveRest rest)
            {
                _dbContext.Tours.Attach(SelectedTour);
                rest.Tour = SelectedTour;
            }
        }

        public override async Task InitIfAllModes()
        {
            if (CurrentElement is ServiceActiveRest)
            {
                await _dbContext.Tours.LoadAsync();
                Tours = _dbContext.Tours.Local.ToObservableCollection();
            }

            if (IsCourse)
            {
                Title = "Услуга обучения в летной школе";
            }
            else if (IsComp)
            {
                Title = "Услуга участия в соревновании";
            }
            else if (IsRest)
            {
                Title = "Услуга активного отдыха";
            }

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.Name = "Название";
                CurrentElement.AgeFrom = 16;
                CurrentElement.AgeBefore = 45;
                CurrentElement.PhysReqs = "Физические требования";
                CurrentElement.Cost = 10000;

                if(CurrentElement is ServiceCourse course)
                {
                    course.CourseDuration = 2;
                    course.ExersiceDuration = 2;
                }

                else if(CurrentElement is ServiceCompetition comp)
                {
                    comp.DateBegin = DateTime.Now;
                    comp.DateEnd = DateTime.Now.AddMonths(6);
                }
                else if (CurrentElement is ServiceActiveRest rest)
                {
                    rest.MinCountPeople = 4;
                    rest.MaxCountPeople = 8;                    

                    if(Tours?.Count > 0)
                    {
                        SelectedTour = Tours.FirstOrDefault();
                    }
                }
            }
        }
    }

    
}
