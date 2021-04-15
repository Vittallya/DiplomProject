using AirClub.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AirClub.Pages.MainScreen
{
    /// <summary>
    /// Логика взаимодействия для PageUserStart.xaml
    /// </summary>
    public partial class PageUserStart : Page
    {
        public PageUserStart()
        {
            InitializeComponent();

            AnimStack1 = new FrameworkElement[]
            {
                Caption,
            };
            AnimStack2 = new FrameworkElement[]
            {
                lastWork_caption,
            };
            AnimStack3 = new FrameworkElement[]
            {
                lastWork_btns,
            };
        }

        FrameworkElement[] AnimStack1;
        FrameworkElement[] AnimStack2;
        FrameworkElement[] AnimStack3;

        DoubleAnimation opacityAnim = new DoubleAnimation()
        {
            To = 1,
            Duration = TimeSpan.FromSeconds(0.5),
        };

        DoubleAnimation moveAnim = new DoubleAnimation(150, 0, TimeSpan.FromSeconds(0.7))
        {
            EasingFunction = new ElasticEase() 
            { 
                Oscillations = 0,
            },
        };

        public async void ExecuteAnimation()
        {
            var all = AnimStack1.Union(AnimStack2.Union(AnimStack3)).ToList();

            foreach(FrameworkElement el in all)
            {
                el.Opacity = 0;
                el.RenderTransform = new TranslateTransform(155, 0);
                el.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            foreach(FrameworkElement el in AnimStack1)
            {
                el.RenderTransform.BeginAnimation(TranslateTransform.XProperty, moveAnim);
                el.BeginAnimation(OpacityProperty, opacityAnim);
            }

            await Task.Delay(500);

            foreach (FrameworkElement el in AnimStack2)
            {
                el.RenderTransform.BeginAnimation(TranslateTransform.XProperty, moveAnim);
                el.BeginAnimation(OpacityProperty, opacityAnim);
            }

            await Task.Delay(250);

            foreach (FrameworkElement el in AnimStack3)
            {
                el.RenderTransform.BeginAnimation(TranslateTransform.XProperty, moveAnim);
                el.BeginAnimation(OpacityProperty, opacityAnim);
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is UserHomeViewModel vm)
            {
                vm.Loaded.Execute(null);
            }
        }
    }
}
