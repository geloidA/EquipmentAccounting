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

        private void btnCreateExcelReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { DefaultExt = "xlsx", Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" };
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == true)
            {
                GenerateDeliveryReport(Distributions, dialog.FileName);
                MessageBox.Show("Отчет сохранен.");
            }
        }

        public void GenerateDeliveryReport(Collection<Distributions> distributions, string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Отчет по распределениям");

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
                worksheet.Cells[2, 4].Value = "Инициатор";
                worksheet.Cells[2, 5].Value = "Описание";

                var i = 0;
                // Заполнение данных
                for (; i < distributions.Count; i++)
                {
                    var distribute = distributions[i];
                    worksheet.Cells[i + 3, 1].Value = distribute.Date.ToString("dd MMM yyyy HH:mm");
                    worksheet.Cells[i + 3, 2].Value = distribute.Equipments.Name;
                    worksheet.Cells[i + 3, 3].Value = distribute.EquipmentCount;
                    worksheet.Cells[i + 3, 4].Value = distribute.Users.FullName;
                    worksheet.Cells[i + 3, 5].Value = distribute.Description;
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
