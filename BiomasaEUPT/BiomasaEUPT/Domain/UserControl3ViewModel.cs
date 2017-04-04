using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    public class UserControl3ViewModel
    {
        private BiomasaEUPTEntities db = null;


        public UserControl3ViewModel()
        {
            db = new BiomasaEUPTEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
            CustomersCollection = new ObservableCollection<usuarios>(db.usuarios);
            // OrdersCollection = new ObservableCollection<Orders>();
            //  ProductCollection = new ObservableCollection<Order_Details>();
        }


        private ObservableCollection<usuarios> customersCollection;
        public ObservableCollection<usuarios> CustomersCollection
        {
            get
            {
                return customersCollection;
            }
            set
            {
                customersCollection = value;
                NotifyPropertyChanged();
            }
        }

        usuarios selectedCustomers = null;
        public usuarios SelectedCustomers
        {
            get { return selectedCustomers; }
            set
            {
                selectedCustomers = value;
                NotifyPropertyChanged();
                Debug.WriteLine(value);
               // GetOrders(selectedCustomers.id_usuario);
               // ProductCollection.Clear();
            }
        }

        /*private ObservableCollection<Orders> ordersCollection;
        public ObservableCollection<Orders> OrdersCollection
        {
            get
            {
                return ordersCollection;
            }
            set
            {
                ordersCollection = value;
                NotifyPropertyChanged();
            }
        }

        Orders selectedOrder = null;
        public Orders SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                NotifyPropertyChanged();
                if (selectedOrder != null)
                {
                    GetOrderDetails(selectedOrder.OrderID);
                }
            }
        }

        private ObservableCollection<Order_Details> productCollection;
        public ObservableCollection<Order_Details> ProductCollection
        {
            get
            {
                return productCollection;
            }
            set
            {
                productCollection = value;
                NotifyPropertyChanged();
            }
        }*/


        /* private void GetOrders(object CustomerID)
         {
             var query = (from order in db.Orders
                          where order.CustomerID == CustomerID
                          select order).ToList();

             OrdersCollection.Clear();
             foreach (Orders order in query)
             {
                 OrdersCollection.Add(order);
             }
         }

         private void GetOrderDetails(object OrderID)
         {
             int orderID = int.Parse(OrderID.ToString());
             var query = (from orderDetails in db.Order_Details
                          where orderDetails.OrderID == orderID
                          select orderDetails).ToList();

             ProductCollection.Clear();

             foreach (Order_Details order in query)
             {
                 ProductCollection.Add(order);
             }
         }*/



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
