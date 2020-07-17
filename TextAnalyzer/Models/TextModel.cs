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
        public async void StartWork()
        {
            Random random = new Random();
            _text.Insert(random.Next(150, 500), "<span style = \"background:#FFFFCC\" > Обычный текст.</span>");
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
