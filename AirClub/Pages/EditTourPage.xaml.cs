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
    /// Логика взаимодействия для EditTourPage.xaml
    /// </summary>
    public partial class EditTourPage : Page
    {
        public EditTourPage()
        {
            InitializeComponent();
        }

        int count = 0;
        private void StackPanel_Error(object sender, ValidationErrorEventArgs e)
        {
            count += e.Action == ValidationErrorEventAction.Added ? 1 : -1;
            btn.IsEnabled = count == 0;
        }
    }
}
