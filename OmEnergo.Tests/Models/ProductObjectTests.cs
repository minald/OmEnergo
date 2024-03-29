﻿using OmEnergo.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OmEnergo.Tests.Models
{
	public class ProductObjectTests
	{
		private ProductModel productModel { get; set; }

		public ProductObjectTests()
		{
			var properties = "{\"Power\":\"11 kW\",\"Measurements\":\"127x150x200 mm\",\"Weight\":\"4 kg\"}";
			productModel = new ProductModel() { Properties = properties };
		}

		[Fact]
		public void GetProperties_AllPropertiesIsNotEmpty_ParsesPropertiesCorrectly()
		{
			//Arrange
			var expected = new Dictionary<string, string>()
			{
				["Power"] = "11 kW",
				["Measurements"] = "127x150x200 mm",
				["Weight"] = "4 kg"
			};

			//Act
			var actual = productModel.GetProperties();

			//Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetPropertiesWithValues_OnePropertyIsEmpty_IgnoresEmptyProperty()
		{
			//Arrange
			productModel.Properties = "{\"Power\":\"11 kW\",\"Year\":\"\",\"Weight\":\"4 kg\"}";
			var expected = new Dictionary<string, string>()
			{
				["Power"] = "11 kW",
				["Weight"] = "4 kg"
			};

			//Act
			var actual = productModel.GetPropertiesWithValues();

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("Measurements", "{\"Measurements\":\"127x150x200 mm\"}")]
		[InlineData("Weight;Power", "{\"Weight\":\"4 kg\",\"Power\":\"11 kW\"}")]
		[InlineData("Length;Weight", "{\"Length\":\"\",\"Weight\":\"4 kg\"}")]
		public void UpdateProperties_ValidPropertiesList_UpdatesProperties(string propertyNamesAsString, string expected)
		{
			//Arrange
			var propertyNames = propertyNamesAsString.Split(';').ToList();

			//Act
			productModel.UpdateProperties(propertyNames);
			var actual = productModel.Properties;

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("12 kW;127x150x200 mm;4 kg", "{\"Power\":\"12 kW\",\"Measurements\":\"127x150x200 mm\",\"Weight\":\"4 kg\"}")]
		[InlineData("10 kW;128x151x201 mm;42 kg", "{\"Power\":\"10 kW\",\"Measurements\":\"128x151x201 mm\",\"Weight\":\"42 kg\"}")]
		[InlineData(";;", "{\"Power\":\"\",\"Measurements\":\"\",\"Weight\":\"\"}")]
		public void UpdatePropertyValues_ValidPropertyValues_UpdatesProperties(string propertyValuesAsString, string expected)
		{
			//Arrange
			var propertyValues = propertyValuesAsString.Split(';');

			//Act
			productModel.UpdatePropertyValues(propertyValues);
			var actual = productModel.Properties;

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(";;", "{\"Power\":\"\",\"Measurements\":\"\",\"Weight\":\"\"}")]
		public void UpdatePropertyValues_PropertyValuesAreEmpty_UpdatesProperties(string propertyValuesAsString, string expected)
		{
			//Arrange
			var propertyValues = propertyValuesAsString.Split(';');

			//Act
			productModel.UpdatePropertyValues(propertyValues);
			var actual = productModel.Properties;

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(";", "{\"Power\":\"\",\"Measurements\":\"\"}")]
		[InlineData("", "{\"Power\":\"\"}")]
		[InlineData("12 kW;127x150x200 mm", "{\"Power\":\"12 kW\",\"Measurements\":\"127x150x200 mm\"}")]
		public void UpdatePropertyValues_LessAmountOfProperties_RemovesProperties(string propertyValuesAsString, string expected)
		{
			//Arrange
			var propertyValues = propertyValuesAsString.Split(';');

			//Act
			productModel.UpdatePropertyValues(propertyValues);
			var actual = productModel.Properties;

			//Assert
			Assert.Equal(expected, actual);
		}
	}
}
