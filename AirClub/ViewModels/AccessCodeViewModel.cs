using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using AirClub.Model;
using AirClub.Model.Access;
using AirClub.ViewModels;
using System.Collections.ObjectModel;
using AirClub.Services;
using AirClub.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace AirClub.ViewModels
{
    public class AccessCodeViewModel: BindableBase
    {
        private readonly ICurrentUserService _currentUser;
        public AccessCodeViewModel(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }
        public ObservableCollection<ISectionView> AccessItems { get; set; }
        AccessCodeWindow _window;
        public bool ShowWindow(string code, out string outCode)
        {
            AccessItems = new ObservableCollection<ISectionView>(
               AccessCodeConverter.ConvertFromCodeToSectionsView(code, _currentUser.AccessSections).Result
            );
            _window = new AccessCodeWindow();
            _window.DataContext = this;
            var res = _window.ShowDialog() ?? false;
            outCode = res ? AccessCodeConverter.ConvertFromSectionsViewToCode(AccessItems) : null;
            return res;
        }
        public ICommand Accept => new DelegateCommand(() =>
        {
            _window.DialogResult = true;
        });
    }
}
