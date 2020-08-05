using System.Collections.Generic;

namespace TextAnalyzer.Modules.Models
{
    public class CodeMeanings
    {
        public Dictionary<EntryCodes, string> codeMeaning { get; }
        public CodeMeanings()
        {
            codeMeaning = new Dictionary<EntryCodes, string>();
            codeMeaning.Add(EntryCodes.LongestWord, "Longest word");
            codeMeaning.Add(EntryCodes.LargestNumber, "Largest number");
            codeMeaning.Add(EntryCodes.OnlyConsonat, "Only consonat symbols");
            codeMeaning.Add(EntryCodes.OnlyVowel, "Only vowel symbols");
        }
    }
}
