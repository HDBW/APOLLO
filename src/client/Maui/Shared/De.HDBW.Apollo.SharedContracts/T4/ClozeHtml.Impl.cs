// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.T4
{
    public partial class ClozeHtml
    {
        private static readonly string INPUTFIELD = "<span class=\"inputdiv\"><input type=\"email\" id=\"{0}\" enterkeyhint=\"{1}\" name=\"{0}\" autocorrect=\"off\" autocapitalize=\"off\" autocomplete=\"off\" spellcheck=\"false\" /></span>";
        private static readonly string SELECTFIELD = "<input type=\"email\" id=\"{0}\" readonly />";

        public ClozeHtml(string input, Dictionary<int, InputMode> inputModes, Dictionary<int, string> initialValues)
        {
            Input = input;
            var tokens = GetTokens();
            if (tokens.Count == 0)
            {
                return;
            }

            foreach (var token in tokens)
            {
                var id = GetIdFromToken(token);
                if (string.IsNullOrWhiteSpace(id))
                {
                    continue;
                }

                Tokens.Add(token);
                Ids.Add(id);
                var index = Ids.IndexOf(id) + 1;
                if (initialValues.ContainsKey(index))
                {
                    Values.Add(id, initialValues[index]);
                }
            }

            Body = Input;
            for (var i = 0; i < Ids.Count; i++)
            {
                var inputMode = inputModes[i + 1];
                var nextInputMode = inputModes.ContainsKey(i + 2) ? inputModes[i + 2] : (InputMode?)null;
                switch (inputMode)
                {
                    case InputMode.TextInput:
                        Body = Body.Replace(Tokens[i], GenerateInput(Ids[i], nextInputMode == InputMode.TextInput));
                        break;
                    case InputMode.Choice:
                        Body = Body.Replace(Tokens[i], GenerateSelect(Ids[i]));
                        break;
                }
            }
        }

        public string Body { get; } = string.Empty;

        public string Input { get; }

        public IList<string> Tokens { get; } = new List<string>();

        public IList<string> Ids { get; } = new List<string>();

        public Dictionary<string, string> Values { get; } = new Dictionary<string, string>();

        private IList<string> GetTokens()
        {
            if (string.IsNullOrWhiteSpace(Input))
            {
                return new List<string>();
            }

            string pattern = @"\{([^}]+)\}"; // A regex pattern to match anything inside curly brackets
            var matches = Regex.Matches(Input, pattern);
            return matches.Select(x => x.Value).ToList();
        }

        private string? GetIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            string pattern = token.Contains(":") ? @"\{([^:]+)" : @"\{(.*?)\}";
            var matches = Regex.Matches(token, pattern);
            return matches.First().Groups[1].Value;
        }

        private string GenerateInput(string id, bool allowNavigateNext)
        {
            var enterKey = allowNavigateNext ? "next" : "done";
            return string.Format(INPUTFIELD, id, enterKey);
        }

        private string GenerateSelect(string id)
        {
            return string.Format(SELECTFIELD, id);
        }
    }
}
