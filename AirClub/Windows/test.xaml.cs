using AirClub.Model.Location;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirClub.Windows
{
    /// <summary>
    /// Логика взаимодействия для test.xaml
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
            LoadD();
            //Test();
        }

        async void LoadD()
        {

            tree.ItemsSource = await LocationConverter.ConverFromFile();
        }

        void Test()
        {
            grid.ItemsSource = new List<Employee>
            {
                new Employee
                {
                    Name = "Серега",
                },
                new Employee
                {
                    Name = "Олег",
                    Id = 5,
                },
            };


            foreach(var cell in grid.Items)
            {
                
            }
        }


        List<Place> Places { get; set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            tree.ItemsSource = await LocationConverter.Filter(tb.Text);
        }
    }

    
}
