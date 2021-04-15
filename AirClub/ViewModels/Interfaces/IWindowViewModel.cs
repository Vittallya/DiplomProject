using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AirClub.ViewModels.Interfaces
{
    public interface IWindowViewModel : IDisposable
    {
        Window GetWindow(Page page);
    }
}
