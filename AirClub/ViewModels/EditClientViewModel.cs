using AirClub.Model;
using AirClub.Services;
using System;
using System.Windows.Controls;
using AirClub.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AirClub.Model.Access;
using AirClub.Model.Db;

namespace AirClub.ViewModels
{
    public class EditClientViewModel : EditItemBaseViewModel<Client>
    {

        public EditClientViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }


        public override Page View => new EditClientPage();

        public override string Title { get; protected set; } = "Профиль клиента";


        public ObservableCollection<Special> Specials { get; set; }

        public string[] GenderComboBox => new[] { "Мужской", "Женский" };

        public int SelectedGenderIndex { get; set; }

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddClients;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditClients;

        public override async Task InitIfAllModes()
        {            
            SelectedGenderIndex = Convert.ToInt32(!CurrentElement.Gender);

            if (!IsEdit)
            {
                CurrentElement.Name = "Имя";
                CurrentElement.Surname = "Фамилия";
                CurrentElement.Phone = "+7(123)456-78-90";
                CurrentElement.PasportData = "0000-000000";
            }
        }


        public override async Task BeforeClosing()
        {
            CurrentElement.Gender = !Convert.ToBoolean(SelectedGenderIndex);
        }
    }
}
