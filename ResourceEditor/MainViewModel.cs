using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using ResourceEditor.Models;
using MessageBox = System.Windows.MessageBox;

namespace ResourceEditor
{
    public class MainViewModel:ViewModelBase
    {
        private string _folderName;

        public string FolderName
        {
            get => _folderName;

            set
            {
                _folderName = value;
                RaisePropertyChanged();
            }
        }

        private Files _files;
        public Files Files
        {
            get => _files;
            private set
            {
                _files=value;
                RaisePropertyChanged();
            }
        }

        public ICommand ChooseFolderCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public MainViewModel()
        {
            _files = new Files();
            ChooseFolderCommand = new DelegateCommand(ChooseFolderMethod);
            SaveCommand = new DelegateCommand(SaveMethod, ()=>Files.ListNotEmpty);
            ClearCommand = new DelegateCommand(ClearMethod, ()=> Files.ListNotEmpty);
        }

        private void ClearMethod()
        {
            Files = new Files();
            FolderName = string.Empty;
        }

        private void SaveMethod()
        {
            Files.Save();
        }

        private void ChooseFolderMethod()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FolderName = dialog.SelectedPath;
                var filePaths = Directory.GetFiles(FolderName, "*.resx", SearchOption.AllDirectories).ToList();
                if (filePaths.Count == 0)
                    MessageBox.Show("There is no resource files in this directory");
                else
                    Files = new Files(filePaths, FolderName);
            }
        }
    }
}
