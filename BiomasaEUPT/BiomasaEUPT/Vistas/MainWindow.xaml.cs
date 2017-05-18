﻿using BiomasaEUPT.Vistas.GestionClientes;
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

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // BiomasaEUPTDataSet biomasaEUPTDataSet;
        // BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter biomasaEUPTDataSetusuariosTableAdapter;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // biomasaEUPTDataSet = ((BiomasaEUPTDataSet)(FindResource("biomasaEUPTDataSet")));
            // biomasaEUPTDataSet = new BiomasaEUPTDataSet();
            // biomasaEUPTDataSetusuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter();
            // biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
        }

        private void menuSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void menuAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            AcercaDe acercaDe = new AcercaDe();
            acercaDe.Owner = GetWindow(this);
            acercaDe.ShowDialog();
        }

        private void menuGitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/F0NT3/BiomasaEUPT");
        }

        private void menuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.contrasena = "";
            Properties.Settings.Default.Save();
            Login login = new Login();
            login.Show();
            login.tbUsuario.Text = Properties.Settings.Default.usuario;
            Close();
        }


        private void tiUsuarios_Unselected(object sender, RoutedEventArgs e)
        {
            // Cancela la inserción de una nueva fila (si no se ha completado)
            if (ucTabUsuarios != null)
                ucTabUsuarios.ucTablaUsuarios.dgUsuarios.CancelEdit();

        }

        private void tiClientes_Unselected(object sender, RoutedEventArgs e)
        {
            if (ucTabClientes != null)
                ucTabClientes.ucTablaClientes.dgClientes.CancelEdit();

        }


    }
}