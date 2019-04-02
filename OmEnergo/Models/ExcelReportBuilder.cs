using ClosedXML.Excel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OmEnergo.Models
{
    public class ExcelReportBuilder
    {
        private Repository Repository { get; }

        public ExcelReportBuilder(Repository repository) => Repository = repository;

        public void CreateDatabaseBackup(string backupPath)
        {
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllSections()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllProducts()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllProductModels()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllConfigKeys()));
            workbook.SaveAs(backupPath);
        }

        public void CreatePricesReport(string backupPath)
        {
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllProductModels(), new List<string>() { "Name", "Price" }));
            workbook.SaveAs(backupPath);
        }

        public static DataTable ToDataTable<T>(IEnumerable<T> items, List<string> propertyNames = null)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType.Name != "List`1" && 
                    (propertyNames == null || propertyNames.Contains(x.Name))).ToList();
            properties.ForEach(x => dataTable.Columns.Add(x.Name));

            foreach (T item in items)
            {
                var values = new List<object>();
                foreach (var property in properties)
                {
                    var mainObj = property.GetValue(item, null);
                    var resultObj = mainObj is CommonObject ? GetPropertyValue(mainObj, "Id") : mainObj;
                    values.Add(resultObj);
                }

                dataTable.Rows.Add(values.ToArray());
            }

            return dataTable;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            return obj?.GetType()?.GetProperty(propertyName)?.GetValue(obj, null) ?? "NULL";
        }
    }
}
