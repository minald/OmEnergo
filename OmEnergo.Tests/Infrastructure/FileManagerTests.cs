using OmEnergo.Infrastructure;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
    public class FileManagerTests
    {
        [Theory]
        [InlineData(@"2\4\135.jpg", 135, true)]
        [InlineData(@"2\4\178_9db442b4-c892-441b-ab76-574fb1978a74.jpg", 178, false)]
        public void IsTheMainImage(string imagePath, int objectId, bool expected)
        {
            //Arrange

            //Act
            bool actual = FileManager.IsTheMainImage(imagePath, objectId);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"folder\anotherfolder\wwwroot\images\7.jpg", @"\images\7.jpg")]
        [InlineData(@"\images\9_Price_03_14.xlsx", @"\images\9_Price_03_14.xlsx")]
        public void GetRelativePath(string fullPath, string expected)
        {
            //Arrange

            //Act
            string actual = FileManager.GetRelativePath(fullPath);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"folder\anotherfolder\wwwroot\images\9_Price.xlsx", "Price.xlsx")]
        [InlineData(@"\images\9_Price_03_14.xlsx", "Price_03_14.xlsx")]
        public void GetFileName(string fullPath, string expected)
        {
            //Arrange

            //Act
            string actual = FileManager.GetFileName(fullPath);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"\images\17_Info.txt", true)]
        [InlineData(@"\images\17_NewPrice.xlsx", false)]
        public void CanBePreviewed(string path, bool expected)
        {
            //Arrange

            //Act
            bool actual = FileManager.CanBePreviewed(path);

            //Assert
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(@"\images\23_Booklet.pdf", "application/pdf")]
        [InlineData(@"\images\23_Description.txt", "text/plain")]
        public void GetContentType(string path, string expected)
        {
            //Arrange

            //Act
            string actual = FileManager.GetContentType(path);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
