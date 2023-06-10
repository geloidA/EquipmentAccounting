using EquipmentAccounting.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ReturnEquipmentFromUnitWindow.xaml
    /// </summary>
    public partial class ReturnEquipmentFromUnitWindow : Window
    {
        public Locations SelectedUnitNumber { get; set; }
        public ObservableCollection<Locations> UnitNumbers { get; set; }

        public ReturnEquipmentFromUnitWindow()
        {
            InitializeComponent();
            UnitNumbers = new ObservableCollection<Locations>(Entities.Context.Locations);            
            DataContext= this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedUnitNumber = UnitNumbers.FirstOrDefault();
        }

        private void ComboBoxUnitNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
