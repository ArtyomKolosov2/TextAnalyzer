using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TextAnalyzer.Modules.ViewModels
{
    public static class FileIOEncodings
    {
        public static ObservableCollection<Encoding> encodingList = new ObservableCollection<Encoding>()
        {
            Encoding.UTF8,
            Encoding.Unicode,
            Encoding.GetEncoding(1251),
            Encoding.GetEncoding("cp866"),
            Encoding.GetEncoding("koi8-r")
        };
    }
}
