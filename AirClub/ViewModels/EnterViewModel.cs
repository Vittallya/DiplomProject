using AirClub.Services;
using DevExpress.Mvvm;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using AirClub.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AirClub.Model.Db;
using System;

namespace AirClub.ViewModels
{
    public class EnterViewModel : BindableBase
    {
        private readonly EventBus _eventBus;
        private readonly ICurrentUserService _currentUser;
        private readonly AirClubDbContext _dbContext;
        public EnterViewModel(EventBus eventBus, ICurrentUserService currentUser, AirClubDbContext dbContext)
        {
            _eventBus = eventBus;
            _currentUser = currentUser;
            _dbContext = dbContext;
        }
        public Visibility LoadingVisibility { get; set; } = Visibility.Hidden;
        public string Login { get; set; } = "vit";
        public PasswordBox PassBox { get; } = new PasswordBox() { Password = "123" };
        public string AnimateCommand { get; set; }
        public string ErrorMessage { get; set; }
        public async Task<Employee> FindEmployee()
        {
            string pass = PassBox.Password;
            retryCount++;

            await _dbContext.Employees.
                Include(x => x.Special).
                LoadAsync();

            var emp = await _dbContext.Employees.
                FirstOrDefaultAsync(x => x.Login.Equals(Login) 
                && x.Password.Equals(pass));

            return emp;
        }

        int retryCount;

        public ICommand Accept => new AsyncCommand<Window>(async window =>
        {
            ErrorMessage = "";
            LoadingVisibility = Visibility.Visible;
            var emp = await FindEmployee();
            LoadingVisibility = Visibility.Hidden;

            if (emp != null)
            {
                window.Close();
                await _currentUser.OnUserEnter(emp, retryCount);
            }
            else
            {
                ErrorMessage = "Пользователь не найден";
            }
        }, w => Login.Length > 2 && PassBox.Password.Length > 2);
    }
}
