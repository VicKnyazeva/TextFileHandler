using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace TextFileHandler
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FileToProcess
        {
            get { return this._FileToProcess; }
            private set
            {
                if (this._FileToProcess == value)
                    return;

                this._FileToProcess = value;
                this.RaisePropertyChanged(nameof(FileToProcess));
                this.RaisePropertyChanged(nameof(CanProcess));
            }
        }
        private string _FileToProcess;


        internal void OpenFileForProcessing()
        {
            // Configure open file dialog box

            OpenFileDialog dlg = new OpenFileDialog
            {
                FileName = "",                         // Default file name
                DefaultExt = ".txt",                   // Default file extension
                Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FileToProcess = dlg.FileName;
            }
        }

        public bool IsBusy
        {
            get { return this._IsBusy; }
            private set
            {
                if (this._IsBusy == value) return;
                this._IsBusy = value;
                this.RaisePropertyChanged(nameof(IsBusy));
            }
        }
        private bool _IsBusy;

        internal void ProcessFile()
        {
            try
            {
                this.IsBusy = true;
                TextFileProcessor p = new TextFileProcessor
                {
                    minWordLength = this.MinSimbolsCount,
                    removeEmptyLines = this.RemoveEmptyLines,
                    removePunctuation = this.RemovePunctuation
                };
                var result = p.ProcessFile(this.FileToProcess, this.FileToSave);

                MessageBoxImage icon = result.Success ? MessageBoxImage.Information : MessageBoxImage.Error;
                MessageBox.Show(result.Message, "Text File Processor", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public string FileToSave
        {
            get { return this._FileToSave; }
            private set
            {
                if (this._FileToSave == value)
                    return;

                this._FileToSave = value;
                this.RaisePropertyChanged(nameof(FileToSave));
                this.RaisePropertyChanged(nameof(CanProcess));
            }
        }
        private string _FileToSave;

        internal void OpenFileToSave()
        {
            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "NewDocument",              // Default file name
                DefaultExt = ".txt",                   // Default file extension
                Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                FileToSave = dlg.FileName;
            }
        }

        public bool RemovePunctuation
        {
            get { return this._RemovePunctuation; }
            set
            {
                if (this._RemovePunctuation == value)
                    return;

                this._RemovePunctuation = value;
                this.RaisePropertyChanged(nameof(RemovePunctuation));
            }
        }
        private bool _RemovePunctuation;

        public bool RemoveEmptyLines
        {
            get { return this._RemoveEmptyLines; }
            set
            {
                if (this._RemoveEmptyLines == value)
                    return;

                this._RemoveEmptyLines = value;
                this.RaisePropertyChanged(nameof(RemoveEmptyLines));
            }
        }
        private bool _RemoveEmptyLines;

        public int MinSimbolsCount
        {
            get { return this._MinSimbolsCount; }
            set
            {
                if (this._MinSimbolsCount == value)
                    return;

                this._MinSimbolsCount = value;
                this.RaisePropertyChanged(nameof(MinSimbolsCount));
            }
        }
        private int _MinSimbolsCount;

        public bool CanProcess
        {
            get
            {
                return
                    this.FileToProcess != null && this.FileToProcess.Length > 0 &&
                    this.FileToSave != null && this.FileToSave.Length > 0;
            }
        }
    }
}
