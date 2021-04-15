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
    public class EditPartnerViewModel : EditItemBaseViewModel<Partner>
    {

        public EditPartnerViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }


        public override Page View => new EditPartnerPage();

        public override string Title { get; protected set; } = "Организация";


        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddParnters;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditParnters;

        public override async Task InitIfAllModes()
        {

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.Name = "Название";
                CurrentElement.INN = "0000000000";
                CurrentElement.Address = "Адрес";
                CurrentElement.FieldOfActivity = "Сфера деятельности";
            }
        }

    }
}
