using System.Windows;
using System.Diagnostics;
namespace EGLProjectPathManager
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void RhyceDevLinkBTN_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rhyce.dev/";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://rhyce.dev/2022/04/14/fix-unreal-projects-not-showing-in-the-epic-games-launcher/";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void GitHubRepoLink_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://github.com/Rhyce/EGL-Project-Path-Manager";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void KoFiLinkBTN_Click(object sender, RoutedEventArgs e)
        {
            var url = "https://ko-fi.com/rhyce";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
