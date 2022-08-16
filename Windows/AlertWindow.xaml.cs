using System.Windows;

namespace EGLProjectPathManager
{
    /// <summary>
    /// Interaction logic for AlertWindow.xaml
    /// </summary>
    public partial class AlertWindow : Window
    {
        public AlertWindow(string ErrorMessage)
        {
            InitializeComponent();

            AlertText.Text = ErrorMessage;
        }
    }
}
