using System;
using System.Text.RegularExpressions;

namespace BiomasaEUPT
{


    partial class BiomasaEUPTDataSet
    {
        partial class usuariosDataTable
        {
            private readonly Regex nombreEx = new Regex(@"^[A-Za-z]+$");
            /*
            private string nombre;

            public string Nombre
            {
                get { return nombre; }
                set
                {
                    if (value == null)
                        throw new ArgumentException("Name cannot be null");

                    if (!nameEx.Match(value).Success)
                        throw new ArgumentException("Name may only " +
                                  "contain characters or spaces");

                    nombre = value;
                }
            }

             private int email;

             public int Email
             {
                 get { return email; }
                 set
                 {
                     if (value < 0 || value > 110)
                         throw new ArgumentException("Age must be positive " +
                                                     "and less than 110");

                    email = value;
                 }
             }*/

            public override void EndInit()
            {
                base.EndInit();
                // Hook up the ColumnChanging event  
                // to call the SampleColumnChangingEvent method.  
                //ColumnChanging += SampleColumnChangingEvent;
                //usuariosRowChanging += RowChangingEvent;
            }

            /*  public void SampleColumnChangingEvent(object sender, System.Data.DataColumnChangeEventArgs e)
              {
                  if (e.Column.ColumnName == nombreColumn.ColumnName)
                  {
                      string valorColumna = (string)e.ProposedValue;
                      if (valorColumna == null)
                      {
                          e.Row.SetColumnError("nombre", "El nombre no puede ser null");
                      }
                      else if (!nombreEx.Match(valorColumna).Success)
                      {
                         // throw new ArgumentException("El nombre sólo puede contener caracteres");
                          e.Row.SetColumnError("nombre", "El nombre sólo puede contener caracteres");
                      }
                  }
              }*/

            public void RowChangingEvent(object sender, usuariosRowChangeEvent e)
            {
                // Perfom the validation logic.  
                if (!nombreEx.Match(e.Row.nombre).Success)
                {
                    //e.Row.RowError = "El nombre sólo puede contener caracteres";
                    e.Row.SetColumnError("nombre", "El nombre sólo puede contener caracteres");
                    Console.WriteLine(e.Row.RowError);
                }
                else
                {
                    // Clear the RowError if validation passes.  
                    //e.Row.RowError = "";
                    e.Row.SetColumnError("nombre", "");
                }
            }
        }
    }
}
