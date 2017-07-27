using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para AcercaDe.xaml
    /// </summary>
    public partial class AcercaDe : Window
    {
        public string Version { get; } = Assembly.GetEntryAssembly().GetName().Version.ToString();

        public AcercaDe()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void bAceptar_Click(object sender, RoutedEventArgs e)
        {
            var animacion = new DoubleAnimation();
            animacion.From = 1;
            animacion.To = 0;
            animacion.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            animacion.EasingFunction = new QuadraticEase();
            animacion.Completed += (_s, _e) =>
            {
                Close();
            };
            BeginAnimation(OpacityProperty, animacion);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var animacion = new DoubleAnimation();
            animacion.From = 0;
            animacion.To = 1;
            animacion.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            animacion.EasingFunction = new QuadraticEase();
            BeginAnimation(OpacityProperty, animacion);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("http://hjnilsson.github.io/country-flags");
        }
    }
}
