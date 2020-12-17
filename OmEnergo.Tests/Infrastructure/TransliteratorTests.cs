using OmEnergo.Infrastructure;
using System;
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
		public void FromRussianToEnglish_ValidValue_ReturnsTransliteratedString(string russianString, string expected)
		{
			//Act
			var actual = Transliterator.FromRussianToEnglish(russianString);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(@"", "")]
		[InlineData(@"   ", "")]
		public void FromRussianToEnglish_EmptyOrWhitespaceValue_ReturnsEmptyString(string russianString, string expected)
		{
			//Act
			var actual = Transliterator.FromRussianToEnglish(russianString);

			//Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(null)]
		public void FromRussianToEnglish_NullValue_ThrowsException(string russianString)
		{
			//Act
			Action action = () => Transliterator.FromRussianToEnglish(russianString);

			//Assert
			Assert.Throws<NullReferenceException>(action);
		}
	}
}
