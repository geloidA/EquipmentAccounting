using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
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

                // Установка значения ячейки для заголовка
                worksheet.Cells["A1"].Value = "Отчет";

                // Применение форматирования для заголовка
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                worksheet.Cells["B1"].Value = $"Дата формирования отчета: {DateTime.Now:dd.MM.yyyy}";
                worksheet.Cells["B1"].Style.Font.Bold = true;
                worksheet.Cells["B1"].Style.Font.Italic = true;
                worksheet.Cells["B1"].Style.Font.Size = 10;
                worksheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Заголовки столбцов
                worksheet.Cells[2, 1].Value = "Дата";
                worksheet.Cells[2, 2].Value = "Оборудование";
                worksheet.Cells[2, 3].Value = "Кол-во, шт.";
                worksheet.Cells[2, 4].Value = "Поставщик";
                int i = 0;
                // Заполнение данных
                for (; i < deliveries.Count; i++)
                {
                    var delivery = deliveries[i];
                    worksheet.Cells[i + 3, 1].Value = delivery.Date.ToString("dd MMM yyyy HH:mm");
                    worksheet.Cells[i + 3, 2].Value = delivery.Equipments.Name;
                    worksheet.Cells[i + 3, 3].Value = delivery.Count;
                    worksheet.Cells[i + 3, 4].Value = delivery.Suppliers.Name;
                }

                worksheet.Cells[i + 3, 1].Value = "Подпись:";

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение в файл
                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }
        }
    }
}
