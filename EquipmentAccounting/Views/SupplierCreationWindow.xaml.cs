using EquipmentAccounting.DataBase;
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
using System.Windows.Shapes;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for SupplierCreationWindow.xaml
    /// </summary>
    public partial class SupplierCreationWindow : Window
    {
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContact { get; set; }

        public SupplierCreationWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SupplierContact) && string.IsNullOrEmpty(SupplierName) && string.IsNullOrEmpty(SupplierAddress))
            {
                MessageBox.Show("Некорректные данные");
                return;
            }
            DialogResult = true;
            Entities.Context.Suppliers.Add(new Suppliers
            {
                Address = SupplierAddress,
                Name = SupplierName,
                Contact = SupplierContact
            });
            Entities.Context.SaveChanges();
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
