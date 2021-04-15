using AirClub.Model;
using AirClub.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using AirClub.Pages;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AirClub.Model.Access;

namespace AirClub.ViewModels
{
    public class EditSpecialViewModel: EditItemBaseViewModel<Special>
    {

        public EditSpecialViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _user, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _user, dbContext)
        {

        }

        public override Page View => new EditSpecialPage();

        public override string Title { get; protected set; } = "Специальность";

        public override async Task InitIfAllModes()
        {
            if (!IsEdit)
            {
                CurrentElement.AccessCode = (await AccessCodeConverter.GetDefaultCode(_currentUser.AccessSections)).Code;
                CurrentElement.Name = "Название";
                CurrentElement.Salary = 25000;
            }
        }

        public ICommand EditAccessCode => new DelegateCommand(() =>
        {
            var spVm = AirClubLocator.ServiceProvider.GetRequiredService<AccessCodeViewModel>();

            if (spVm.ShowWindow(CurrentElement.AccessCode, out string acCode))
            {
                CurrentElement.AccessCode = acCode;
            }

        });

        public ICommand CopyAccessCode => new DelegateCommand(() =>
        {
            Clipboard.Clear();
            Clipboard.SetText(CurrentElement.AccessCode);

        });

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddSpecial;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditSpecial;

        public bool CanReadAccessCode => _currentUser.GetUserPossibility(ReadEditIndex) >= 3;

        public bool CanEditAccessCode => _currentUser.GetUserPossibility(ReadEditIndex) == 4;

    }
}
