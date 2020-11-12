using System.IO;
using System.Threading.Tasks;
using TextAnalyzer.Modules.Interfaces;

namespace TextAnalyzer.Modules
{
    public static class Loader
    {
        public async static Task LoadTextFile(ITextContainer textModel, string path)
        {
            
            using (StreamReader streamReader = new StreamReader
                    (
                    new FileStream(path, FileMode.Open, FileAccess.Read), 
                    textModel.TextEncoding
                    )
                )
            {
                string myText = await streamReader.ReadToEndAsync();
                textModel.SetNewText(myText);
            }
        }
        public static async Task SaveTextFile(ITextContainer textModel, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter
                    (
                    new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write),
                    textModel.TextEncoding
                    )
                )
            {
                await streamWriter.WriteAsync(textModel.Text);
            }
        }
    }
}
