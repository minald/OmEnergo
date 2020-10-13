using ClosedXML.Excel;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OmEnergo.Infrastructure.Excel
{
	public class ExcelDbUpdater
	{
		private Repository _Repository { get; }

		private DataRow CurrentDataRow { get; set; }

		public ExcelDbUpdater(Repository repository) => _Repository = repository;

		public void ReadExcelAndUpdateDb(Stream excelFileStream)
		{
			using (var workbook = new XLWorkbook(excelFileStream))
			{
				UpdateValuesInTable<Section>(workbook);
				UpdateValuesInTable<Product>(workbook);
				UpdateValuesInTable<ProductModel>(workbook);
				UpdateValuesInTable<ConfigKey>(workbook);
			}
		}

		private void UpdateValuesInTable<T>(XLWorkbook workbook) where T : UniqueObject
		{
			IXLWorksheet worksheet = workbook.Worksheet(typeof(T).Name);
			using (DataTable dataTable = new DataTableCreatorFromXLWorkbook().Create(worksheet))
			{
				UpdateDbWithValuesFromDataTable<T>(dataTable);
			}
		}

		private void UpdateDbWithValuesFromDataTable<T>(DataTable dataTable) where T : UniqueObject
		{
			foreach (DataRow dataRow in dataTable.Rows)
			{
				CurrentDataRow = dataRow;
				UpdateCurrentRow<T>();
			}
		}

		private void UpdateCurrentRow<T>() where T : UniqueObject
		{
			List<PropertyInfo> primitiveProperties = GetPrimitiveProperties<T>();
			int id = Convert.ToInt32(CurrentDataRow["Id"]);
			T obj = _Repository.Get<T>(x => x.Id == id);
			primitiveProperties.ForEach(p => ReadAndSetValue(obj, p));
			_Repository.Update(obj);
		}

		private List<PropertyInfo> GetPrimitiveProperties<T>() => typeof(T).GetProperties()
			.Where(p => p.CanWrite && p.Name != "Id" && (p.PropertyType.IsPrimitive || p.PropertyType == typeof(String)))
			.ToList();

		private void ReadAndSetValue<T>(T obj, PropertyInfo property)
		{
			Type propertyType = property.PropertyType;
			string stringValue = CurrentDataRow[property.Name] as string;
			stringValue = ConvertNonIntegerValueToEnUsFormat(stringValue, propertyType);
			ChangeTypeAndSetValue(obj, property, stringValue);
		}

		private string ConvertNonIntegerValueToEnUsFormat(string stringValue, Type type) =>
			IsNonIntegerNumber(type) ? stringValue.Replace(",", ".") : stringValue;

		private bool IsNonIntegerNumber(Type type) => type == typeof(double) || type == typeof(float) || type == typeof(decimal);

		private void ChangeTypeAndSetValue<T>(T obj, PropertyInfo property, string stringValue)
		{
			Type propertyType = property.PropertyType;
			try
			{
				var value = Convert.ChangeType(stringValue, propertyType, CultureInfo.CreateSpecificCulture("en-US"));
				property.SetValue(obj, value);
			}
			catch (Exception ex)
			{
				string message = $"Значение некорректно и не может быть обновлено."
					+ $"\nТаблица {CurrentDataRow.Table.TableName}, Строка: {CurrentDataRow.Table.Rows.IndexOf(CurrentDataRow) + 2}"
					+ $"\nСвойство: {property.Name}, Значение: {stringValue}";
				if (IsNonIntegerNumber(propertyType))
				{
					message += $"\nДопустимый формат нецелых чисел: 1234.56 или 1234,56";
				}

				throw new Exception(message, ex);
			}
		}
	}
}
