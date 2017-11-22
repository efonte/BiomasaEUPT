using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    public class TabTrazabilidadViewModel : ViewModelBase
    {
        public ObservableCollection<Proveedor> Arbol { get; set; } = new ObservableCollection<Proveedor>();
        private string _codigo = "";
        public string Codigo
        {
            get { return _codigo; }
            set
            {
                _codigo = value;
                GenerarArbol();
            }
        }

        public bool TrazabilidadCliente { get; set; }

        public bool MostrarGenerarPDF { get; set; } = false;
        public string TextoTrazabilidad { get; set; } = "Trazabilidad";

        private object _itemArbolSeleccionado;
        public object ItemArbolSeleccionado
        {
            get
            { return _itemArbolSeleccionado; }
            set
            {
                _itemArbolSeleccionado = value;
            }
        }

        private ICommand _generarPDFComando;

        private BiomasaEUPTContext context;
        private Trazabilidad trazabilidad;

        public TabTrazabilidadViewModel()
        {

        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            context.Configuration.LazyLoadingEnabled = false;
            trazabilidad = new Trazabilidad();
        }

        private void GenerarArbol()
        {
            Arbol.Clear();
            MostrarGenerarPDF = false;
            TextoTrazabilidad = "Trazabilidad";

            if (context.Recepciones.Any(r => r.NumeroAlbaran == Codigo))
            {
                MostrarGenerarPDF = true;
                TextoTrazabilidad = "Trazabilidad Recepción";
                Arbol.Add(trazabilidad.Recepcion(Codigo));
            }
            else if (Codigo.Length == 10)
            {
                switch (Codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == Codigo))
                        {
                            MostrarGenerarPDF = true;
                            TextoTrazabilidad = "Trazabilidad Materia Prima";
                            Arbol.Add(trazabilidad.MateriaPrima(Codigo));
                        }
                        break;


                    case Constantes.CODIGO_ELABORACIONES:
                        if (context.ProductosTerminados.Any(pt => pt.Codigo == Codigo))
                        {
                            MostrarGenerarPDF = true;
                            TextoTrazabilidad = "Trazabilidad Producto Terminado";
                            Arbol = new ObservableCollection<Proveedor>(trazabilidad.ProductoTerminado(Codigo));
                        }
                        break;


                    case Constantes.CODIGO_VENTAS:
                        if (context.ProductosEnvasados.Any(mp => mp.Codigo == Codigo))
                        {
                            MostrarGenerarPDF = true;
                            TextoTrazabilidad = "Trazabilidad Producto Envasado";
                            Arbol = new ObservableCollection<Proveedor>(trazabilidad.ProductoEnvasado(Codigo));
                            //var proveedores = trazabilidad.ProductoTerminado(codigo);
                            //proveedores.ForEach(ucTrazabilidadCodigos.ArbolRecepcion.Add);
                        }
                        break;
                }
            }
        }

        #region Generar PDF
        public ICommand GenerarPDFComando => _generarPDFComando ??
            (_generarPDFComando = new RelayCommand(
                param => GenerarPDF()
            ));

        private void GenerarPDF()
        {
            var informe = new InformePDF(Properties.Settings.Default.DirectorioInformes);
            var rutaInforme = "";
            if (context.Recepciones.Any(r => r.NumeroAlbaran == Codigo))
            {
                rutaInforme = informe.GenerarInformeRecepcion(trazabilidad.Recepcion(Codigo));
            }
            else
            {
                switch (Codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        rutaInforme = informe.GenerarInformeMateriaPrima(trazabilidad.MateriaPrima(Codigo));
                        break;

                    case Constantes.CODIGO_ELABORACIONES:
                        rutaInforme = informe.GenerarInformeProductoTerminado(trazabilidad.ProductoTerminado(Codigo));
                        break;

                    case Constantes.CODIGO_VENTAS:
                        rutaInforme = informe.GenerarInformeProductoEnvasado(trazabilidad.ProductoEnvasado(Codigo));
                        break;
                }
            }

            System.Diagnostics.Process.Start(rutaInforme);
        }
        #endregion
    }
}
