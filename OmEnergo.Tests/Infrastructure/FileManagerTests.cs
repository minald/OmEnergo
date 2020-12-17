using OmEnergo.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
	public class FileManagerTests
	{
		[Theory]
		[InlineData(@"folder\anotherfolder\wwwroot\images\7.jpg", @"\images\7.jpg")]
		[InlineData(@"\images\9_Price_03_14.xlsx", @"\images\9_Price_03_14.xlsx")]
		[InlineData(@"\images\images\9_Price_03_15.xls", @"\images\images\9_Price_03_15.xls")]
		public void GetRelativePath_PathWithImagesFolder_ReturnsValidSubpath(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetRelativePath(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"\folder\9_Price_03_16.xls", @"\folder\9_Price_03_16.xls")]
		[InlineData(@"\9_Price_03_17.xls", @"\9_Price_03_17.xls")]
		public void GetRelativePath_PathWithoutImagesFolder_ReturnsInitialPath(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetRelativePath(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(null)]
		public void GetRelativePath_NullPath_ThrowsException(string fullPath)
		{
			//Act
			Action action = () => FileManager.GetRelativePath(fullPath);

			//Assert
			Assert.Throws<NullReferenceException>(action);
		}

		[Theory]
		[InlineData(@"folder\anotherfolder\wwwroot\images\9_Price.xlsx", "Price.xlsx")]
		[InlineData(@"\images\9_Price_03_14.xlsx", "Price_03_14.xlsx")]
		[InlineData(@"\ima_ges\9_Price_03_17.xlsx", "Price_03_17.xlsx")]
		[InlineData(@"\images\images\9__Price_03_15.xlsx", "_Price_03_15.xlsx")]
		public void GetFileName_FileNameWithUnderscore_ReturnsValidFileName(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetFileName(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"\images\Price0316.xlsx", "Price0316.xlsx")]
		public void GetFileName_FileNameWithoutUnderscore_ReturnsFullFileName(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetFileName(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(null)]
		public void GetFileName_NullPath_ThrowsException(string fullPath)
		{
			//Act
			Action action = () => FileManager.GetFileName(fullPath);

			//Assert
			Assert.Throws<NullReferenceException>(action);
		}

		[Theory]
		[InlineData(@"\images\23_Booklet.pdf", "application/pdf")]
		[InlineData(@"\images\23_Description.txt", "text/plain")]
		public void GetContentType_ValidExtension_ReturnsCorrespondingContentType(string path, string expected)
		{
			//Act
			var actual = FileManager.GetContentType(path);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"\images\24_Booklet.pgw")]
		[InlineData(@"\images\24_Description.tht")]
		public void GetContentType_InvalidExtension_ThrowsKeyNotFoundException(string path)
		{
			//Act
			Action action = () => FileManager.GetContentType(path);

			//Assert
			Assert.Throws<KeyNotFoundException>(action);
		}
	}
}
