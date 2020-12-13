using ClosedXML.Excel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
		private readonly CompoundRepository compoundRepository;

		private readonly ILogger<ExcelDbUpdater> logger;

		private readonly IStringLocalizer localizer;

		private DataRow currentDataRow { get; set; }

		public ExcelDbUpdater(CompoundRepository compoundRepository, ILogger<ExcelDbUpdater> logger, IStringLocalizer localizer)
		{
			this.compoundRepository = compoundRepository;
			this.logger = logger;
			this.localizer = localizer;
		}

		public void ReadExcelAndUpdateDb(Stream excelFileStream)
		{
			using var workbook = new XLWorkbook(excelFileStream);
			UpdateValuesInTable<Section>(workbook);
			UpdateValuesInTable<Product>(workbook);
			UpdateValuesInTable<ProductModel>(workbook);
			UpdateValuesInTable<ConfigKey>(workbook);
		}

		private void UpdateValuesInTable<T>(XLWorkbook workbook) where T : UniqueObject
		{
			var worksheet = workbook.Worksheet(typeof(T).Name);
			using var dataTable = new DataTableCreatorFromXLWorkbook().Create(worksheet);
			UpdateDbWithValuesFromDataTable<T>(dataTable);
		}

		private void UpdateDbWithValuesFromDataTable<T>(DataTable dataTable) where T : UniqueObject
		{
			foreach (DataRow dataRow in dataTable.Rows)
			{
				currentDataRow = dataRow;
				UpdateCurrentRow<T>();
			}
		}

		private void UpdateCurrentRow<T>() where T : UniqueObject
		{
			var primitiveProperties = GetPrimitiveProperties<T>();
			var id = Convert.ToInt32(currentDataRow["Id"]);
			var obj = compoundRepository.Get<T>(x => x.Id == id);
			primitiveProperties.ForEach(p => ReadAndSetValue(obj, p));
			compoundRepository.Update(obj);
		}

		private List<PropertyInfo> GetPrimitiveProperties<T>() => typeof(T).GetProperties()
			.Where(p => p.CanWrite && p.Name != "Id" && (p.PropertyType.IsPrimitive || p.PropertyType == typeof(String)))
			.ToList();

		private void ReadAndSetValue<T>(T obj, PropertyInfo property)
		{
			var propertyType = property.PropertyType;
			var stringValue = currentDataRow[property.Name] as string;
			stringValue = ConvertNonIntegerValueToEnUsFormat(stringValue, propertyType);
			ChangeTypeAndSetValue(obj, property, stringValue);
		}

		private string ConvertNonIntegerValueToEnUsFormat(string stringValue, Type type) =>
			IsNonIntegerNumber(type) ? stringValue.Replace(",", ".") : stringValue;

		private bool IsNonIntegerNumber(Type type) => type == typeof(double) || type == typeof(float) || type == typeof(decimal);

		private void ChangeTypeAndSetValue<T>(T obj, PropertyInfo property, string stringValue)
		{
			var propertyType = property.PropertyType;
			try
			{
				var value = Convert.ChangeType(stringValue, propertyType, CultureInfo.CreateSpecificCulture("en-US"));
				property.SetValue(obj, value);
			}
			catch (Exception ex)
			{
				var message = localizer["TheValueIsInvalidAndCannotBeUpdated"].Value
					.Replace("{{tableName}}", currentDataRow.Table.TableName)
					.Replace("{{rowNumber}}", (currentDataRow.Table.Rows.IndexOf(currentDataRow) + 2).ToString())
					.Replace("{{propertyName}}", property.Name)
					.Replace("{{propertyValue}}", stringValue);
				if (IsNonIntegerNumber(propertyType))
				{
					message += Environment.NewLine + localizer["ValidNonIntegerFormat"];
				}

				logger.LogError($"ExcelDbUpdater: {message}");
				throw new Exception(message, ex);
			}
		}
	}
}
