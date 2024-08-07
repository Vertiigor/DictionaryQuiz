using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace DictionaryQuiz
{
    /// <summary>
    /// Interaction logic for DeveloperInfoWindow.xaml
    /// </summary>
    public partial class DeveloperInfoWindow : Window
    {
        public DeveloperInfoWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
