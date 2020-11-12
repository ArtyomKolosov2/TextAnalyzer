namespace TextAnalyzer.Modules.Models
{
    public static class StringExtinsions
    {
        public static int FindTrueLength(this string word)
        {
            int result = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (!char.IsControl(word[i]))
                {
                    result++;
                }
            }
            return result;
        }
    }
}
