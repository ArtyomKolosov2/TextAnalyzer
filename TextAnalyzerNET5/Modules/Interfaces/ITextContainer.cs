using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer.Modules.Interfaces
{
    public interface ITextContainer
    {
        string Text { get; }
        Encoding TextEncoding { get; set; }
        void SetNewText(string text);
    }
}
