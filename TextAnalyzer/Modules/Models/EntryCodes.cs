﻿using System;
using System.Drawing;

namespace TextAnalyzer.Modules.Models
{
    public enum EntryCodes
    {
        LongestWord,
        OnlyConsonat,
        OnlyVowel,
        Number, 
        LargestNumber,
        LowestNumber
    }
    public static class GetCode
    {
        private static CodeMeanings CodeMeanings { get; } = new CodeMeanings();
        public static string GetCodeMeaning(EntryCodes code)
        {
            return CodeMeanings.codeMeaning[code];
        }

        public static EntryCodes GetCodeByColor(Color color)
        {
            EntryCodes code = default;
            for (int i = 0; i < GetColor.textColors.Length; i++)
            {
                if (color.Equals(GetColor.textColors[i]))
                {
                    code = (EntryCodes)i;
                    if (!Enum.IsDefined(code.GetType(), code))
                    {
                        throw new Exception("This Color is not defined in EntryCodes");
                    }
                    break;
                }
            }
            return code;
        }
    }
}
