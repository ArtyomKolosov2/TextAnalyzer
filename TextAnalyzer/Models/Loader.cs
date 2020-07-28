using System.IO;

namespace TextAnalyzer.Models
{
    public class Loader
    {
        public async void LoadFile(TextModel textModel, string path)
        {
            
            using (StreamReader streamReader = new StreamReader
                    (
                    new FileStream(path, FileMode.Open, FileAccess.Read), 
                    textModel.CurrentEncoding
                    )
                )
            {
                string myText = await streamReader.ReadToEndAsync();
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
