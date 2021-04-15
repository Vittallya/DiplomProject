using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AirClub.Model.Location;
using DevExpress.Mvvm.Native;
using Microsoft.Data.SqlClient;

namespace AirClub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SplashScreen screen = new SplashScreen("/Resources/startTitle.jpg");
            screen.Show(true);
            try
            {
                AirClubLocator.Init();
                base.OnStartup(e);
            }
            catch (Exception)
            {

            }
        }
    }
}
