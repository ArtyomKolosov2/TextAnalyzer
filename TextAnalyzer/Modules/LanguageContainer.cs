namespace TextAnalyzer.Modules
{
    public static class LanguageGetter
    {
        private static LanguageContainer[] _languages = new LanguageContainer[]
        {
            new LanguageContainer
            (
                name:"Russian",
                vowels:new char[]{ 'а', 'ы', 'у', 'е', 'о', 'э', 'я', 'и', 'ю', 'ё' },
                consonat:new char[]{ 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' },
                start:'а',
                end:'я'
            ),
            new LanguageContainer
            (
                name:"English",
                vowels:new char[]{'a','e','i','u','y','o'},
                consonat:new char[]{'b','c','d','f','g','h','j','k','l','m','n','p','q','r','s','t','v','w','x','z' },
                start:'a',
                end:'z'
            )
        };
        public static LanguageContainer GetLanguageContainer(char c)
        {
            LanguageContainer result = null;
            c = char.ToLower(c);
            foreach (var language in _languages)
            {
                if (c >= language.UnicodeStart && c <= language.UnicodeStop)
                {
                    result = language;
                    break;
                }
            }
            return result;
        }

        public static char [] GetCharArray(char c, EntryCodes code)
        {
            char[] result = null;
            LanguageContainer language = GetLanguageContainer(c);
            if (language != null)
            {
                result = (code.Equals(EntryCodes.OnlyVowel)) ? language.VowelsSymbols : language.ConsonatSymbols;
            }
            return result;
        }
    }
    public class LanguageContainer
    {
        public LanguageContainer
            (
                string name, 
                char [] vowels, 
                char [] consonat,
                int start,
                int end
            )
        {
            Language = name;
            VowelsSymbols = vowels;
            ConsonatSymbols = consonat;
            UnicodeStart = start;
            UnicodeStop = end;
        }
        public char[] VowelsSymbols { get; private set; }
        public char[] ConsonatSymbols { get; private set; }

        public int UnicodeStart { get;}
        public int UnicodeStop { get;}
        public string Language { get; }
    }
}
