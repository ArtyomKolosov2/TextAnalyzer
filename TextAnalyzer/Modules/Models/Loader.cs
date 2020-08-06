using System.IO;
using TextAnalyzer.Modules.ViewModels;

namespace TextAnalyzer.Modules
{
    public static class Loader
    {
        public async static void LoadFile(TextModel textModel, string path)
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
        public static async void SaveFile(TextModel textModel, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter
                    (
                    new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write),
                    textModel.CurrentEncoding
                    )
                )
            {
                await streamWriter.WriteAsync(textModel.Text);
            }
        }
    }
}
