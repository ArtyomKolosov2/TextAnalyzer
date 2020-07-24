using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextAnalyzer.Models
{
    public class TextModel : INotifyPropertyChanged
    {
        public bool IsAnalyzed { get; private set; } = false;

        public bool IsAnalasing { get; private set; } = false;

        private int _readyPercent = 0;

        private int _maxLen = 0;
        public int ReadyPercent
        {
            get { return _readyPercent; }
            set 
            { 
                _readyPercent = value;
                OnChanged();
            }
        }

        public int MaxLen
        {
            get { return _maxLen; }
            set
            {
                _maxLen = value;
                OnChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler TextChanged;


        private List<EntryModel> EntryModels = new List<EntryModel>();

        private List<EntryModel> LongestWords = new List<EntryModel>();

        private List<EntryModelNum> BiggestNums = new List<EntryModelNum>();

        private void OnChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnTextChanged([CallerMemberName]string propertyName = null)
        {
            TextChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StringBuilder _text = new StringBuilder();
        public Encoding CurrentEncoding { get; set; } = Encoding.UTF8;
        public async void StartWork()
        {
            bool flag = true;
            int index = 0;
            char[] array = new char[100];
            MaxLen = _text.Length;
            IsAnalasing = true;
            for (int i = 0; i < _text.Length; i++)
            {
                char nextSymbol = _text[i];
                if (nextSymbol == '<')
                {
                    i += IgnoreHtmlTags(i) - 1;
                }
                else if (IsLetterOrDigitMod(nextSymbol))
                {
                    flag = false;
                    array[index] = _text[i];
                    index++;
                }
                else if (!flag)
                {
                    string word = new string(array);
                    Task t1 = Task.Run( () => FindLongestWord(word, i));
                    Task t2 = Task.Run( () => FindBiggetsNum(word, i));
                    Task t3 = Task.Run ( () => Test(word, i, new char[] { 'а', 'ы', 'у', 'е', 'о', 'э', 'я', 'и', 'ю', 'ё' }, EntryCodes.OnlyVowel));
                    Task t4 = Task.Run ( () => Test(word, i, new char[] { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' }, EntryCodes.OnlyConsonat));
                    await Task.WhenAll(new [] { t1, t2, t3, t4 });
                    array = new char[100];
                    ReadyPercent = i;
                    flag = true;
                    index = 0;                
                }
            }
            IsAnalasing = false;
            GC.Collect();
            EntryModels.AddRange(LongestWords);
            EntryModels.AddRange(BiggestNums);
            ColorizeEntries();
            IsAnalyzed = true;
            ;
        }

        private bool IsLetterOrDigitMod(char c)
        {
            char[] symbols = new char[] { '-', '\'', '"'};
            return char.IsLetterOrDigit(c) || symbols.Contains(c);
        }

        private int FindTrueLength(string word)
        {
            int result = 0;
            for (int i = 0; word[i]!='\0'; i++)
            {
                result++;
            }
            return result;
        }

        private int FindTrueLength(char [] word)
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

        private void Test(string word, int endIndex, char[] symbols, EntryCodes codes)
        {
            int trueLength = FindTrueLength(word);
            bool result = true;
            for (int i = 0; i < trueLength; i++)
            {
                if (!symbols.Contains(char.ToLower(word[i])))
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                EntryModels.Add(new EntryModel
                {
                    StartIndex = endIndex - trueLength,
                    EndIndex = endIndex,
                    textColor = GetColor.GetColorByCode(codes)
                });
            }
        }
 
        private void FindLongestWord(string word, int endIndex)
        {
            int trueLength = FindTrueLength(word);
            if (LongestWords.Count != 0)
            {
                for (int i = 0; i < LongestWords.Count; i++)
                {
                    if (trueLength >= LongestWords[i].Length)
                    {
                        if (trueLength > LongestWords[i].Length)
                        {
                            LongestWords.Clear(); 
                        }
                        LongestWords.Add(new EntryModel
                        {
                            StartIndex = endIndex - trueLength,
                            EndIndex = endIndex,
                            textColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
                        });
                        break;
                    }
                }
            }
            else
            {
                LongestWords.Add(new EntryModel
                {
                    StartIndex = endIndex - trueLength,
                    EndIndex = endIndex,
                    textColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
                });
            }
        }

        private void FindBiggetsNum(string word, int endIndex)
        {
            int trueLength = FindTrueLength(word);
            char[] numPart = new char[trueLength];
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsDigit(word[i]))
                {
                    numPart[i] = word[i];
                }
                else { break; }
            }
            int NumTrueLength = FindTrueLength(numPart);
            if (NumTrueLength != 0)
            {
                long newNum = long.Parse(new string(numPart));
                if (BiggestNums.Count != 0) {
                    for (int i = 0; i < BiggestNums.Count; i++)
                    {
                        if (newNum >= BiggestNums[i].Num)
                        {
                            if (newNum > BiggestNums[i].Num)
                            {
                                BiggestNums.Clear();
                            }
                            BiggestNums.Add(new EntryModelNum
                            {
                                StartIndex = endIndex - trueLength,
                                EndIndex = endIndex - (trueLength - NumTrueLength),
                                Num = newNum,
                                textColor = GetColor.GetColorByCode(EntryCodes.LongestNumber)
                            });
                            break;
                        }
                    }
                }
                else
                {
                    BiggestNums.Add(new EntryModelNum
                    {
                        StartIndex = endIndex - trueLength,
                        EndIndex = endIndex - (trueLength - NumTrueLength),
                        Num = newNum,
                        textColor = GetColor.GetColorByCode(EntryCodes.LongestNumber)
                    });
                }
            }
        }

        private void ColorizeEntries()
        {
            EntryModels.Sort(new SortByStartIndex());
            foreach (var entry in EntryModels)
            {
                string startString = $"<span style=\"background:{ColorTranslator.ToHtml(entry.textColor)}\">",
                    endString = $"</span>";
                _text.Insert(entry.StartIndex, startString);
                _text.Insert(entry.EndIndex+startString.Length, endString);
                EntryModel.Offset += startString.Length + endString.Length;
            }
            OnTextChanged();
        }

        public int IgnoreHtmlTags(int startIndex)
        {
            int overgoCheckIndex = 100,
                        i = startIndex,
                        result = 0;
            char nextSymbol = _text[i];
            while  (nextSymbol != '>')
            {
                if ((i - startIndex) > overgoCheckIndex)
                {
                    result = 1;
                    break;
                }
                nextSymbol = _text[i++];
                result++;
            }
            return result;
        }

        public void SetNewText(string value)
        {
            ClearData();
            _text.Append($"<!DOCTYPE html><html><meta http-equiv='Content-Type'content='text/html;charset={CurrentEncoding.WebName}'><head></head><body>{value}</body></html>");
            ReplaceSpecialSymbols();
            OnTextChanged();
        }

        private void ClearData()
        {
            IsAnalyzed = false;
            LongestWords.Clear();
            BiggestNums.Clear();
            EntryModels.Clear();
            MaxLen = 0;
            ReadyPercent = 0;
            EntryModel.Offset = 0;
            _text.Clear();
            GC.Collect();
        }

        public string Text
        {
            get { return _text.ToString(); }
        } 

        private void ReplaceSpecialSymbols()
        {
             _text.Replace("\n", "<br>");
        }
    }
}
