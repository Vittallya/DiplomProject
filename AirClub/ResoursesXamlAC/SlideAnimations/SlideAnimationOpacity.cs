using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AirClub.ResoursesXamlAC.SlideAnimations
{
    public class SlideAnimationOpacity : ISlideAnimation
    {
        public TimeSpan Length { get; }
        public double From { get; }
        public double To { get; }

        public void Execute(FrameworkElement animElement, double a, double b)
        {
            var animation = new DoubleAnimation();
            animation.Duration = Length;

            animation.From = From;

            animation.To = To;
            animElement.RenderTransform.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public ISlideAnimation Invert()
        {
            var fromInv = To;
            var toInv = From;

            return new SlideAnimationOpacity(fromInv, toInv, Length.TotalSeconds);
        }

        public SlideAnimationOpacity(double from, double to, double length)
        {
            From = from;
            To = to;
            To = to;
            Length = TimeSpan.FromSeconds(length);
        }
    }
}
