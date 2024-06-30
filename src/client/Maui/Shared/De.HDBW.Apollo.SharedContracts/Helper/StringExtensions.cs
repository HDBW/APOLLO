// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace De.HDBW.Apollo.SharedContracts.Helper
{
    public static class StringExtensions
    {
        public static (string Text, string? Question) ToTextAndQuestion(this string text, CultureInfo cultureInfo)
        {
            var currentText = text?.Trim();
            currentText = currentText?.Trim() ?? string.Empty;
            (string Text, string? Question) result = (currentText, null);

            var pattern = @"(?<=[\.\!\?])\s+";
            var detectionChar = "?";
            if (cultureInfo.TwoLetterISOLanguageName == "ar")
            {
                pattern = "(?<=[\\.\\!\\؟])\\s+";
                detectionChar = Convert.ToString((char)0x061F);
            }

            string[] sentences = Regex.Split(currentText, pattern);
            if (sentences.Length > 1 && sentences.Last().Trim().EndsWith(detectionChar))
            {
                result = (string.Join(" ", sentences.Take(sentences.Length - 1).Select(x => x.Trim())), sentences.Last().Trim());
            }

            if (sentences.Length == 1 && currentText.EndsWith(detectionChar))
            {
                result = (string.Empty, ReverseIfNeeded(currentText, cultureInfo));
            }

            return result;
        }

        private static string? ReverseIfNeeded(string? text, CultureInfo cultureInfo)
        {
            if (!cultureInfo.TextInfo.IsRightToLeft)
            {
                return text;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            StringBuilder reversed = new StringBuilder(text.Length);
            for (int i = text.Length - 1; i >= 0; i--)
            {
                reversed.Append(text[i]);
            }

            return reversed.ToString();
        }
    }
}
