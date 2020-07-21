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

        public event PropertyChangedEventHandler PropertyChanged;

        public List<EntryModel> EntryModels = new List<EntryModel>();

        private List<EntryModel> LongestWords = new List<EntryModel>();

        private void OnChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StringBuilder _text = new StringBuilder();
        public Encoding CurrentEncoding { get; set; } = Encoding.UTF8;
        public async void StartWork()
        {
            bool flag = true;
            int index = 0;
            char[] array = new char[100];
            for (int i = 0; i < _text.Length; i++)
            {
                char nextSymbol = _text[i];
                if (nextSymbol == '<')
                {
                    i += IgnoreHtmlTags(i) - 1;
                    continue;
                }
                else if (char.IsLetterOrDigit(nextSymbol))
                {
                    flag = false;
                    array[index] = _text[i];
                    index++;
                }
                else if (!flag)
                {
                    string word = new string(array);
                    FindLongestWord(word, i);
                    array = new char[100];
                    flag = true;
                    index = 0;                
                }
            }
            GC.Collect();
            EntryModels.AddRange(LongestWords);
            ColorizeEntries();
            IsAnalyzed = true;
            ;
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

        private void FindLongestWord(string word, int startIndex)
        {
            int trueLength = FindTrueLength(word);
            if (LongestWords.Count != 0)
            {
                for (int i = 0; i < LongestWords.Count; i++)
                {
                    if (trueLength > LongestWords[i].Length)
                    {
                        LongestWords.Clear();
                        LongestWords.Add(new EntryModel
                        {
                            StartIndex = startIndex - trueLength,
                            EndIndex = startIndex,
                            textColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
                        });
                        break;
                    }
                    else if (trueLength == LongestWords[i].Length)
                    {
                        LongestWords.Add(new EntryModel
                        {
                            StartIndex = startIndex - trueLength,
                            EndIndex = startIndex ,
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
                    StartIndex = startIndex - trueLength,
                    EndIndex = startIndex,
                    textColor = GetColor.GetColorByCode(EntryCodes.LongestWord)
                });
            }
        }

        private void ColorizeEntries()
        {
            foreach (var entry in EntryModels)
            {
                string startString = $"<span style=\"background:{ColorTranslator.ToHtml(entry.textColor)}\">",
                    endString = $"</span>";
                _text.Insert(entry.StartIndex, startString);
                _text.Insert(entry.EndIndex+startString.Length, endString);
                EntryModel.Offset = startString.Length + endString.Length;
            }
            OnChanged();
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
            OnChanged();
        }

        private void ClearData()
        {
            IsAnalyzed = false;
            LongestWords.Clear();
            EntryModels.Clear();
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
