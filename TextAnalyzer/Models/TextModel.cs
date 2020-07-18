using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextAnalyzer.Models
{
    public class TextModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StringBuilder _text = new StringBuilder();
        public Encoding CurrentEncoding { get; set; } = Encoding.UTF8;

        private List<string> GetVs = new List<string>(); 
        public async void StartWork()
        {
            GetVs.Clear();
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
                    GetVs.Add(new string(array));
                    array = new char[100];
                    flag = true;
                    index = 0;                
                }
            }
            GC.Collect();
            ;
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
        public string Text
        {
            get { return _text.ToString(); }
            set 
            {
                _text.Clear();
                _text.Append($"<!DOCTYPE html><html><meta http-equiv='Content-Type'content='text/html;charset={CurrentEncoding.WebName}'><head></head><body>{value}</body></html>");
                ReplaceSpecialSymbols();
                OnPropertyChanged();
            }
        } 

        private void ReplaceSpecialSymbols()
        {
             _text.Replace("\n", "<br>");
        }
    }
}
