using BiomasaEUPT.Modelos;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionUsuarios;
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

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para Contador.xaml
    /// </summary>
    public partial class Contador : UserControl
    {
        public Contador()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Actualizar();
        }

        public void Actualizar()
        {
            DependencyObject ucParent = Parent;
            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            using (var context = new BiomasaEUPTContext())
            {
                gContador.Children.Clear();
                gContador.RowDefinitions.Clear();

                // Pestaña Usuarios
                if (ucParent.GetType().Equals(typeof(TabUsuarios)))
                {
                    var tiposUsuarios = context.TiposUsuarios.ToList();
                    int fila = 0;
                    foreach (var tu in tiposUsuarios)
                    {
                        gContador.RowDefinitions.Add(new RowDefinition() { });
                        var nombre = new TextBlock() { Text = tu.Nombre };
                        Grid.SetRow(nombre, fila);
                        Grid.SetColumn(nombre, 0);
                        gContador.Children.Add(nombre);
                        var cantidad = new TextBlock() { Text = tu.Usuarios.Count().ToString(), FontWeight = FontWeights.Light };
                        Grid.SetRow(cantidad, fila);
                        Grid.SetColumn(cantidad, 1);
                        gContador.Children.Add(cantidad);
                        fila++;
                    }
                }

                // Pestaña Clientes
                else if (ucParent.GetType().Equals(typeof(TabClientes)))
                {
                    var tiposClientes = context.TiposClientes.ToList();
                    int fila = 0;
                    foreach (var tu in tiposClientes)
                    {
                        gContador.RowDefinitions.Add(new RowDefinition() { });
                        var nombre = new TextBlock() { Text = tu.Nombre };
                        Grid.SetRow(nombre, fila);
                        Grid.SetColumn(nombre, 0);
                        gContador.Children.Add(nombre);
                        var cantidad = new TextBlock() { Text = tu.Clientes.Count().ToString(), FontWeight = FontWeights.Light };
                        Grid.SetRow(cantidad, fila);
                        Grid.SetColumn(cantidad, 1);
                        gContador.Children.Add(cantidad);
                        fila++;
                    }
                }
            }
        }


    }
}
