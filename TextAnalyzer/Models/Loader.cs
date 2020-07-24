using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer.Models
{
    public class Loader
    {
        public void LoadFile(TextModel textModel, string path)
        {
            
            using (StreamReader streamReader = new StreamReader
                    (
                    new FileStream(path, FileMode.Open, FileAccess.Read), 
                    textModel.CurrentEncoding
                    )
                )
            {
                string myText = streamReader.ReadToEnd();
                textModel.SetNewText(myText);
            }
        }
        public void SaveFile(TextModel textModel, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter
                    (
                    new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write),
                    textModel.CurrentEncoding
                    )
                )
            {
                streamWriter.Write(textModel.Text);
            }
        }
    }
}
