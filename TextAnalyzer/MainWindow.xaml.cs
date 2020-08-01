using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TextAnalyzer.Modules;
using TextAnalyzer.Modules.Models;
using TextAnalyzer.Modules.View;

namespace TextAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextModel _textModel;
        public string FilePath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var browserInfo = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (browserInfo == null) 
            { 
                return; 
            }
            var objComWebBrowser = browserInfo.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide);
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textModel = new TextModel();
            Load_Colors();
            HideScriptErrors(MainWebBrowser, true);
            DefaultEncodingMenu.ItemsSource = DefaultEncodings.defaultEncodings;
            _textModel.TextChanged += TextModelChanged;
            StackPan.DataContext = _textModel;
            InfoListView.DataContext = _textModel;
            
            
        }

        private void Load_Colors()
        {
            List<ColorInfo> colorInfos = new List<ColorInfo>(GetColor.textColors.Length);
            EntryCodes entryCodes = new EntryCodes();
            foreach (var color in Enum.GetValues(entryCodes.GetType()))
            {
                colorInfos.Add(new ColorInfo
                {
                    Mean = color.ToString(),
                    Name = GetColor.GetColorByCode((EntryCodes)color).Name
                });
            }
            ColorListView.ItemsSource = colorInfos;
        }
        private void TextModelChanged(string newText)
        {
            MainWebBrowser.NavigateToString(newText);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalasing)
            {
                bool openResult = OpenFile();
                if (openResult && FilePath != null)
                {
                    Loader.LoadFile(_textModel, FilePath);
                }
            }
            else
            {
                MessageBox.Show("The text is currently being analyzed");
            }  
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                await Task.Run(() => {_textModel.StartWork(this); });
            }
            else
            {
                MessageBox.Show("The text has already been analyzed");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                bool openResult = SaveFile();
                if (openResult && FilePath != null)
                {
                    Loader.SaveFile(_textModel, FilePath);
                }
            }
            else
            {
                MessageBox.Show("The text is currently being analyzed");
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            ;
        }

        private void MenuAddEncoding_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox messageBox = new CustomMessageBox();
            messageBox.ShowDialog();
        }
    }
}
