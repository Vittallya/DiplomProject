using AirClub.Model.Location;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AirClub.ViewModels
{
    public class EditPlaceViewModel: BindableBase
    {
        public Place Item { get; set; }

        public string Header { get; set; } = "Добавить";

        public List<KeyValuePair<PlaceType, string>> PlaceNames { get; }
        = Place.PlaceNames.ToList();

        public EditPlaceViewModel()
        {
            
        }



        public bool Show(Window win, out Place place, Place inPlace = null)
        {
            Item = inPlace ?? new Place();

            if(inPlace != null)
            {
                Header = "Реадактировать";
            }
            else
            {
                Item.PlaceType = PlaceType.Place;
            }

            win.DataContext = this;
            var res = win.ShowDialog();

            place = Item;

            return res ?? false;
        }

        public ICommand Accept => new DelegateCommand<Window>(win =>
        {
            win.DialogResult = true;
        });
    }
}
