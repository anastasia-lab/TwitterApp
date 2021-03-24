using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TwitInterf
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Window, INotifyPropertyChanged
    {
        public LogIn()
        {
            InitializeComponent();
            DataContext = this.textBox.Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }


        //private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{OnPropertyChanged(this.textBox.Text);
        //    if (string.IsNullOrEmpty(this.textBox.Text))
        //    {
        //        this.buttonOk.IsEnabled = false;
        //    }
        //    else
        //    {
        //        this.buttonOk.IsEnabled = true;
        //    }
        //}

        public string Verifier
        {
            //get; set;
            get { return this.textBox.Text; }
            set { this.textBox.Text = value; OnPropertyChanged(this.textBox.Text); }
        }
    }
}