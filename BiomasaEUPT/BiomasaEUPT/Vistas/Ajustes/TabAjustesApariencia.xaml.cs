using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.Ajustes
{
    /// <summary>
    /// Lógica de interacción para TabAjustesApariencia.xaml
    /// </summary>
    public partial class TabAjustesApariencia : UserControl
    {
        public IEnumerable<Swatch> Swatches { get; }

        public TabAjustesApariencia()
        {
            InitializeComponent();
            DataContext = this;
            Swatches = new SwatchesProvider().Swatches;
        }

        public ICommand ApplyPrimaryCommand { get; } = new RelayCommand(o => AplicarColorPrimario((Swatch)o));

        private static void AplicarColorPrimario(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
            Properties.Settings.Default.ColorPrimario = swatch.Name;
            Properties.Settings.Default.Save();
        }

        public ICommand ApplyAccentCommand { get; } = new RelayCommand(o => AplicarColorSecundario((Swatch)o));

        private static void AplicarColorSecundario(Swatch swatch)
        {
            new PaletteHelper().ReplaceAccentColor(swatch);
            Properties.Settings.Default.ColorSecundario = swatch.Name;
            Properties.Settings.Default.Save();
        }

    }
}
