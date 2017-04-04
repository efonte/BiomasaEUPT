using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    class PruebaViewSource
    {
        public ObservableCollection<tipos_usuarios> TiposUsuarios { get; private set; }
       // public ObservableCollection<usuarios> Usuarios { get; private set; }
        public PruebaViewSource()
        {
            TiposUsuarios = new ObservableCollection<tipos_usuarios>();
            BiomasaEUPTEntities ctx = new BiomasaEUPTEntities();
            foreach (var tipoUsuario in ctx.tipos_usuarios)
            {
                TiposUsuarios.Add(tipoUsuario);
            }
          /*  Usuarios = new ObservableCollection<usuarios>();
            foreach (var usuario in ctx.usuarios)
            {
                Usuarios.Add(usuario);
            }*/
        }
    }
}
