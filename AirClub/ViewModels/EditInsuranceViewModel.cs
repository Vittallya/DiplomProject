using AirClub.Model;
using AirClub.Services;
using System;
using System.Windows.Controls;
using AirClub.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AirClub.Model.Access;
using AirClub.Model.Db;
using System.Windows.Input;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace AirClub.ViewModels
{
    public class EditInsuranceViewModel : EditItemBaseViewModel<Insurance>
    {

        public EditInsuranceViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }

        public ICommand ChoosePartner => new DelegateCommand(() =>
        {
            var vm = AirClubLocator.ServiceProvider.GetRequiredService<PartnersViewModel>();
            if (vm.ShowSlectionMode(out Partner partner, 3, "Страхование"))
            {
                SelectedPartner = partner;
            }
        });

        public string[] CompensationsTypes { get; set; } = new[] { "Доставка на Родину", "Полная оплата лечения", "Возмещение ущерба" };

        public string[] InsuranceTypes { get; set; } = new[]
        { "Страхование жизни и здоровья", "Страхование имущества" };


        public int Validity { get; set; }

        public bool IsValidityUndefined { get; set; }


        public override Page View => new EditInsurancePage();

        public override string Title { get; protected set; } = "Страховка";


        public IList<Partner> Partners { get; set; }

        public Partner SelectedPartner { get; set; }


        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddInsurance;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditInsurance;

        public override async Task InitIfAllModes()
        {
            await _dbContext.Partners.LoadAsync();
            Partners = _dbContext.Partners.Where(x => x.FieldOfActivity == "Страхование").ToList();

            if (_showMode == ShowMode.Add)
            {
                CurrentElement.Name = "Название страховки";
                CurrentElement.PayementPeriod = 30;
                CurrentElement.Cost = 2500;
                CurrentElement.Compensation = CompensationsTypes.FirstOrDefault();
                CurrentElement.InsuranceType = InsuranceTypes.FirstOrDefault();
                CurrentElement.Validity = 180;
                SelectedPartner = Partners.FirstOrDefault();
            }
        }

        public override void LoadDataToControlIfViewOrEdit(Insurance dbClass)
        {
            SelectedPartner = Partners.SingleOrDefault(x => x.Id == dbClass.PartnerId);
            IsValidityUndefined = !dbClass.Validity.HasValue;
        }

        public async override Task<bool> Validate()
        {
            if(SelectedPartner == null)
            {
                MessageBox.Show("Необходимо выбрать из списка (или добавить) компанию-партнера для страховки", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        public override async Task BeforeClosing()
        {
            _dbContext.Partners.Attach(SelectedPartner);
            CurrentElement.Partner = SelectedPartner;
            if (IsValidityUndefined)
            {
                CurrentElement.Validity = null;
            }
        }
    }
}
