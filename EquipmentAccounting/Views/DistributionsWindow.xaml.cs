using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace EquipmentAccounting.Views
{
    /// <summary>
    /// Interaction logic for DistributionsWindow.xaml
    /// </summary>
    public partial class DistributionsWindow : Window
    {
        public ObservableCollection<Distributions> Distributions { get; set; }
        public string SearchText { get; set; }

        public DistributionsWindow()
        {
            InitializeComponent();
            Distributions = new ObservableCollection<Distributions>(Entities.Context.Distributions);
            dgDistributions.ItemsSource = Distributions;
            DataContext = this;
        }

        public Users User { get; internal set; }

        private void BtnAddNewDistribution_Click(object sender, RoutedEventArgs e)
        {
            var res = new DestributionCreationWindow { User = User }.ShowDialog();
            if (res == true)
            {
                Distributions.Clear();
                Distributions.AddRange(Entities.Context.Distributions);
                (Owner as MainWindow).UpdateEquipments();
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Distributions.Clear();
            Distributions.AddRange(GetDistributionsContainedText(SearchText?.ToLower() ?? string.Empty));
        }

        private IEnumerable<Distributions> GetDistributionsContainedText(string text)
        {
            return Entities.Context.Distributions
                .ToList()
                .Where(x => $"{x.Users.FullName}{x.Date}{x.Description}".ToLower().Contains(text));
        }
    }
}
