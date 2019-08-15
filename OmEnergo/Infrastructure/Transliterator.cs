using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OmEnergo.Infrastructure
{
	public static class Transliterator
	{
		//In accordance with http://transliteration.ru/gosdep/
		private static Dictionary<char, string> RusToEngMatchingDictionary = new Dictionary<char, string>
		{
			{ 'а', "a" },
			{ 'б', "b" },
			{ 'в', "v" },
			{ 'г', "g" },
			{ 'д', "d" },
			{ 'е', "e" },
			{ 'ё', "e" },
			{ 'ж', "zh" },
			{ 'з', "z" },
			{ 'и', "i" },
			{ 'й', "y" },
			{ 'к', "k" },
			{ 'л', "l" },
			{ 'м', "m" },
			{ 'н', "n" },
			{ 'о', "o" },
			{ 'п', "p" },
			{ 'р', "r" },
			{ 'с', "s" },
			{ 'т', "t" },
			{ 'у', "u" },
			{ 'ф', "f" },
			{ 'х', "kh" },
			{ 'ц', "ts" },
			{ 'ч', "ch" },
			{ 'ш', "sh" },
			{ 'щ', "shch" },
			{ 'ъ', "" },
			{ 'ы', "y" },
			{ 'ь', "" },
			{ 'э', "e" },
			{ 'ю', "yu" },
			{ 'я', "ya" },
			{ ' ', "-" },
			{ '-', "-" }
		};

		public static string FromRussianToEnglish(string russianString)
		{
			string transliteratedString = "";
			foreach (var c in russianString.ToLower().Trim())
			{
				if (RusToEngMatchingDictionary.TryGetValue(c, out string engChar))
				{
					transliteratedString += engChar;
				}
				else if (Regex.IsMatch(c.ToString(), "[a-zA-Z0-9*]"))
				{
					transliteratedString += c;
				}
			}

			return transliteratedString;
		}
	}
}
