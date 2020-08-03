﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer.Modules
{
    public class TextModel : INotifyPropertyChanged
    {
        private int _readyPercent = 0;

        private int _symbolsAmount = 0;

        private int _wordsAmount = 0;

        private string _currentEncodingName;

        private Encoding _currentEncoding;
        public int ReadyPercent
        {
            get { return _readyPercent; }
            set
            {
                _readyPercent = value;
                OnPropertyChangedEvent();
            }
        }

        public int SymbolsAmount
        {
            get { return _symbolsAmount; }
            set
            {
                _symbolsAmount = value;
                OnPropertyChangedEvent();
            }
        }
        public int WordsAmount
        {
            get { return _wordsAmount; }
            set
            {
                _wordsAmount = value;
                OnPropertyChangedEvent();
            }
        }

        public string Text
        {
            get { return $"<!DOCTYPE html><html><meta http-equiv='Content-Type'content='text/html;charset={CurrentEncoding.WebName}'><head></head><body>{_text.ToString()}</body></html>"; }
        }

        public Encoding CurrentEncoding 
        {
            get {return _currentEncoding; }
            set 
            {
                _currentEncoding = value;
                CurrentEncodingName = _currentEncoding.EncodingName;
                OnPropertyChangedEvent();
            } 
        }
     
        public string CurrentEncodingName
        {
            get { return _currentEncodingName; }
            set 
            {
                _currentEncodingName = value;
                OnPropertyChangedEvent();
            }
        }

        public bool IsAnalyzed { get; private set; } = false;
        public bool IsAnalasing { get; private set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void TextChangedEventHandler(string text);

        public event TextChangedEventHandler TextChanged;


        private List<EntryModel> _entryModels = new List<EntryModel>();

        private List<EntryModel> _longestWords = new List<EntryModel>();

        private List<EntryModelNum> _biggestNums = new List<EntryModelNum>();

        private void OnPropertyChangedEvent([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StringBuilder _text = new StringBuilder();
        public async void StartWork(MainWindow main)
        {
            bool flag = true;
            int index = 0;
            char[] array = new char[100];
            double onePercent = 100.0 / _text.Length;
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
                    Task t3 = Task.Run(() => FindSymbols(array, i, EntryCodes.OnlyVowel));
                    Task t4 = Task.Run(() => FindSymbols(array, i, EntryCodes.OnlyConsonat));
                    ReadyPercent = (int)(i * onePercent);
                    await Task.WhenAll(new[] { t1, t2, t3, t4 });
                    ClearCharArray(array);
                    WordsAmount++;
                    flag = true;
                    index = 0;
                }
                
            }
            _entryModels.AddRange(_longestWords);
            _entryModels.AddRange(_biggestNums);
            ColorizeEntries(main);
            ReadyPercent = 100;
            IsAnalyzed = true;
            IsAnalasing = false;
            GC.Collect();
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
            char[] symbols = new char[] { '-', '\'', '"', '’' };
            return char.IsLetterOrDigit(c) || symbols.Contains(c);
        }

        private int FindTrueLength(string word)
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

        private void FindSymbols(char[] word, int endIndex, EntryCodes codes)
        {
            int trueLength = FindTrueLength(word);
            char[] symbols;
            bool result = true;
            for (int i = 0; i < trueLength; i++)
            {
                symbols = LanguageGetter.GetSymbolsArray(word[i], codes);
                if (symbols == null || !symbols.Contains(char.ToLower(word[i])))
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
                    TextColor = GetColor.GetColorByCode(codes)
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
                            TextColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
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
                    TextColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
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
            int numTrueLength = FindTrueLength(numPart);
            if (numTrueLength != 0)
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
                                EndIndex = endIndex - (trueLength - numTrueLength),
                                Num = newNum,
                                TextColor = GetColor.GetColorByCode(EntryCodes.LargestNumber)
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
                        EndIndex = endIndex - (trueLength - numTrueLength),
                        Num = newNum,
                        TextColor = GetColor.GetColorByCode(EntryCodes.LargestNumber)
                    });
                }
            }
        }

        private void ColorizeEntries(MainWindow main)
        {
            _entryModels.Sort(new SortByStartIndex());
            foreach (var entry in _entryModels)
            {
                string startString = $"<span style=\"background:{ColorTranslator.ToHtml(entry.TextColor)}\">",
                    endString = $"</span>";
                _text.Insert(entry.StartIndex, startString);
                _text.Insert(entry.EndIndex + startString.Length, endString);
                EntryModel.Offset += startString.Length + endString.Length;
            }
            main.Dispatcher?.Invoke(new Action(() => main.MainWebBrowser.NavigateToString(Text)));
        }

        public int IgnoreHtmlTags(int startIndex)
        {
            int overgoCheckIndex = 300,
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
            int amount = FindTrueLength(value);
            _text.Append(value);
            ReplaceSpecialSymbols();
            SymbolsAmount = amount;
            TextChanged?.Invoke(Text);
        }

        private void ClearData()
        {
            IsAnalyzed = false;
            _longestWords.Clear();
            _biggestNums.Clear();
            _entryModels.Clear();
            ReadyPercent = 0;
            WordsAmount = 0;
            EntryModel.Offset = 0;
            _text.Clear();
            GC.Collect();
        }
        private void ReplaceSpecialSymbols()
        {
            _text.Replace("\n", "<br>");
        }
    }
}
