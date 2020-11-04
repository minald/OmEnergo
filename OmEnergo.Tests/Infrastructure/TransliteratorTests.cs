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
		[InlineData(@"Звезда*", "zvezda*")]
		public void FromRussianToEnglish(string russianString, string expected)
		{
			//Act
			var actual = Transliterator.FromRussianToEnglish(russianString);

			//Assert
			Assert.Equal(expected, actual);
		}
	}
}
