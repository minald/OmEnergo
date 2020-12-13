using ClosedXML.Excel;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OmEnergo.Infrastructure.Excel
{
	public class ExcelWriter
	{
		public const string XlsxMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		private const int ExcelDefaultRowHeight = 15;

		private readonly SectionRepository sectionRepository;
		private readonly ProductRepository productRepository;
		private readonly ProductModelRepository productModelRepository;
		private readonly ConfigKeyRepository configKeyRepository;

		public ExcelWriter(SectionRepository sectionRepository, ProductRepository productRepository, ProductModelRepository productModelRepository, ConfigKeyRepository configKeyRepository)
		{
			this.sectionRepository = sectionRepository;
			this.productRepository = productRepository;
			this.productModelRepository = productModelRepository;
			this.configKeyRepository = configKeyRepository;
		}

		public MemoryStream CreateExcelStream()
		{
			using var workbook = new XLWorkbook();
			AddWorksheet(workbook, sectionRepository.GetAll<Section>());
			AddWorksheet(workbook, productRepository.GetAll<Product>());
			AddWorksheet(workbook, productModelRepository.GetAll<ProductModel>());
			AddWorksheet(workbook, configKeyRepository.GetAll<ConfigKey>());
			SetCellsWidthAndHeight(workbook);
			return ToMemoryStream(workbook);
		}

		private void AddWorksheet<T>(XLWorkbook workbook, IEnumerable<T> objects)
		{
			var dataTableCreator = new DataTableCreatorFromIEnumerable<T>();
			using var dataTable = dataTableCreator.Create(objects);
			workbook.Worksheets.Add(dataTable);
		}

		private void SetCellsWidthAndHeight(XLWorkbook xlWorkbook)
		{
			xlWorkbook.Worksheets.ToList().ForEach(x => x.Rows(1, 10000).Height = ExcelDefaultRowHeight);
			foreach (var worksheet in xlWorkbook.Worksheets)
			{
				worksheet.Columns(1, 50).ToList().ForEach(c => SetColumnWidth(c));
			}
		}

		private void SetColumnWidth(IXLColumn column)
		{
			var cellsUsed = column.CellsUsed().Skip(1);
			if (column.FirstCell().Value as string == "Id")
			{
				column.Hide();
			}
			else
			{
				var isAllValuesAreNumbersOrEmpty = cellsUsed.Count() == 0 ||
					cellsUsed.All(x => Regex.IsMatch(x.Value as string, "^[0-9\\.\\,]+$"));
				column.Width = isAllValuesAreNumbersOrEmpty ? 10 : 60;
			}
		}

		private MemoryStream ToMemoryStream(XLWorkbook xlWorkbook)
		{
			var memoryStream = new MemoryStream();
			xlWorkbook.SaveAs(memoryStream, false);
			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}
