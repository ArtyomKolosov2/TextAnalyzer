using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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


        private List<EntryModel> _entryModels = new List<EntryModel>();

        private List<EntryModel> _longestWords = new List<EntryModel>();

        private List<EntryModelNum> _biggestNums = new List<EntryModelNum>();

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
                    Task t1 = Task.Run(() => FindLongestWord(array, i));
                    Task t2 = Task.Run(() => FindBiggetsNum(array, i));
                    Task t3 = Task.Run(() => Test(array, i, new char[] { 'а', 'ы', 'у', 'е', 'о', 'э', 'я', 'и', 'ю', 'ё' }, EntryCodes.OnlyVowel));
                    Task t4 = Task.Run(() => Test(array, i, new char[] { 'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' }, EntryCodes.OnlyConsonat));

                    await Task.WhenAll(new[] { t1, t2, t3, t4 });
                    ClearCharArray(array);
                    ReadyPercent = i;
                    flag = true;
                    index = 0;
                }
            }
            IsAnalasing = false;
            GC.Collect();
            _entryModels.AddRange(_longestWords);
            _entryModels.AddRange(_biggestNums);
            ColorizeEntries();
            IsAnalyzed = true;
            ;
        }

        private void ClearCharArray(char[] array)
        {
            int index = 0;
            while (array[index] != '\0')
            {
                array[index] = '\0';
                index++;
            }
        }
        private bool IsLetterOrDigitMod(char c)
        {
            char[] symbols = new char[] { '-', '\'', '"' };
            return char.IsLetterOrDigit(c) || symbols.Contains(c);
        }

        private int FindTrueLength(string word)
        {
            int result = 0;
            for (int i = 0; word[i] != '\0'; i++)
            {
                result++;
            }
            return result;
        }

        private int FindTrueLength(char[] word)
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

        private void Test(char[] word, int endIndex, char[] symbols, EntryCodes codes)
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
                _entryModels.Add(new EntryModel
                {
                    StartIndex = endIndex - trueLength,
                    EndIndex = endIndex,
                    textColor = GetColor.GetColorByCode(codes)
                });
            }
        }

        private void FindLongestWord(char[] word, int endIndex)
        {
            int trueLength = FindTrueLength(word);
            if (_longestWords.Count != 0)
            {
                for (int i = 0; i < _longestWords.Count; i++)
                {
                    if (trueLength >= _longestWords[i].Length)
                    {
                        if (trueLength > _longestWords[i].Length)
                        {
                            _longestWords.Clear();
                        }
                        _longestWords.Add(new EntryModel
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
                _longestWords.Add(new EntryModel
                {
                    StartIndex = endIndex - trueLength,
                    EndIndex = endIndex,
                    textColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
                });
            }
        }

        private void FindBiggetsNum(char[] word, int endIndex)
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
                if (_biggestNums.Count != 0)
                {
                    for (int i = 0; i < _biggestNums.Count; i++)
                    {
                        if (newNum >= _biggestNums[i].Num)
                        {
                            if (newNum > _biggestNums[i].Num)
                            {
                                _biggestNums.Clear();
                            }
                            _biggestNums.Add(new EntryModelNum
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
                    _biggestNums.Add(new EntryModelNum
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
            _entryModels.Sort(new SortByStartIndex());
            foreach (var entry in _entryModels)
            {
                string startString = $"<span style=\"background:{ColorTranslator.ToHtml(entry.textColor)}\">",
                    endString = $"</span>";
                _text.Insert(entry.StartIndex, startString);
                _text.Insert(entry.EndIndex + startString.Length, endString);
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
            while (nextSymbol != '>')
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
            _longestWords.Clear();
            _biggestNums.Clear();
            _entryModels.Clear();
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
