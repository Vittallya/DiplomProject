using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для ElementsPage.xaml
    /// </summary>
    public partial class ElementsPage : Page
    {
        public ElementsPage()
        {
            InitializeComponent();
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void GridViewHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if(headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {

                if(headerClicked != _lastHeaderClicked )
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    if(_lastDirection == ListSortDirection.Ascending )
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }

                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                var sortBy = columnBinding?.Path?.Path ?? headerClicked.Column.Header as string;

                Sort(sortBy, direction);

                //Arrows header template

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;

            }

        }


        private void Sort(string sortBy, ListSortDirection direction)
        {


            ICollectionView collection = 
                CollectionViewSource.GetDefaultView(MainList.ItemsSource);

            if(collection == null)
            {
                return;
            }

            collection.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            collection.SortDescriptions.Add(sd);
            collection.Refresh();


        }
    }
}
