using AirClub.Model;
using AirClub.Services;
using System;
using System.Windows.Controls;
using AirClub.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AirClub.Model.Access;
using AirClub.Model.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Documents;
using System.Collections.Generic;
using AirClub.Model.Location;
using System.Windows.Automation.Provider;
using AirClub.Windows;

namespace AirClub.ViewModels
{
    public class EditTransferViewModel : EditItemBaseViewModel<Transfer>
    {

        public EditTransferViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }

        public ICommand ChoosePartner => new DelegateCommand(() =>
        {
            var vm = AirClubLocator.ServiceProvider.GetRequiredService<PartnersViewModel>();
            if(vm.ShowSlectionMode(out Partner partner, 3, "Транспорт|Авиаперевозки"))
            {
                SelectedPartner = partner;
            }

        });

        public ICommand AddPlace => new AsyncCommand(async () =>
        {
            

        });

        public override Page View => new EditTransfer();

        public override string Title { get; protected set; } = "Трансфер";

        public ObservableCollection<Partner> Partners { get; set; }

        public Partner SelectedPartner { get; set; }

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddTransfer;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditTransfer;

        public override void LoadDataToControlIfViewOrEdit(Transfer dbClass)
        {
            SelectedPartner = Partners.SingleOrDefault(x => x.Id == dbClass.PartnerId);
        }        
        public Place SelectedPlace { get; set; }

        public bool PanelVisible { get; set; }

        public bool AtFromPos { get; set; }

        public Thickness PanelMargin { get; set; } = new Thickness(0, 145, 0, 0);

        public ICommand ChooseLocation => new DelegateCommand<string>(loc =>
        {
            if (AtFromPos)
            {
                PointA = loc;
            }
            else
            {
                PointB = loc;
            }
            PanelVisible = false;
        });

        public string PointA { get; set; }

        public string PointB { get; set; }

        public ICommand ClosePanel => new DelegateCommand(() =>
        {
            PanelVisible = false;
        });

        public ICommand CommandFrom => new DelegateCommand(() =>
        {
            if (!AtFromPos)
            {
                PanelVisible = true;
                PanelMargin = new Thickness(0, 60, 0, 0);
            }
            else
            {
                PanelVisible = !PanelVisible;
            }
            AtFromPos = true;
        });

        public ICommand CommandTo => new DelegateCommand(() =>
        {
            if (AtFromPos)
            {
                PanelVisible = true;
                PanelMargin = new Thickness(0, 120, 0, 0);
            }
            else
            {
                PanelVisible = !PanelVisible;
            }
            AtFromPos = false;
        });

        public override async Task InitIfAllModes()
        {            
            await _dbContext.Partners.
                Where(x => x.FieldOfActivity == "Транспорт" || x.FieldOfActivity == "Авиаперевозки").LoadAsync();

            Partners = _dbContext.Partners.Local.ToObservableCollection();
            
            if (_showMode == ShowMode.Add)
            {
                CurrentElement.TransferAPoint = "Откуда";
                CurrentElement.TransferBPoint = "Куда";
                CurrentElement.Transport = "Транспорт";
                SelectedPartner = Partners.FirstOrDefault();
            }

            PointA = CurrentElement.TransferAPoint;
            PointB = CurrentElement.TransferBPoint;
        }


        public override async Task BeforeClosing()
        {
            CurrentElement.TransferAPoint = PointA;
            CurrentElement.TransferBPoint = PointB;

            _dbContext.Partners.Attach(SelectedPartner);
            CurrentElement.Partner = SelectedPartner;
        }
    }
}
