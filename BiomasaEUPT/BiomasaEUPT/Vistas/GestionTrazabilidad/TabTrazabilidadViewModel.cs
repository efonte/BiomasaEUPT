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
                if (_itemArbolSeleccionado != null)
                    Console.WriteLine(_itemArbolSeleccionado.GetType());
            }
        }

        /*private bool _isSelectedItemArbol;
        public bool IsSelectedItemArbol
        {
            get { return _isSelectedItemArbol; }
            set
            {
                _isSelectedItemArbol = value;
                if (_isSelectedItemArbol)
                {
                    ItemArbolSeleccionado = this;
                }
            }
        }*/

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

                        /*  var consulta = context.MateriasPrimas
                                         .Join(context.Recepciones,
                                            mp => mp.RecepcionId,
                                            r => r.RecepcionId,
                                            (mp, r) => new { mp, r })
                                         .Join(context.Proveedores,
                                            r1 => r1.r.ProveedorId,
                                            p => p.ProveedorId,
                                            (r1, p) => new { r1.mp, r1.r, p })
                                         .Join(context.HistorialHuecosRecepciones,
                                            p1 => p1.mp.MateriaPrimaId,
                                            hhr => hhr.MateriaPrimaId,
                                            (p1, hhr) => new { p1.mp, p1.r, p1.p, hhr })
                                         .Join(context.HuecosRecepciones,
                                            hhr1 => hhr1.hhr.HuecoRecepcionId,
                                            hr => hr.HuecoRecepcionId,
                                            (hhr1, hr) => new { hhr1.mp, hhr1.r, hhr1.p, hhr1.hhr, hr })
                                         .Join(context.SitiosRecepciones,
                                            hr1 => hr1.hr.SitioId,
                                            sr => sr.SitioRecepcionId,
                                            (hr1, sr) => new { hr1.mp, hr1.p, hr1.r, hr1.hhr, hr1.hr, sr }).Distinct().ToList();*/
                        break;


                    case Constantes.CODIGO_ELABORACIONES:
                        if (context.ProductosTerminados.Any(pt => pt.Codigo == Codigo))
                        {
                            MostrarGenerarPDF = true;
                            TextoTrazabilidad = "Trazabilidad Órden de Elaboración";
                            Arbol = new ObservableCollection<Proveedor>(trazabilidad.ProductoTerminado(Codigo));
                        }
                        break;


                    case Constantes.CODIGO_VENTAS:
                        if (context.ProductosEnvasados.Any(mp => mp.Codigo == Codigo))
                        {
                            MostrarGenerarPDF = true;
                            TextoTrazabilidad = "Trazabilidad Producto Envasado";
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
            var informe = new InformePDF(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Informes\");
            var rutaInforme = "";
            if (context.Recepciones.Any(r => r.NumeroAlbaran == Codigo))
            {
                rutaInforme = informe.GenerarPDFRecepcion(trazabilidad.Recepcion(Codigo));
            }
            else
            {
                switch (Codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        rutaInforme = informe.GenerarPDFMateriaPrima(trazabilidad.MateriaPrima(Codigo));
                        break;

                    case Constantes.CODIGO_ELABORACIONES:
                        //rutaInforme = informe.GenerarPDFProductoTerminado(trazabilidad.ProductoTerminado(codigo));
                        break;

                    case Constantes.CODIGO_VENTAS:
                        //rutaInforme = informe.GenerarPDFProductoEnvasado(trazabilidad.ProductoEnvasado(codigo));
                        break;
                }
            }

            System.Diagnostics.Process.Start(rutaInforme);
        }
        #endregion
    }
}
