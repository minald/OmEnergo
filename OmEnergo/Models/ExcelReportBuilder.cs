using ClosedXML.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OmEnergo.Models
{
    public class ExcelReportBuilder
    {
        public const string XlsxMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private Repository Repository { get; }

        public ExcelReportBuilder(Repository repository) => Repository = repository;

        public MemoryStream CreateDatabaseBackup()
        {
            var workbook = new XLWorkbook();
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllSections()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllProducts()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllProductModels()));
            workbook.Worksheets.Add(ToDataTable(Repository.GetAllConfigKeys()));
            return workbook.ToMemoryStream();
        }

        public MemoryStream CreatePricesReport()
        {
            var workbook = new XLWorkbook();
            var allProductModels = Repository.GetAllProductModels();
            var propertiesList = new List<string>() { "Name", "Price", "Product", "Section" };
            workbook.Worksheets.Add(ToDataTable(allProductModels, propertiesList, "Name"));
            return workbook.ToMemoryStream();
        }

        private DataTable ToDataTable<T>(IEnumerable<T> items, List<string> propertyNames = null, string defaultProperty = "Id")
        {
            var dataTable = new DataTable(typeof(T).Name);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.GetInterfaces().Contains(typeof(IList))).ToList();
            properties = SortAndFilterProperties(properties, propertyNames);
            properties.ForEach(x => dataTable.Columns.Add(x.Name));

            foreach (T item in items)
            {
                var values = new List<object>();
                foreach (var property in properties)
                {
                    var mainObj = property.GetValue(item, null);
                    var resultObj = mainObj is CommonObject ? GetPropertyValue(mainObj, defaultProperty) : mainObj;
                    values.Add(resultObj);
                }

                dataTable.Rows.Add(values.ToArray());
            }

            return dataTable;
        }

        private List<PropertyInfo> SortAndFilterProperties(List<PropertyInfo> properties, List<string> propertyNames)
        {
            if (propertyNames == null)
            {
                return properties;
            }
            else
            {
                var result = new List<PropertyInfo>();
                propertyNames.ForEach(pn => result.Add(properties.FirstOrDefault(p => p.Name == pn)));
                return result;
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj?.GetType()?.GetProperty(propertyName)?.GetValue(obj, null) ?? "NULL";
        }
    }

    public static class XLWorkbookExtension
    {
        public static MemoryStream ToMemoryStream(this XLWorkbook xlWorkbook)
        {
            var memoryStream = new MemoryStream();
            xlWorkbook.SaveAs(memoryStream, false);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
