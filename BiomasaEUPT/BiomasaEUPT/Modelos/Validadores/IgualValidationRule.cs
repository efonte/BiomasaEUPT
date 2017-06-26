using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace BiomasaEUPT.Modelos.Validadores
{

    /// <summary>
    /// AUN NO FUNCIONALLLLLLLLLLLLLLLL
    /// https://stackoverflow.com/questions/3862385/wpf-validationrule-with-dependency-property
    /// http://dedjo.blogspot.com/2007/05/fully-binded-validation-by-using.html
    /// </summary>
    public class IgualValidationRule : ValidationRule
    {
        private string _nombreCampo;
        public string NombreCampo
        {
            get { return _nombreCampo; }
            set { _nombreCampo = value; }
        }

        private string _nombreCampoAComparar;
        public string NombreCampoAComparar
        {
            get { return _nombreCampoAComparar; }
            set { _nombreCampoAComparar = value; }
        }

        private CadenaACompararDependencyObject _cadenaAComparar;
        public CadenaACompararDependencyObject CadenaAComparar
        {
            get { return _cadenaAComparar; }
            set { _cadenaAComparar = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Console.WriteLine(CadenaAComparar.CadenaAComparar);
            if ((string)value == CadenaAComparar.CadenaAComparar)
            {
                return new ValidationResult(false, String.Format("El campo{0} y el campo{1} no son iguales.", " " + NombreCampo, " " + NombreCampoAComparar));
            }

            return ValidationResult.ValidResult;
        }
    }

    public class CadenaACompararDependencyObject : DependencyObject
    {
        public string CadenaAComparar
        {
            get { return (string)GetValue(CadenaACompararProperty); }
            set { SetValue(CadenaACompararProperty, value); }
        }
        public static readonly DependencyProperty CadenaACompararProperty =
            DependencyProperty.Register("CadenaAComparar", typeof(string), typeof(CadenaACompararDependencyObject), new UIPropertyMetadata(default(string)));

    }


    [ContentProperty("ComparisonValue")]
    public class GreaterThanValidationRule : ValidationRule
    {
        public ComparisonValue ComparisonValue { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string s = value?.ToString();
            int number;

            if (!Int32.TryParse(s, out number))
            {
                return new ValidationResult(false, "Not a valid entry");
            }

            if (number <= ComparisonValue.Value)
            {
                return new ValidationResult(false, $"Number should be greater than {ComparisonValue}");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class ComparisonValue : DependencyObject
    {
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(int),
            typeof(ComparisonValue),
            new PropertyMetadata(default(int), OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComparisonValue comparisonValue = (ComparisonValue)d;
            BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(comparisonValue, BindingToTriggerProperty);
            bindingExpressionBase?.UpdateSource();
        }

        public object BindingToTrigger
        {
            get { return GetValue(BindingToTriggerProperty); }
            set { SetValue(BindingToTriggerProperty, value); }
        }
        public static readonly DependencyProperty BindingToTriggerProperty = DependencyProperty.Register(
            nameof(BindingToTrigger),
            typeof(object),
            typeof(ComparisonValue),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
