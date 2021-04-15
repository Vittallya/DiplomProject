using AirClub.Model.Location;
using AirClub.ViewModels;
using AirClub.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditTransfer.xaml
    /// </summary>
    public partial class EditTransfer : Page
    {
        public EditTransfer()
        {
            InitializeComponent();
            LoadPlaces();
        }

        IList<Place> Places { get; set; }

        async void LoadPlaces()
        {
            Places = (await LocationConverter.ConverFromFile()).ToList();
            tree.ItemsSource = Places;
        }

        int count;

        
       
        private void stack_Error(object sender, ValidationErrorEventArgs e)
        {
            count += e.Action == ValidationErrorEventAction.Added  ? 1: -1;
            btn.IsEnabled = count <= 0;            
        }



        private void ButtonChoose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = tree.SelectedItem as Place;
            string result = string.Empty;
            
            List<Place> ierarhy = new List<Place>();
            ierarhy.Add(item);

            Place parent = item.Parent;

            while (parent != null)
            {
                ierarhy.Add(parent);
                parent = parent.Parent;
            }

            ierarhy.Reverse();
            result = string.Join(", ", ierarhy.Select(x => x.FullName));
            

            if (DataContext is EditTransferViewModel vm && vm.ChooseLocation?.CanExecute(result) == true)
            {
                vm.ChooseLocation.Execute(result);
            }
        }

        private async void textFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            tree.ItemsSource = await LocationConverter.Filter(textFilter.Text);
        }



        private void MenuItemAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {           
            var editVm = AirClubLocator.ServiceProvider.GetRequiredService<EditPlaceViewModel>();
            if (editVm.Show(new WindowPlace(), out Place newPlace))
            {
                if (tree.SelectedItem == null)
                {
                    LocationConverter.Add(newPlace);
                }
                else if(tree.SelectedItem is Place place && place.Parent == null)
                {
                    LocationConverter.AddChildTo(newPlace, place);
                }
                else
                {
                    var sel = tree.SelectedItem as Place;
                    if(sel.Parent != null)
                    {
                        LocationConverter.AddChildTo(newPlace, sel.Parent);
                    }
                }
                tree.ItemsSource = LocationConverter.Places;
            }
        }

        private void MenuItemAddChild_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(tree.SelectedItem == null)
            {
                return;
            }

            var editVm = AirClubLocator.ServiceProvider.GetRequiredService<EditPlaceViewModel>();
            if (editVm.Show(new WindowPlace(), out Place newPlace))
            {
                if (tree.SelectedItem is Place place)
                {
                    LocationConverter.AddChildTo(newPlace, place);
                }
                tree.ItemsSource = LocationConverter.Places;
            }
        }

        private void MenuItemEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tree.SelectedItem == null)
            {
                return;
            }
            if (tree.SelectedItem is Place editable)
            {
                var editVm = AirClubLocator.ServiceProvider.GetRequiredService<EditPlaceViewModel>();
                if (editVm.Show(new WindowPlace(), out Place newPlace, editable))
                {
                    LocationConverter.Edit(newPlace, editable);
                    tree.ItemsSource = LocationConverter.Places;
                }
            }
        }

        private void MenuItemRemove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tree.SelectedItem == null)
            {
                return;
            }

            if (MessageBox.Show("Подтвердить удаление?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) 
                == MessageBoxResult.Yes)
            {
                if (tree.SelectedItem is Place place)
                {
                    LocationConverter.Remove(place);
                }
                tree.ItemsSource = LocationConverter.Places;
            }
        }
    }
}
