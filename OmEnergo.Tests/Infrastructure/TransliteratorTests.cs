using OmEnergo.Infrastructure;
using Xunit;

namespace OmEnergo.Tests.Infrastructure
{
    public class TransliteratorTests
    {
        [Theory]
        [InlineData(@"слово", "slovo")]
        [InlineData(@"englishWord", "englishword")]
        [InlineData(@"цифра2", "tsifra2")]
        [InlineData(@"белый котёнок", "belyy-kotenok")]
        [InlineData(@"Щи-БОРщ", "shchi-borshch")]
        public void FromRussianToEnglish(string russianString, string expected)
        {
            //Arrange

            //Act
            string actual = Transliterator.FromRussianToEnglish(russianString);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
