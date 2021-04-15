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
    public class EditEmployeeViewModel: EditItemBaseViewModel<Employee>
    {

        public EditEmployeeViewModel(PageService _pageService, EventBus _eventBus, ICurrentUserService _currentUser, AirClubDbContext dbContext)
            : base(_pageService, _eventBus, _currentUser, dbContext)
        {

        }

        public bool LoginAndPassRead => _currentUser.GetUserPossibility(AccessSectionsIndex.ReadEditEmployee) >= 3;

        public bool LoginAndPassEdit => _currentUser.GetUserPossibility(AccessSectionsIndex.ReadEditEmployee) >= 4;

        public bool AccessCodeRead => _currentUser.GetUserPossibility(AccessSectionsIndex.ReadEditEmployee) >= 5;

        public bool AccessCodeEdit => _currentUser.GetUserPossibility(AccessSectionsIndex.ReadEditEmployee) == 6;

        public bool SpecialRead => _currentUser.GetUserPossibility(AccessSectionsIndex.ReadEditEmployee) >= 1;

        public override Page View => new EditEmployeePage();

        public override string Title { get; protected set; } = "Профиль сотрудника";

        public string EdDocSerial { get; set; } = "000000";
        public string EdDocNumber { get; set; } = "0000000";

        public ObservableCollection<Special> Specials { get; set; }

        public string[] GenderComboBox => new[] { "Мужской", "Женский" };

        public int SelectedGenderIndex { get; set; }

        async void SpecialChaged(Special special)
        {
            if(special == null)
            {
                return;
            }

            if (!AccessCodeConverter.IsAccessCodeCorrect(special.AccessCode, _currentUser.AccessSections))
            {
                var result = await AccessCodeConverter.GetDefaultCode(_currentUser.AccessSections);
                CurrentElement.AccessCode = result.Code;
            }

            CurrentElement.AccessCode = special.AccessCode;
        }

        public ICommand SpecialComboChanged => new DelegateCommand<Special>(sp => SpecialChaged(sp));

        public ICommand ChooseSpecial => new DelegateCommand(() =>
        {
            var spVm = AirClubLocator.ServiceProvider.GetRequiredService<SpecialsViewModel>();
            if (spVm.ShowSlectionMode(out Special special))
            {
                SelectedSpecial = special;
                SpecialChaged(special);
            }

        });

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

        public override async Task InitIfAllModes()
        {

            await _dbContext.Specials.LoadAsync();
            Specials = _dbContext.Specials.Local.ToObservableCollection();

            SelectedGenderIndex = Convert.ToInt32(!CurrentElement.Gender);

            if (_showMode == ShowMode.Add)
            {
                if(Specials.Count > 0)
                {
                    SelectedSpecial = Specials.FirstOrDefault();
                }

                CurrentElement.Name = "Имя";
                CurrentElement.Surname = "Фамилия";
                CurrentElement.Phone = "+7(123)456-78-90";
                CurrentElement.AccessCode = (await AccessCodeConverter.GetDefaultCode(_currentUser.AccessSections)).Code;
                CurrentElement.Login = "Логин";
                CurrentElement.Password = "Пароль";
                CurrentElement.DateBirth = DateTime.Now.AddYears(-28);
                CurrentElement.EdDocGetDate = CurrentElement.DateBirth.AddYears(22);
                CurrentElement.Gender = true;                
            }
        }

        string firstLogin;
        string firstPass;


        public override async Task<bool> Validate()
        {
            string login = CurrentElement.Login;
            string pass = CurrentElement.Password;

            await _dbContext.Employees.LoadAsync();

            if (_showMode == ShowMode.Add || _showMode == ShowMode.Edit && firstLogin != login)
            {

                if (await _dbContext.Employees.SingleOrDefaultAsync(x => x.Login == login) != null)
                {
                    LoadingVisibility = Visibility.Collapsed;
                    MessageBox.Show("Данный логин уже зарегистрирован", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            if (_showMode == ShowMode.Add || _showMode == ShowMode.Edit && firstPass != pass)
            {

                if (await _dbContext.Employees.SingleOrDefaultAsync(x => x.Password == pass) != null)
                {
                    LoadingVisibility = Visibility.Collapsed;

                   MessageBox.Show("Данный пароль уже зарегистрирован", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;

        }

        public override async Task BeforeClosing()
        {
            _dbContext.Specials.Attach(SelectedSpecial);
            CurrentElement.Special = SelectedSpecial;

            CurrentElement.EducationDoc = $"{EdDocSerial}-{EdDocNumber}";

            CurrentElement.Gender = !Convert.ToBoolean(SelectedGenderIndex);

            if (_showMode == ShowMode.Edit && _currentUser.CurrentUser?.Id == CurrentElement.Id)
            {
                await _currentUser.UpdateUser(CurrentElement);
                NeedReload = true;
            }          
        }

        public override void LoadDataToControlIfViewOrEdit(Employee emp)
        {            
            var id = emp?.Special?.Id;

            if (id.HasValue)
            {
                SelectedSpecial = _dbContext.Specials.Find(id);
            }

            if (_showMode == ShowMode.Edit)
            {
                firstLogin = emp.Login;
                firstPass = emp.Password;
            }

            try
            {
                var arr = CurrentElement.EducationDoc?.Split('-');
                EdDocSerial = arr?[0] ?? "000000";
                EdDocNumber = arr?[1] ?? "0000000";
            }
            catch (Exception)
            {
                EdDocSerial = "000000";
                EdDocNumber = "0000000";
            }
        }


        public Special SelectedSpecial { get; set; }

        public string Login { get; set; }

        public PasswordBox Password { get; set; }

        public override AccessSectionsIndex CanAddIndex => AccessSectionsIndex.AddEmployee;

        public override AccessSectionsIndex ReadEditIndex => AccessSectionsIndex.ReadEditEmployee;
    }
}
