using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para TabRecepciones.xaml
    /// </summary>
    public partial class TabRecepciones : UserControl
    {
        private TabRecepcionesViewModel viewModel;
        public TabRecepciones()
        {
            InitializeComponent();
            viewModel = new TabRecepcionesViewModel();
            DataContext = viewModel;

            // Filtro datagrid recepciones
            ucTablaRecepciones.cbNumeroAlbaran.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbNumeroAlbaran.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbFechaRecepcion.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbFechaRecepcion.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbEstado.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbEstado.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbProveedor.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaRecepciones.cbProveedor.Unchecked += (s, e1) => { FiltrarTablaRecepciones(); };

            // Filtro datagrid materias primas
            ucTablaMateriasPrimas.cbTipo.Checked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbTipo.Unchecked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbGrupo.Checked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbGrupo.Unchecked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbVolUni.Checked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbVolUni.Unchecked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbProcedencia.Checked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbProcedencia.Unchecked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbFechaBaja.Checked += (s, e1) => { FiltrarTablaMateriasPrimas(); };
            ucTablaMateriasPrimas.cbFechaBaja.Unchecked += (s, e1) => { FiltrarTablaMateriasPrimas(); };

            // Hacer doble clic en una fila del datagrid de recepcions hará que se ejecuta el evento RowRecepciones_DoubleClick
            Style rowStyleRecepciones = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleRecepciones.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarRecepcion(); })));
            ucTablaRecepciones.dgRecepciones.RowStyle = rowStyleRecepciones;
            // Hacer doble clic en una fila del datagrid de materias primas hará que se ejecuta el evento RowMateriasPrimas_DoubleClick
            Style rowStyleMateriasPrimas = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleMateriasPrimas.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarMateriaPrima(); })));
            ucTablaMateriasPrimas.dgMateriasPrimas.RowStyle = rowStyleMateriasPrimas;

            (ucTablaRecepciones.ucPaginacion.DataContext as PaginacionViewSource).ParentUC = this;
            (ucTablaRecepciones.ucPaginacion.DataContext as PaginacionViewSource).CalcularItemsTotales();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void FiltrarTablaRecepciones()
        {
            //  recepcionesViewSource.Filter += new FilterEventHandler(FiltroTablaRecepciones);
        }

        private void FiltroTablaRecepciones(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaRecepciones.tbBuscar.Text.ToLower();
            var recepcion = e.Item as Recepcion;
            string fechaRecepcion = recepcion.FechaRecepcion.ToString();
            string numeroAlbaran = recepcion.NumeroAlbaran.ToLower();
            string proveedor = recepcion.Proveedor.RazonSocial.ToLower();
            string estado = recepcion.EstadoRecepcion.Nombre.ToLower();

            e.Accepted = (ucTablaRecepciones.cbFechaRecepcion.IsChecked == true ? fechaRecepcion.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbNumeroAlbaran.IsChecked == true ? numeroAlbaran.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbProveedor.IsChecked == true ? proveedor.Contains(textoBuscado) : false) ||
                         (ucTablaRecepciones.cbEstado.IsChecked == true ? estado.Contains(textoBuscado) : false);
        }

        public void FiltrarTablaMateriasPrimas()
        {
            // materiasPrimasViewSource.Filter += new FilterEventHandler(FiltroTablaMateriasPrimas);
        }

        private void FiltroTablaMateriasPrimas(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaMateriasPrimas.tbBuscar.Text.ToLower();
            var materiaPrima = e.Item as MateriaPrima;
            string tipo = materiaPrima.TipoMateriaPrima.Nombre.ToLower();
            string grupo = materiaPrima.TipoMateriaPrima.GrupoMateriaPrima.Nombre.ToLower();
            string volumen = materiaPrima.Volumen.ToString();
            string unidades = materiaPrima.Unidades.ToString();
            string procedencia = materiaPrima.Procedencia.Nombre.ToLower();
            string fechaBaja = materiaPrima.FechaBaja.ToString();

            e.Accepted = (ucTablaMateriasPrimas.cbFechaBaja.IsChecked == true ? fechaBaja.Contains(textoBuscado) : false) ||
                         (ucTablaMateriasPrimas.cbTipo.IsChecked == true ? tipo.Contains(textoBuscado) : false) ||
                         (ucTablaMateriasPrimas.cbGrupo.IsChecked == true ? grupo.Contains(textoBuscado) : false) ||
                         (ucTablaMateriasPrimas.cbVolUni.IsChecked == true ? (volumen.Contains(textoBuscado) || unidades.Contains(textoBuscado)) : false) ||
                         (ucTablaMateriasPrimas.cbProcedencia.IsChecked == true ? procedencia.Contains(textoBuscado) : false);
        }
    }
}
