using OmEnergo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OmEnergo.Infrastructure.Excel
{
    public class DataTableCreatorFromIEnumerable<T> : IDataTableCreator
    {
        public DataTable Create(object data) => Create(data as IEnumerable<T>);

        private DataTable Create(IEnumerable<T> objects)
        {
            var dataTable = new DataTable(typeof(T).Name);
            List<PropertyInfo> properties = GetNonListProperties();
            properties.ForEach(x => dataTable.Columns.Add(x.Name));
            FillDataTable(dataTable, objects, properties);
            return dataTable;
        }

        private List<PropertyInfo> GetNonListProperties() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => !x.PropertyType.GetInterfaces().Contains(typeof(IList))).ToList();

        private void FillDataTable(DataTable dataTable, IEnumerable<T> objects, List<PropertyInfo> properties)
        {
            foreach (T obj in objects)
            {
                object[] propertyValues = GetPropertyValues(obj, properties).ToArray();
                dataTable.Rows.Add(propertyValues);
            }
        }

        private IEnumerable<object> GetPropertyValues(T obj, List<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                object value = property.GetValue(obj, null);
                object resultValue = value is UniqueObject uniqueObject ? uniqueObject.Id : value;
                yield return resultValue;
            }
        }
    }
}
