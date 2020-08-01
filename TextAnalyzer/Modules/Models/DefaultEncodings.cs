using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer.Modules.Models
{
    public static class DefaultEncodings
    {
        public static ObservableCollection<Encoding> defaultEncodings = new ObservableCollection<Encoding>()
        {
            Encoding.UTF8,
            Encoding.Unicode,
            Encoding.GetEncoding(1251)
        };
    }
}
