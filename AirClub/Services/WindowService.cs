using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AirClub.Services
{
    public class WindowService
    {
        public Action<Window> OnWindowShow;

        public Window ActiveWindow { get; }

        public void ShowHelperWindow()
        {

        }

        public void ShowWindow(Window window) => OnWindowShow?.Invoke(window);
    }
}
