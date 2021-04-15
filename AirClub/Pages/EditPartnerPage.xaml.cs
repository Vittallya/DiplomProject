using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditPartnerPage.xaml
    /// </summary>
    public partial class EditPartnerPage : Page
    {
        public EditPartnerPage()
        {
            InitializeComponent();
        }

        int a;
        private void stack_Error(object sender, ValidationErrorEventArgs e)
        {
            a += e.Action == ValidationErrorEventAction.Added ? 1 : -1;
            btn.IsEnabled = a <= 0;
        }
    }
}
