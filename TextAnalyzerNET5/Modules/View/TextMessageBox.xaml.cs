using System.Windows;

namespace TextAnalyzer.Modules.View
{
    /// <summary>
    /// Логика взаимодействия для TextMessageBox.xaml
    /// </summary>
    public partial class TextMessageBox : Window
    {
        public string TextMessage { get; set; }

        public TextMessageBox(int width, int height, string textMessage) : this(textMessage)
        {
            this.Width = width;
            this.Height = height;
        }
        public TextMessageBox(string textMessage) : this()
        {
            TextMessage = textMessage;
            mainTextBlock.Text = TextMessage;
        }
        public TextMessageBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
