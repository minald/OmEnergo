using OmEnergo.Infrastructure;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
	public class FileManagerTests
	{
		[Theory]
		[InlineData(@"folder\anotherfolder\wwwroot\images\7.jpg", @"\images\7.jpg")]
		[InlineData(@"\images\9_Price_03_14.xlsx", @"\images\9_Price_03_14.xlsx")]
		public void GetRelativePath(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetRelativePath(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"folder\anotherfolder\wwwroot\images\9_Price.xlsx", "Price.xlsx")]
		[InlineData(@"\images\9_Price_03_14.xlsx", "Price_03_14.xlsx")]
		public void GetFileName(string fullPath, string expected)
		{
			//Act
			var actual = FileManager.GetFileName(fullPath);

			//Assert
			Assert.Equal(expected, actual);
		}
		
		[Theory]
		[InlineData(@"\images\23_Booklet.pdf", "application/pdf")]
		[InlineData(@"\images\23_Description.txt", "text/plain")]
		public void GetContentType(string path, string expected)
		{
			//Act
			var actual = FileManager.GetContentType(path);

			//Assert
			Assert.Equal(expected, actual);
		}
	}
}
