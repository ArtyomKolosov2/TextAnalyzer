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
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                string myText = Encoding.Default.GetString(bytes);
                textModel.Text = myText;
            }
            
        }
    }
}
