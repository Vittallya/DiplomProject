using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AirClub.ResoursesXamlAC.SlideAnimations
{
    public interface ISlideAnimation
    {
        void Execute(FrameworkElement animElement, double width, double heigth);

        ISlideAnimation Invert();
    }
}
