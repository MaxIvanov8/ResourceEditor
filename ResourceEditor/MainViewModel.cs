using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ResourceEditor.Models;

namespace ResourceEditor;

public partial class MainViewModel:ObservableObject
{
	[ObservableProperty]
	private string _folderName;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SaveCommand), nameof(ClearCommand))]
	private Files _files;

	public MainViewModel()
	{
		_files = new Files();
	}

	[RelayCommand(CanExecute = nameof(CanExecute))]
	private void Clear()
	{
		Files = new Files();
		FolderName = string.Empty;
	}

	[RelayCommand(CanExecute = nameof(CanExecute))]
	private void Save() => Files.Save();

	private bool CanExecute => Files.ListNotEmpty;

	[RelayCommand]
	private void ChooseFolder()
	{
		var dialog = new OpenFolderDialog();
		if (dialog.ShowDialog() == true)
		{
			FolderName = dialog.FolderName;
			var filePaths = Directory.GetFiles(FolderName, "*.resx", SearchOption.AllDirectories);
			if (filePaths.Length == 0)
				MessageBox.Show("There are no resource files in this directory");
			else
				Files = new Files(filePaths, FolderName);
		}
	}
}