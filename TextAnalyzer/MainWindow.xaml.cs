using Microsoft.Win32;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TextAnalyzer.Modules;
using TextAnalyzer.Modules.Models;
using TextAnalyzer.Modules.ViewModels;
using TextAnalyzer.Modules.View;
using System.Text;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace TextAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextModel _textModel;

        private ObservableCollection<ColorInfo> _colorInfos;

        public string FilePath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            FieldInfo browserInfo = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (browserInfo == null) 
            { 
                return; 
            }
            object objComWebBrowser = browserInfo.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide);
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textModel = TextModel.GetTextModelInstance();
            _colorInfos = Load_Colors();
            HideScriptErrors(MainWebBrowser, true);
            _textModel.TextEncoding = Encoding.UTF8;
            _textModel.TextChanged += GetTextFromModel;
            _textModel.NewColorCreated += NewColorCreated;
            ChooseEncodingMenu.ItemsSource = FileIOEncodings.EncodingList;
            ColorListView.ItemsSource = _colorInfos;
            StackPan.DataContext = _textModel;
            InfoListView.DataContext = _textModel;
        }

        private ObservableCollection<ColorInfo> Load_Colors()
        {
            var result = new ObservableCollection<ColorInfo>();
            EntryCodes entryCodes = new EntryCodes();
            foreach (var meaning in Enum.GetValues(entryCodes.GetType()))
            {
                result.Add(new ColorInfo
                {
                    Mean = GetCode.GetCodeMeaning((EntryCodes)meaning),
                    Name = GetColor.GetColorByCode((EntryCodes)meaning).Name
                });
            }
            return result;
        }
        private void GetTextFromModel()
        {
            Dispatcher?.Invoke(new Action(() => MainWebBrowser.NavigateToString(_textModel.Text)));
        }

        private void NewColorCreated(Color newColor, string colorMeaning)
        {
            ColorInfo newColorInfo = new ColorInfo { Name = ColorTranslator.ToHtml(newColor), Mean = colorMeaning };
            bool AddFlag = true;
            foreach (var colorInfo in _colorInfos)
            {
                if (colorInfo.CompareTo(newColorInfo) == 0)
                {
                    AddFlag = false;
                }
            }
            if (AddFlag)
            {
                Dispatcher?.Invoke(new Action(() => _colorInfos.Add(newColorInfo)));
            }
        }

        private bool OpenFile() 
        {
            bool result = false;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|HTML Files (*.html,*.htm)|*.html*.htm| All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                result = true;
            }
            return result;
        }

        private bool SaveFile()
        {
            bool result = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|HTML Files (*.htm,*.html)|*.htm*.html"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                result = true;
            }
            return result;
        }

        private async void Start_Analyze(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                await _textModel.StartWorkAsync();
            }
            else
            {
                string message;
                message = (_textModel.IsAnalyzed) ? "The text has already been analyzed" : "The text is currently being analyzed";
                TextMessageBox textMessage = new TextMessageBox(message);
                textMessage.Show();
            }
        }

        private void Refresh_Text(object sender, RoutedEventArgs e)
        {
            GetTextFromModel();
        }

        private void Show_Help(object sender, RoutedEventArgs e)
        {
            string messageText = 
                "Welcome to the Text Analyzer!\n" +
                "The program is designed for text analysis,\n" +
                "it highlights the features of the text with differents colors,\n" +
                "and also provides other info about text.\n\n"+
                "Steps to work:\n" +
                "1. Load text file (.txt, .html)\n"+
                "2. Click \"Analyze\" button\n"+
                "3. Wait for the end of the analysis\n"+
                "4. Save result as text file(.txt, .html)\n"+
                "To read various files, it is possible to change I/O file encoding\n"+
                "Standart Encoding - Unicode(UTF-8)\n\n"+
                "©Created by BNTU student Artyom Kolosov";
            TextMessageBox messageBox = new TextMessageBox(messageText);
            messageBox.Show();

        }
        private async void StartFileLoading(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalasing)
            {
                bool openResult = OpenFile();
                if (openResult && FilePath != null)
                {
                    await Loader.LoadTextFile(_textModel, FilePath);
                }
            }
            else
            {
                TextMessageBox textMessage = new TextMessageBox("The text is currently being analyzed");
                textMessage.Show();
            }
        }
        private async void StartFileSaving(object sender, RoutedEventArgs e)
        {
            if (_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                bool openResult = SaveFile();
                if (openResult && FilePath != null)
                {
                    await Loader.SaveTextFile(_textModel, FilePath);
                }
            }
            else
            {
                TextMessageBox textMessage = new TextMessageBox("The text is currently being analyzed");
                textMessage.Show();
            }
        }

        private void MenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            object newEncoding = ((MenuItem)sender).DataContext;
            if (newEncoding is Encoding encoding)
            {
                _textModel.TextEncoding = encoding;
            }
        }

        private void MenuAddEncoding_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowDialog();
            if (messageBox.DialogResult == true)
            {
                Encoding userEncoding = messageBox.UserEncoding;
                if (userEncoding != null)
                {
                    FileIOEncodings.EncodingList.Add(userEncoding);
                }
            }
        }
    }
}
