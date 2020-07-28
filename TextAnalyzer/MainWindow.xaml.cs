using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextAnalyzer.Models;

namespace TextAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextModel _textModel;
        public string FilePath { get; set; }

        Loader fileLoader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textModel = new TextModel();
            fileLoader = new Loader();
            _textModel.TextChanged += TextModelChanged;
            StackPan.DataContext = _textModel;
            InfoListView.DataContext = _textModel;
            ColorListView.ItemsSource = typeof(Colors).GetProperties();
            
        }

        private void TextModelChanged(string newText)
        {
            MainWebBrowser.NavigateToString(newText);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool openResult = OpenFile();
            if (openResult && FilePath != null)
            {
                fileLoader.LoadFile(_textModel, FilePath);   
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

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                _textModel.StartWork();
            }
            else
            {
                MessageBox.Show("Text Already Analyzed!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (_textModel.IsAnalyzed && !_textModel.IsAnalasing)
            {
                bool openResult = SaveFile();
                if (openResult && FilePath != null)
                {
                    fileLoader.SaveFile(_textModel, FilePath);
                }
            }
            else
            {
                MessageBox.Show("Text still Analysing!");
            }
        }
    }
}
