using System.Windows;

namespace OnlineCinema.App
{
    public partial class InputDialog : Window
    {
        public string Input1 => TbInput1.Text;
        public string Input2 => TbInput2.Text;

        public InputDialog(string title, string label1, string label2)
        {
            InitializeComponent();
            Title = title;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}