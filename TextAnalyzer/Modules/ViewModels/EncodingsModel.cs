using System.Collections.ObjectModel;
using System.Text;

namespace TextAnalyzer.Modules.ViewModels
{
    public static class FileIOEncodings
    {
        public static ObservableCollection<Encoding> EncodingList { get; } = new ObservableCollection<Encoding>()
        {
            Encoding.UTF8,
            Encoding.Unicode,
            Encoding.GetEncoding("windows-1251"),
            Encoding.GetEncoding("cp866"),
            Encoding.GetEncoding("koi8-r")
        };
    }
}
