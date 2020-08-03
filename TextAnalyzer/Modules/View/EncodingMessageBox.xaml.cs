using System;
using System.Text;
using System.Windows;

namespace TextAnalyzer.Modules.View
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            UserEncoding = GetEncoding(EncodeBox.Text);
        }

        private Encoding GetEncoding(string data)
        {
            Encoding encoding;
            try
            {
                encoding = int.TryParse(data, out int encodingCodePage) ? Encoding.GetEncoding(encodingCodePage) : Encoding.GetEncoding(data);
            }
            catch (Exception)
            {
                encoding = null;
            }
            return encoding;
        }
        public Encoding UserEncoding { get; private set; }
    }
}
