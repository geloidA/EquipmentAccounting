﻿using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for DeliveriesWindow.xaml
    /// </summary>
    public partial class DeliveriesWindow : Window
    {
        public ObservableCollection<Deliveries> Deliveries { get; set; }

        public DeliveriesWindow()
        {
            InitializeComponent();
            Deliveries = new ObservableCollection<Deliveries>(Entities.Context.Deliveries);
            dgDeliveries.ItemsSource = Deliveries;
            DataContext = this;
        }

        private void SearchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Deliveries.Clear();
            Deliveries.AddRange(GetDeliveriesContainedText(searchTxtBox.Text.ToLower()));
            if (!Deliveries.Any())
                MessageBox.Show("Нет результатов поиска");
        }

        private IEnumerable<Deliveries> GetDeliveriesContainedText(string text)
        {
            return Entities.Context.Deliveries
                .ToList()
                .Where(x => $"{x.Date}{x.Count}{x.Equipments.Name}{x.Suppliers.Name}".ToLower().Contains(text));
        }

        private void BtnAddNewDelivery_Click(object sender, RoutedEventArgs e)
        {
            var result = new DeliveryCreationWindow { Owner = this }.ShowDialog();
            if (result == true)
            {
                Deliveries.Clear();
                Deliveries.AddRange(Entities.Context.Deliveries);
                var mainWnd = Owner as MainWindow;
                mainWnd.UpdateEquipments();
            }
        }
    }
}
