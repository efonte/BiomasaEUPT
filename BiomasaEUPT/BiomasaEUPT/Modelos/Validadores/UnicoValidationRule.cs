using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class UnicoValidationRule : ValidationRule
    {
        public CollectionViewSource CurrentCollection { get; set; }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {

                //ObservableCollection<usuarios> castedCollection = (CollectionViewSource)CurrentCollection.Source;
                // ObservableCollection<usuarios> castedCollection = CurrentCollection.Source as ObservableCollection<usuarios>;
                //Console.WriteLine(CurrentCollection.View==null);
             //    usuarios curValue = (usuarios)((BindingExpression)value).DataItem;

               // Console.WriteLine(CurrentCollection.View.IsEmpty);
               foreach (usuarios swVM in CurrentCollection.View)
               {
                   // usuarios u = swVM as usuarios;
                     Console.WriteLine(swVM);
                  //    if (curValue.GetHashCode() != swVM.GetHashCode() && swVM.nombre == curValue.nombre.ToString())
                   /* if (swVM.nombre == value.ToString())
                    {
                        return new ValidationResult(false, "AAAAAAAAAAAA");
                    }*/
               }
            }

            return  ValidationResult.ValidResult;

        }
    }
}
