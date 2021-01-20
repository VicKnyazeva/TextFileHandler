using System;
using System.Windows;

namespace TextFileHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (this.DataContext == null)
            {
                this.DataContext = new MainWindowModel();
            }
        }

        private MainWindowModel Model
        {
            get { return this.DataContext as MainWindowModel; }
        }

        private void OpenFileToProcess_Click(object sender, RoutedEventArgs e)
        {
            this.Model.OpenFileForProcessing();
        }

        private void OpenFileToSave_Click(object sender, RoutedEventArgs e)
        {
            this.Model.OpenFileToSave();
        }

        private void ProcessFile_Click(object sender, RoutedEventArgs e)
        {
            ProcessFile();
        }

        private void ProcessFile()
        {
            Model.ProcessFile();
        }
    }
}
