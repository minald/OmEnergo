using ClosedXML.Excel;
using Moq;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Infrastructure.Excel;
using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
	public class ExcelWriterTests
	{
		private readonly ExcelWriter excelWriter;

		public ExcelWriterTests()
		{
			var sections = new List<Section>()
			{
				new Section() { Id = 1, Name = "SectionA", ProductProperties = "Width,Height" },
				new Section() { Id = 2, Name = "SectionB", ProductProperties = "Weight" }
			};
			var productModels = new List<ProductModel>()
			{
				new ProductModel() { Id = 1, Name = "ModelA", Section = sections[0] },
				new ProductModel() { Id = 2, Name = "ModelB", Section = sections[1] }
			};
			var repositoryMock = new Mock<Repository>();
			repositoryMock.Setup(x => x.GetAllSections()).Returns(sections);
			repositoryMock.Setup(x => x.GetAllProducts()).Returns(new List<Product>());
			repositoryMock.Setup(x => x.GetAllProductModels()).Returns(productModels);
			repositoryMock.Setup(x => x.GetAllConfigKeys()).Returns(new List<ConfigKey>());
			excelWriter = new ExcelWriter(repositoryMock.Object);
		}

		[Fact]
		public void CreateExcelStream()
		{
			//Act
			using var actualMemoryStream = excelWriter.CreateExcelStream();
			using var actualXlWorkbook = new XLWorkbook(actualMemoryStream);

			//Assert
			Assert.Equal(4, actualXlWorkbook.Worksheets.Count);
			Assert.Equal(3, actualXlWorkbook.Worksheet("Section").RowsUsed().Count());
			Assert.Equal(3, actualXlWorkbook.Worksheet("ProductModel").RowsUsed().Count());
			Assert.Single(actualXlWorkbook.Worksheet("Product").RowsUsed());
			Assert.Single(actualXlWorkbook.Worksheet("ConfigKey").RowsUsed());
		}
	}
}
