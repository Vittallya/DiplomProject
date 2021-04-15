using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AirClub.ResoursesXamlAC.SlideAnimations
{
    public class SlideAnimationMove : ISlideAnimation
    {
        public void Execute(FrameworkElement animElement, double width, double height)
        {

            var animation = new DoubleAnimation();
            animation.Duration = Length;
            double from;

            try
            {
                if (Dir == AnimSlideDir.Back)
                {
                    if (AnimProperty == TranslateTransform.XProperty)
                    {
                        from = width;
                        animElement.RenderTransform = new TranslateTransform(from, 0);
                    }
                    else if (AnimProperty == TranslateTransform.YProperty)
                    {
                        from = height;
                        animElement.RenderTransform = new TranslateTransform(0, from);
                    }
                }
                else
                {
                    if (AnimProperty == TranslateTransform.XProperty)
                    {
                        from = -animElement.ActualWidth;
                        animElement.RenderTransform = new TranslateTransform(from, 0);
                    }
                    else if (AnimProperty == TranslateTransform.YProperty)
                    {
                        from = -animElement.ActualHeight;
                        animElement.RenderTransform = new TranslateTransform(0, from);
                    }
                }


                animation.To = 0;
                var el = new ElasticEase();
                el.Oscillations = 0;

                animation.EasingFunction = el;


                animElement.RenderTransform.BeginAnimation(AnimProperty, animation);
            }
            catch (Exception exc) { Debug.WriteLine(exc.Message); }
        }

        public ISlideAnimation Invert()
        {
            var invertDir = Dir == AnimSlideDir.Back ? AnimSlideDir.Forw : AnimSlideDir.Back;

            return new SlideAnimationMove(invertDir, Length.TotalSeconds, AnimProperty);
        }

        public TimeSpan Length { get; }

        public DependencyProperty AnimProperty { get; }

        public AnimSlideDir Dir { get; private set; }


        public SlideAnimationMove(AnimSlideDir dir, double length, DependencyProperty property)
        {
            Dir = dir;
            AnimProperty = property;
            Length = TimeSpan.FromSeconds(length);
        }

        public SlideAnimationMove(AnimSlideDir dir, double length)
            :this(dir,length, TranslateTransform.XProperty) { }

        public SlideAnimationMove(AnimSlideDir dir)
            :this(dir, 0.3)
        {

        }
    }

    public enum AnimSlideDir
    {
        Back, Forw
    }

}
