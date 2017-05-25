using BiomasaEUPT.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Clases
{
    public sealed class BaseDeDatos
    {
        //public BiomasaEUPTDataSet biomasaEUPTDataSet;
        //public BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter biomasaEUPTDataSetusuariosTableAdapter;
        //public BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter biomasaEUPTDataSettipos_usuariosTableAdapter;
        public BiomasaEUPTContext biomasaEUPTContext;

        private BaseDeDatos()
        {
            //biomasaEUPTDataSet = new BiomasaEUPTDataSet();
            //biomasaEUPTDataSetusuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter();
            //biomasaEUPTDataSettipos_usuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter();
            biomasaEUPTContext = new BiomasaEUPTContext();
            //biomasaEUPTEntidades.Configuration.ProxyCreationEnabled = false;
        }

        private static BaseDeDatos _instancia;
        public static BaseDeDatos Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new BaseDeDatos();
                }
                return _instancia;
            }
        }


        public void a()
        {

        }

    }
}
