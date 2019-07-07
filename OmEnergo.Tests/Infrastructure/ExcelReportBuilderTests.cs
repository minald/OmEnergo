using ClosedXML.Excel;
using Moq;
using OmEnergo.Infrastructure;
using OmEnergo.Infrastructure.Database;
using OmEnergo.Models;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
    public class ExcelReportBuilderTests
    {
        private ExcelReportBuilder ExcelReportBuilder { get; set; }

        public ExcelReportBuilderTests()
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
            ExcelReportBuilder = new ExcelReportBuilder(repositoryMock.Object);
        }

        [Fact]
        public void CreateDatabaseBackup()
        {
            //Arrange

            //Act
            MemoryStream actualMemoryStream = ExcelReportBuilder.CreateDatabaseBackup();
            var actualXlWorkbook = new XLWorkbook(actualMemoryStream);

            //Assert
            Assert.Equal(4, actualXlWorkbook.Worksheets.Count);
        }

        [Fact]
        public void CreatePricesReport()
        {
            //Arrange

            //Act
            MemoryStream actualMemoryStream = ExcelReportBuilder.CreatePricesReport();
            var actualXlWorkbook = new XLWorkbook(actualMemoryStream);
            IXLWorksheet actualWorksheet = actualXlWorkbook.Worksheets.Worksheet("ProductModel");

            //Assert
            Assert.Equal(1, actualXlWorkbook.Worksheets.Count);
            Assert.Equal("Name", actualWorksheet.Cell(1, 1).GetString());
            Assert.Equal("Price", actualWorksheet.Cell(1, 2).GetString());
            Assert.Equal("ModelA", actualWorksheet.Cell(2, 1).GetString());
            Assert.Equal("", actualWorksheet.Cell(2, 3).GetString());
            Assert.Equal("ModelB", actualWorksheet.Cell(3, 1).GetString());
            Assert.Equal("SectionB", actualWorksheet.Cell(3, 4).GetString());
        }
    }
}
