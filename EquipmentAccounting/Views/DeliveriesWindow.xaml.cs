using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private void btnCreateExcelReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { DefaultExt = "xlsx", Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" };
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == true)
            {
                GenerateDeliveryReport(Deliveries, dialog.FileName);
                MessageBox.Show("Отчет сохранен.");
            }
        }

        public void GenerateDeliveryReport(Collection<Deliveries> deliveries, string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Отчет по поставкам");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Дата";
                worksheet.Cells[1, 2].Value = "Оборудование";
                worksheet.Cells[1, 3].Value = "Кол-во, шт.";
                worksheet.Cells[1, 4].Value = "Поставщик";

                // Заполнение данных
                for (int i = 0; i < deliveries.Count; i++)
                {
                    var delivery = deliveries[i];
                    worksheet.Cells[i + 2, 1].Value = delivery.Date.ToString("dd MMM yyyy HH:mm");
                    worksheet.Cells[i + 2, 2].Value = delivery.Equipments.Name;
                    worksheet.Cells[i + 2, 3].Value = delivery.Count;
                    worksheet.Cells[i + 2, 4].Value = delivery.Suppliers.Name;
                }

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}
