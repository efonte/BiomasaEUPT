
using BiomasaEUPT.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        BiomasaEUPTEntities context = new BiomasaEUPTEntities();
        private IRepositorioGenerico<usuarios> repositorio = null;
        // private IRepositorioGenerico<tipos_usuarios> repositorioTiposUsuarios = null;
        public UserControl2()
        {
            InitializeComponent();
            //var context = new BiomasaEUPTEntities();
            /*
             var context = new BiomasaEUPTEntities();
             BindingSource bi = new BindingSource();
             bi.DataSource = context.usuarios;
             dgUsuarios.DataSource = bi;
             dgUsuarios.Refresh();*/
            repositorio = new RepositorioGenerico<usuarios>();
            //UsuariosColeccion = new ObservableCollection<usuarios>(repositorio.SelectAll());
            //repositorioTiposUsuarios = new RepositorioGenerico<tipos_usuarios>();
            //  TiposUsuariosColeccion = new ObservableCollection<tipos_usuarios>(repositorioTiposUsuarios.SelectAll());
            //TiposUsuariosColeccion = new ObservableCollection<tipos_usuarios>();
            //foreach (var tipoUsuario in context.tipos_usuarios)
            //{
            //    TiposUsuariosColeccion.Add(tipoUsuario);
            //}
            DataContext = new UserControl2ViewModel();
            var context = new BiomasaEUPTEntities();
            var a = new usuarios()
            {
                nombre = "asd",
                contrasena = "asd",
                tipo_id = 1,
                email = "asd@asd.asd"
            };
            context.usuarios.Add(a);
            context.SaveChanges();
        }

        private void EmployeeDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            /*usuarios usuario = new usuarios();
            usuarios u = e.Row.DataContext as usuarios;

            if (u != null)
            {
                if (u.id_usuario > 0)
                {
                    isInsert = false;
                }
                else
                {
                    isInsert = true;
                }
            }

            if (isInsert)
            {
                var InsertRecord = MessageBox.Show("¿Quiere añadir al usuario '" + u.nombre + "'?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (InsertRecord == MessageBoxResult.Yes)
                {
                    usuario.nombre = u.nombre;
                    usuario.email = u.email;
                    usuario.tipo_id = u.tipo_id;
                    usuario.fecha_alta = u.fecha_alta;                   
                    context.usuarios.InsertOnSubmit(usuario);
                    context.SubmitChanges();
                    dgUsuarios.ItemsSource = obtenerUsuarios();
                    MessageBox.Show("El usuario '"+usuario.nombre + "' ha sido añadido correctamente.", "Añadido nuevo usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    dgUsuarios.ItemsSource = obtenerUsuarios();
            }
            context.SubmitChanges();*/
        }

        /* private void InsertHandler(object sender, RoutedEventArgs e)
         {
             usuarios nuevoUsuario = new usuarios()
             {
                 nombre = "efwfwfwf",
                 email = "ffefefef@ffefe.com",
                 contrasena = "a",
                 tipo_id = 1
             };
             repositorio.Insert(nuevoUsuario);
             repositorio.Save();
             UpdateCollection();
         }

         private void DeleteHandler(object sender, RoutedEventArgs e)
         {
             repositorio.Delete(UsuarioSeleccionado.id_usuario);
             repositorio.Save();
             UpdateCollection();
         }

         private void UpdateHandler(object sender, RoutedEventArgs e)
         {
             UsuarioSeleccionado.nombre = UsuarioSeleccionado.nombre + "_Updated";
             UsuarioSeleccionado.email = UsuarioSeleccionado.email + "_Updated";
             repositorio.Update(UsuarioSeleccionado);
             repositorio.Save();
             UpdateCollection();
         }

         private void ReadHandler(object sender, RoutedEventArgs e)
         {
             string usuario = UsuarioSeleccionado.nombre + Environment.NewLine +
                 UsuarioSeleccionado.email + Environment.NewLine +
                 UsuarioSeleccionado.tipo_id + Environment.NewLine;

             MessageBox.Show(usuario);
         }



         private void UpdateCollection()
         {
             UsuariosColeccion.Clear();
             foreach (usuarios usuario in repositorio.SelectAll())
             {
                 UsuariosColeccion.Add(usuario);
             }
         }*/

        private void dgUsuarios_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                usuarios usuario = e.Row.DataContext as usuarios;
                var usuarioExistente = context.usuarios.Where(u => u.id_usuario == usuario.id_usuario).FirstOrDefault();
                if (usuarioExistente == null)
                {
                    usuarios nuevoUsuario = new usuarios();
                    nuevoUsuario.nombre = usuario.nombre;
                    nuevoUsuario.email = usuario.email;
                    nuevoUsuario.tipo_id = usuario.tipo_id;
                    nuevoUsuario.contrasena = usuario.contrasena = "a";

                    context.usuarios.Add(nuevoUsuario);
                    context.SaveChanges();

                    Debug.WriteLine("---");
                }
                else
                {
                    usuarioExistente.nombre = usuario.nombre;
                    usuarioExistente.email = usuario.email;
                    usuarioExistente.tipo_id = usuario.tipo_id;
                    context.SaveChanges();
                    Debug.WriteLine("+++");
                }

            }
        }

        private void dgUsuarios_PreviewExecuted(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            usuarios usuario = dgUsuarios.SelectedItem as usuarios;

            if (usuario != null)
            {
                var matchedUsuario = (from u in context.usuarios
                                      where u.id_usuario == usuario.id_usuario
                                      select u).SingleOrDefault();
                if (e.Command == DataGrid.DeleteCommand)
                {
                    if (!(MessageBox.Show("¿Está seguro de querer borrar el usuario?",
                        "Confirmar borrado", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        context.usuarios.Remove(matchedUsuario);
                        //context.SaveChanges();
                    }
                }
            }

        }

        private void bBorrarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;
            foreach (DataRowView row in dgUsuarios.ItemsSource)
            {
                bool isSelected = Convert.ToBoolean(row[0]);
                if (isSelected)
                {
                    message += Environment.NewLine;
                    message += row["nombre"].ToString();
                }
            }
            MessageBox.Show("Selected Values" + message);
        }

    }

}
