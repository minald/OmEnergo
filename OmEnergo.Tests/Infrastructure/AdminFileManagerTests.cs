using OmEnergo.Infrastructure;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
	public class AdminFileManagerTests
	{
		[Theory]
		[InlineData(@"2\4\135.jpg", 135, true)]
		[InlineData(@"2\4\178_9db442b4-c892-441b-ab76-574fb1978a74.jpg", 178, false)]
		public void IsTheMainImage(string imagePath, int objectId, bool expected)
		{
			//Act
			var actual = AdminFileManager.IsTheMainImage(imagePath, objectId);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"\images\17_Info.txt", true)]
		[InlineData(@"\images\17_NewPrice.xlsx", false)]
		public void CanBePreviewed(string path, bool expected)
		{
			//Act
			var actual = AdminFileManager.IsDocumentCanBePreviewed(path);

			//Assert
			Assert.Equal(expected, actual);
		}
	}
}
