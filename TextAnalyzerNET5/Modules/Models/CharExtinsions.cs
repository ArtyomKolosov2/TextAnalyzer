using System;
using System.Linq;

namespace TextAnalyzer.Modules.Models
{
    public static class CharExtinsions
    {
        public static int FindTrueLength(this char[] word)
        {
            int result = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == '\0')
                {
                    break;
                }
                result++;
            }
            return result;
        }

        public static void CopyDataFromCharArray(this char[] newArray, char[] oldArray)
        {
            if (oldArray.Length > newArray.Length)
            {
                throw new InvalidOperationException("Size Error: New array must be bigger than old!");
            }
            for (int i = 0; i < oldArray.Length; i++)
            {
                newArray[i] = oldArray[i];
            }
        }

        public static void ClearCharArray(this char[] array, int clearLength)
        {
            for (int i = 0; i < clearLength; i++)
            {
                array[i] = '\0';
            }
        }

        public static bool IsWord(this char[] word, int trueLength)
        {
            bool result = true;
            for (int i = 0; i < trueLength; i++)
            {
                if (!char.IsLetter(word[i]))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static bool IsLetterOrDigitMod(this char c)
        {
            char[] symbols = new char[] { '-', '\'', '"', '’' };
            return char.IsLetterOrDigit(c) || symbols.Contains(c);
        }
    }
}
