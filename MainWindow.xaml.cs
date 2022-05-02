using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.IO;

namespace EGLProjectPathManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EGLHelper EGL = new EGLHelper();
        public MainWindow()
        {
            InitializeComponent();




            EGL.CreateGSUBackup();

            // Bind Buttons
            AddNewPathBTN.Click += AddNewPathBTN_Click;

            RemovePathButton.Click += RemovePathButton_Click;

            // Check if EGL is running
            if (Process.GetProcessesByName("EpicGamesLauncher").Length > 0)
            {
                ShowAlertWindow("Please close the Epic Games Launcher to avoid potential problems.");       
            }

            // Verify if GameUserSettings.ini exists
            if (!EGL.DoesEGLGUSConfigExist())
            {
                ShowAlertWindow("Could not find GameUserSettings.ini for the Epic Games Launcher.");
                return;
            }

            // Verify if GameUserSettings.ini exists
            if (!EGL.DoesSectionExist("Launcher"))
            {
                ShowAlertWindow("Could not find Launcher section in GameUserSettings.ini");
                return;
            }

            RegenerateList();
        }

        private void RemovePathButton_Click(object sender, RoutedEventArgs e)
        {
            if (PathList.SelectedValue != null)
            {

                EGL.RemoveProjectPath(PathList.SelectedItem.ToString());

                RegenerateList();
                return;
            }
        }

        private void AddNewPathBTN_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog browserDialog = new VistaFolderBrowserDialog
            {
                Multiselect = false,
                Description = "Select the directory where you'll be storing your projects.",
                UseDescriptionForTitle = true, // This applies to the Vista style dialog only, not the old dialog.
            };
            if (browserDialog.ShowDialog() == true)
            {
                if (EGL.DoesUProjectExistInFolder(browserDialog.SelectedPath))
                {
                    ShowAlertWindow("This is a game project file. Try the parent folder instead.");
                    return;
                }
                else if(EGL.DoesDirectoryAlreadyExistInIni(browserDialog.SelectedPath))
                {
                    ShowAlertWindow("You've already added this folder. Maybe try another.");
                    return;
                }
                else
                {
                    // As long as everything is fine. Add it to the .ini.
                    EGL.AppendDirectoryToIni(browserDialog.SelectedPath);

                    RegenerateList();

                    return;
                }
            }
        }

        void RegenerateList()
        {
            PathList.Items.Clear();

            List<string> paths = EGL.GetProjectPaths();

            foreach (string p in paths)
            {
                PathList.Items.Add(p);
            }
        }

        private void AboutBTN_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        void ShowAlertWindow(string message)
        {
            AlertWindow alertWindow = new AlertWindow(message);
            alertWindow.ShowDialog();
        }
    }
}
