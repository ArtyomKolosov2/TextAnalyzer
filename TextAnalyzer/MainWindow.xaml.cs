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
        private string filePath;

        TextModel _textModel;

        Loader fileLoader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textModel = new TextModel();
            fileLoader = new Loader();
            _textModel.PropertyChanged += TextModelChanged;
            ColorListView.ItemsSource = typeof(Colors).GetProperties();
            
        }

        private void TextModelChanged(object sender, PropertyChangedEventArgs e)
        {
            MainWebBrowser.NavigateToString(_textModel.Text);
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

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!_textModel.IsAnalyzed)
            {

                _textModel.StartWork();
            }
            else
            {
                MessageBox.Show("Text Already Analyzed!");
            }
        }
    }
}
