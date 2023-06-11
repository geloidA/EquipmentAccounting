using EquipmentAccounting.DataBase;
using EquipmentAccounting.Extensions;
using EquipmentAccounting.Help;
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
    /// Interaction logic for EquipmentBuildsWindow.xaml
    /// </summary>
    public partial class EquipmentBuildsWindow : Window
    {
        public ObservableCollection<EquipmentBuild> EquipmentBuilds { get; set; }
        public EquipmentBuild SelectedBuild { get; set; }
        public string SearchText { get; set; } = "";

        public EquipmentBuildsWindow()
        {
            InitializeComponent();
            DataContext = this;
            EquipmentBuilds = new ObservableCollection<EquipmentBuild>(Entities.Context.EquipmentBuild);
        }

        private void UpdateBuilds()
        {
            EquipmentBuilds.Clear();
            EquipmentBuilds.AddRange(Entities.Context.EquipmentBuild
                .ToList()
                .Where(x => x.GetSearchString().Contains(SearchText.ToLower())));
        }

        private void AddNewEquipmentBuild(object sender, RoutedEventArgs e)
        {
            var res = new AddEditEquipmentBuildsWindow().ShowDialog();
            if (res == true)
            {
                UpdateBuilds();
                (Owner as MainWindow).UpdateEquipments();
            }
        }

        private void DisbandBuild(object sender, RoutedEventArgs e)
        {
            if (SelectedBuild is null) return;
            var res = MessageBox.Show("Вы действительно хотите расформировать сборку", "Расформирование", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                var toRemoveEquip = new List<(Equipments Remove, Equipments Substitute)>();
                var stockLoc = Entities.Context.Locations.First(x => x.Name == "Склад");
                foreach (var equip in SelectedBuild.Equipments)
                {
                    var stockEquip = Entities.Context.Equipments.FirstOrDefault(x => x.Name == equip.Name && x.LocationID == stockLoc.ID);
                    if (stockEquip != null)
                    {
                        stockEquip.Count += equip.Count;
                        toRemoveEquip.Add((equip, stockEquip));
                    }
                    else
                    {
                        equip.Locations = stockLoc;
                        equip.EquipmentBuild = null;
                    }
                }
                Entities.Context.EquipmentBuild.Remove(SelectedBuild);
                foreach (var pair in toRemoveEquip)
                    DBHelp.RemoveEquipment(pair.Remove, pair.Substitute);
                Entities.Context.SaveChanges();
                UpdateBuilds();
                (Owner as MainWindow).UpdateEquipments();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBuilds();
        }

        private void BtnCreateExcelReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { DefaultExt = "xlsx", Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*" };
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == true)
            {
                GenerateBuildsReport(EquipmentBuilds, dialog.FileName);
                MessageBox.Show("Отчет сохранен.");
            }
        }

        public void GenerateBuildsReport(Collection<EquipmentBuild> builds, string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Отчет по сборкам оборудования");

                // Установка значения ячейки для заголовка
                worksheet.Cells["A1"].Value = "Отчет по сборкам оборудования";

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
                worksheet.Cells[2, 1].Value = "Инвентарный номер";
                worksheet.Cells[2, 2].Value = "Расположение";
                worksheet.Cells[2, 3].Value = "Дата";
                worksheet.Cells[2, 4].Value = "Имя оборудования";
                worksheet.Cells[2, 5].Value = "Тип оборудования";
                worksheet.Cells[2, 6].Value = "Кол-во, шт.";

                int row = 3;
                // Заполнение данных
                foreach (var build in builds)
                {
                    worksheet.Cells[row, 1].Value = build.InventoryNumber;
                    worksheet.Cells[row, 2].Value = build.Locations.Name;
                    worksheet.Cells[row, 3].Value = build.Date.ToString("dd.MM.yyyy");

                    // Для каждого оборудования в сборке
                    foreach (var equipment in build.Equipments)
                    {
                        worksheet.Cells[row, 4].Value = equipment.Name;
                        worksheet.Cells[row, 5].Value = equipment.EquipmentTypes.Name;
                        worksheet.Cells[row, 6].Value = equipment.Count;
                        row++;
                    }

                    // Добавление пустой строки между сборками
                    row++;
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
