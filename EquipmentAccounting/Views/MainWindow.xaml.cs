using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Users User { get; set; }

        public ObservableCollection<Equipments> Equipments { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Equipments = new ObservableCollection<Equipments>(Entities.Context.Equipments);
            dgEquipments.ItemsSource = Equipments;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.Show();
        }

        private void BtnShowDeliveries_Click(object sender, RoutedEventArgs e) => new DeliveriesWindow().ShowDialog();

        private void BtnShowDistributions_Click(object sender, RoutedEventArgs e) => new DistributionsWindow { User = User }.ShowDialog();

        private void SearchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Equipments.Clear();
            Equipments.AddRange(GetEquipmentsContainedText(searchTxtBox.Text.ToLower()));
            if (!Equipments.Any())
                MessageBox.Show("Нет результатов поиска");
        }

        private IEnumerable<Equipments> GetEquipmentsContainedText(string text)
        {
            return Entities.Context.Equipments
                .ToList()
                .Where(x => $"{x.Name}{x.DeliveryDate}{x.Type}{x.CountInStock}".ToLower().Contains(text));
        }
    }
}
