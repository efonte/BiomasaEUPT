using BiomasaEUPT.Modelos.Tablas;
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

namespace BiomasaEUPT.Vistas.GestionPermisos
{
    /// <summary>
    /// Lógica de interacción para FormTipoUsuario.xaml
    /// </summary>
    public partial class FormTipoUsuario : UserControl
    {
        public FormTipoUsuario()
        {
            InitializeComponent();
        }

        private void lbTabs_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var dato = GetDataFromListBox(lbTabs, e.GetPosition(parent));
            if (dato != null)
            {
                var tab = (Tab)dato;
                DataObject dragData = new DataObject("Tab", tab);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }

        private object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }
    }
}
