using OmEnergo.Services;
using Xunit;

namespace OmEnergo.Tests.Services
{
	public class AdminFileManagerServiceTests
	{
		[Theory]
		[InlineData(@"2\4\s135.jpg")]
		[InlineData(@"2\4\s135-250.jpg")]
		[InlineData(@"2\5\p28.jpg")]
		[InlineData(@"2\5\p28-80.jpg")]
		public void IsTheMainImage_MainImageName_ReturnsTrue(string imagePath)
		{
			//Act
			var actual = AdminFileManagerService.IsTheMainImage(imagePath);

			//Assert
			Assert.True(actual);
		}

		[Theory]
		[InlineData(@"2\4\m178_9db442b4-c892-441b-ab76-574fb1978a74.jpg")]
		[InlineData(@"2\4\m178-250_9db442b4-c892-441b-ab76-574fb1978a74.jpg")]
		public void IsTheMainImage_MainImageName_ReturnsFalse(string imagePath)
		{
			//Act
			var actual = AdminFileManagerService.IsTheMainImage(imagePath);

			//Assert
			Assert.False(actual);
		}

		[Theory]
		[InlineData(@"\images\17_Info.txt")]
		[InlineData(@"18_Details.pdf")]
		public void IsDocumentCanBePreviewed_SupportedExtension_ReturnsTrue(string path)
		{
			//Act
			var actual = AdminFileManagerService.IsDocumentCanBePreviewed(path);

			//Assert
			Assert.True(actual);
		}

		[Theory]
		[InlineData(@"\images\17_NewPrice.xlsx")]
		[InlineData(@"18_NewDetails.docx")]
		public void IsDocumentCanBePreviewed_UnsupportedExtension_ReturnsFalse(string path)
		{
			//Act
			var actual = AdminFileManagerService.IsDocumentCanBePreviewed(path);

			//Assert
			Assert.False(actual);
		}
	}
}
