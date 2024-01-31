using Geradovana.ScrapingService.Application.Common.Constants;
using System.Text;
using System.Text.RegularExpressions;

namespace Geradovana.ScrapingService.Application.Common.Utils
{
    public static class StringUtils
    {
        private static readonly Dictionary<char, char> LithuanianLetters = new Dictionary<char, char>
        {
            {'ą', 'a'}, {'č', 'c'}, {'ę', 'e'}, {'ė', 'e'}, {'į', 'i'},
            {'š', 's'}, {'ų', 'u'}, {'ū', 'u'}, {'ž', 'z'},
            {'Ą', 'A'}, {'Č', 'C'}, {'Ę', 'E'}, {'Ė', 'E'}, {'Į', 'I'},
            {'Š', 'S'}, {'Ų', 'U'}, {'Ū', 'U'}, {'Ž', 'Z'},
        };

        public static string ConvertLithuanianToAscii(string inputText)
        {
            StringBuilder convertedText = new StringBuilder();
            foreach (char character in inputText)
            {
                if (LithuanianLetters.TryGetValue(character, out char replacement))
                {
                    convertedText.Append(replacement);
                }
                else
                {
                    convertedText.Append(character);
                }
            }
            return convertedText.ToString();
        }

        public static string RemoveEmoji(string inputText)
        {
            if(Regex.IsMatch(inputText, RegexPatterns.EmojiPattern))
            {
                return Regex.Replace(inputText, RegexPatterns.EmojiPattern, string.Empty);
            }

            return inputText;
        }

        public static string ExtractDigits(string inputText)
        {
            return Regex.Match(inputText, RegexPatterns.DigitsPattern).Value;
        }
    }
}