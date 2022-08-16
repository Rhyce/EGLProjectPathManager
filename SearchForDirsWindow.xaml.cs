
using System.Windows;

using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EGLProjectPathManager
{

    /// <summary>
    /// Interaction logic for SearchForDirsWindow.xaml
    /// </summary>
    public partial class SearchForDirsWindow : Window
    {
        static bool bIsSearchingForDirs = false;
        static Task searchTask;
        static CancellationTokenSource searchTaskTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = searchTaskTokenSource.Token;

        public SearchForDirsWindow()
        {
            InitializeComponent();

            

            SearchBTN.Click += SearchBTN_Click;
        }

        private void SearchBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!bIsSearchingForDirs)
            {
                StartSearch();
            }
            else
            {
                StopSearch();
            }
        }

        private void StartSearch()
        {
            bIsSearchingForDirs = true;
            SearchBTN.Content = "Cancel";

            try
            {
                searchTask = Task.Run(() =>
                {

                    foreach (string s in Directory.GetLogicalDrives())
                    {

                        foreach (string f in Directory.EnumerateFiles(s, "", SearchOption.AllDirectories))
                        {
                            Dispatcher.Invoke(() =>
                        {
                            OutputLog.AppendText("\n" + f);

                            OutputLog.ScrollToEnd();
                        });

                            if (cancelToken.IsCancellationRequested)
                            {
                                Debug.WriteLine("Cancel Requested");
                                Dispatcher.Invoke(() =>
                            {
                                OutputLog.AppendText("\n" + "Search cancelled.");

                                OutputLog.ScrollToEnd();

                            });

                            }
                        }


                    }
             

                
            }, cancelToken);

        }
        catch{}

            searchTask.ContinueWith(t => {
                this.Dispatcher.Invoke(() =>
                {
                    OutputLog.AppendText("\n\nDone");
                    bIsSearchingForDirs = false;
                    SearchBTN.Content = "Search";


                    searchTask.Dispose();
                });
            });

        }

        private void StopSearch()
        {
            
            bIsSearchingForDirs = false;
            SearchBTN.Content = "Search";

            searchTaskTokenSource.Cancel();
            
        }
    }
}
