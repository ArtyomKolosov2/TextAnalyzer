using System.Collections.Generic;

namespace TextAnalyzer.Modules.Models
{
    public class CodeMeanings
    {
        public Dictionary<EntryCodes, string> codeMeaning { get; }
        public CodeMeanings()
        {
            codeMeaning = new Dictionary<EntryCodes, string>
            {
                { EntryCodes.LongestWord, "Longest word" },
                { EntryCodes.OnlyConsonat, "Only consonat symbols" },
                { EntryCodes.OnlyVowel, "Only vowel symbols" },
                { EntryCodes.Number, "Number" },
                { EntryCodes.LargestNumber, "Largest number" },
                { EntryCodes.LowestNumber, "LowestNumber" }
            };
        }
    }
}
