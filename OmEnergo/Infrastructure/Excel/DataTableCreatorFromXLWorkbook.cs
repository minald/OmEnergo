using ClosedXML.Excel;
using System.Data;
using System.Linq;

namespace OmEnergo.Infrastructure.Excel
{
	public class DataTableCreatorFromXLWorkbook : IDataTableCreator
	{
		public DataTable Create(object data) => Create(data as IXLWorksheet);

		private DataTable Create(IXLWorksheet worksheet)
		{
			var dataTable = new DataTable(worksheet.Name);
			AddColumns(dataTable, worksheet);
			FillDataTable(dataTable, worksheet);
			return dataTable;
		}

		private void AddColumns(DataTable dataTable, IXLWorksheet worksheet)
		{
			foreach (IXLCell cell in worksheet.FirstRow().CellsUsed())
			{
				string columnName = cell.Value.ToString();
				dataTable.Columns.Add(columnName);
			}
		}

		private void FillDataTable(DataTable dataTable, IXLWorksheet worksheet) =>
			worksheet.RowsUsed().Skip(1).ToList().ForEach(r => AddRowWithData(dataTable, r));

		private void AddRowWithData(DataTable dataTable, IXLRow row)
		{
			dataTable.Rows.Add();
			foreach (IXLCell cell in row.Cells(1, dataTable.Columns.Count))
			{
				dataTable.Rows[cell.Address.RowNumber - 2][cell.Address.ColumnNumber - 1] = cell.Value.ToString();
			}
		}
	}
}
