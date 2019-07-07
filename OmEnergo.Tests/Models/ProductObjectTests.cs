using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OmEnergo.Tests.Models
{
    public class ProductObjectTests
    {
        private ProductModel ProductModel { get; set; }

        public ProductObjectTests()
        {
            string properties = "{\"Power\":\"11 kW\",\"Measurements\":\"127x150x200 mm\",\"Weight\":\"4 kg\"}";
            ProductModel = new ProductModel() { Properties = properties };
        }

        [Fact]
        public void GetProperties()
        {
            //Arrange
            var expected = new Dictionary<string, string>()
            {
                ["Power"] = "11 kW",
                ["Measurements"] = "127x150x200 mm",
                ["Weight"] = "4 kg"
            };

            //Act
            var actual = ProductModel.GetProperties();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPropertiesWithValues()
        {
            //Arrange
            ProductModel.Properties = "{\"Power\":\"11 kW\",\"Year\":\"\",\"Weight\":\"4 kg\"}";
            var expected = new Dictionary<string, string>()
            {
                ["Power"] = "11 kW",
                ["Weight"] = "4 kg"
            };

            //Act
            var actual = ProductModel.GetPropertiesWithValues();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Measurements", "{\"Measurements\":\"127x150x200 mm\"}")]
        [InlineData("Weight;Power", "{\"Weight\":\"4 kg\",\"Power\":\"11 kW\"}")]
        [InlineData("Length;Weight", "{\"Length\":\"\",\"Weight\":\"4 kg\"}")]
        public void UpdateProperties(string propertyNamesAsString, string expected)
        {
            //Arrange
            List<string> propertyNames = propertyNamesAsString.Split(';').ToList();

            //Act
            ProductModel.UpdateProperties(propertyNames);

            //Assert
            string actual = ProductModel.Properties;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("12 kW;127x150x200 mm;4 kg", "{\"Power\":\"12 kW\",\"Measurements\":\"127x150x200 mm\",\"Weight\":\"4 kg\"}")]
        [InlineData("10 kW;128x151x201 mm;42 kg", "{\"Power\":\"10 kW\",\"Measurements\":\"128x151x201 mm\",\"Weight\":\"42 kg\"}")]
        [InlineData(";;", "{\"Power\":\"\",\"Measurements\":\"\",\"Weight\":\"\"}")]
        public void UpdatePropertyValues(string propertyValuesAsString, string expected)
        {
            //Arrange
            string[] propertyValues = propertyValuesAsString.Split(';');

            //Act
            ProductModel.UpdatePropertyValues(propertyValues);

            //Assert
            string actual = ProductModel.Properties;
            Assert.Equal(expected, actual);
        }
    }
}
