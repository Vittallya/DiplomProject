using AirClub.Model;
using AirClub.Model.Access;
using AirClub.Model.Db;
using AirClub.Pages;
using AirClub.Services;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AirClub.ViewModels
{
    public class EditTourViewModel : EditItemBaseViewModel<Tour>
    {

        public EditTourViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _user, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _user, dbContext)
        {

        }

        public override Page View => new EditTourPage();

        public override string Title { get; protected set; } = "Тур";

        public override async Task InitIfAllModes()
        {
            await _dbContext.Transfers.Include(x => x.Partner).LoadAsync();

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.Name = "Название";
            }
        }

        public async override Task<bool> Validate()
        {

            if(Transfers.Count <= 1)
            {
                MessageBox.Show("Как минимум два трансфера должны быть добавлены", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        public ObservableCollection<Transfer> Transfers { get; set; } = new ObservableCollection<Transfer>();

        public async override void LoadDataToControlIfViewOrEdit(Tour dbClass)
        {
            if(dbClass.TransferTours != null && dbClass.TransferTours.Count > 1)
            {
                Transfers = new ObservableCollection<Transfer>(dbClass.TransferTours.Select(x => x.Transfer));
            }
        }

        public async override Task BeforeClosing()
        {
            CurrentElement.TransferTours?.Clear();

            if (CurrentElement.TransferTours == null)
            {
                CurrentElement.TransferTours = new ObservableCollection<TransferTour>();
            }


            foreach (var tr in Transfers)
            {
                _dbContext.Transfers.Attach(tr);
                CurrentElement.TransferTours.Add(new TransferTour { Tour = CurrentElement, Transfer = tr });
            }
        }

        public ICommand AddTransfer => new DelegateCommand(() =>
        {
            var spVm = AirClubLocator.ServiceProvider.GetRequiredService<TransfersViewModel>();
            if (spVm.ShowSlectionMode(out Transfer transfer))
            {
                var tr = Transfers.SingleOrDefault(x => x.Id == transfer.Id);
                if (tr != null)
                {
                    tr.UpdateData(transfer);
                }
                else 
                {
                    Transfers.Add(transfer);
                }
            }
        });

        public ICommand RemoveTransfer => new DelegateCommand<object>(t =>
        {
            if (MessageBox.Show("Удалить трансфер?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (t is DockPanel panel && panel.DataContext is Transfer transfer)
                {
                    Transfers.Remove(transfer);
                }
            }
        });

        public ICommand UpdateTransfer => new DelegateCommand<object>(async t =>
        {
        if (t is DockPanel panel && panel.DataContext is Transfer transfer)
        {
            Transfer invTr = new Transfer();

                if (transfer.InvertedForId != null)
                {
                    //в данном случае transfer является обратным

                    if (Transfers.FirstOrDefault(x => x.Id == transfer.InvertedForId) != null)
                    {
                       MessageBox.Show("Такой трансфер уже есть!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    }
                    else
                    {
                        invTr = await _dbContext.Transfers.FirstOrDefaultAsync(x => x.Id == transfer.InvertedForId);
                        Transfers.Add(invTr);
                    }

                }
                else
                {
                    //в данном случае transfer является изначальным, ищем для него обратный

                    var exist = await _dbContext.Transfers.
                    FirstOrDefaultAsync(x => x.InvertedForId == transfer.Id);

                    if (exist != null)
                    {
                        //если обратный есть, добавл. его

                        if (Transfers.FirstOrDefault(x => x.Id == exist.Id) != null)
                        {
                            MessageBox.Show("Такой трансфер уже есть!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            Transfers.Add(exist);
                        }
                    }
                    else
                    {
                        //если нет - создаем

                        invTr.UpdateData(transfer);
                        invTr.TransferAPoint = transfer.TransferBPoint;
                        invTr.TransferBPoint = transfer.TransferAPoint;
                        invTr.Id = 0;
                        invTr.InvertedForId = transfer.Id;

                        _dbContext.Transfers.Add(invTr);
                        await _dbContext.SaveChangesAsync();
                        Transfers.Add(invTr);
                    }
                }
            }
        });

        public ICommand ChangeTransfer => new DelegateCommand<object>(t =>
        {
            if (t is DockPanel panel && panel.DataContext is Transfer transfer)
            {
                
                var spVm = AirClubLocator.ServiceProvider.GetRequiredService<TransfersViewModel>();
                spVm.SelectedElement = spVm.Elements.FirstOrDefault(x => x.Id == transfer.Id);
                spVm.EditElementProcess(transfer);

                if (spVm.ShowSlectionMode(out Transfer newTransfer))
                {
                    var tr = Transfers.FirstOrDefault(x => x.Id == newTransfer.Id);
                    if (tr != null)
                    {
                        tr.UpdateData(transfer);
                    }
                    else
                    {
                        Transfers.Add(transfer);
                    }
                }
            }
        });

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddTour;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditTour;


    }
}
