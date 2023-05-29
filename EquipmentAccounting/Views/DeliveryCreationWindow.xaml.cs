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
    /// Interaction logic for DeliveryCreationWindow.xaml
    /// </summary>
    public partial class DeliveryCreationWindow : Window
    {
        public DeliveryCreationWindow()
        {
            InitializeComponent();
        }

        public Deliveries CreatedDelivery { get; private set; }
    }
}
