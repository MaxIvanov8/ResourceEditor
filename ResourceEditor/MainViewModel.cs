using System.Data;
using System.IO;
using System.Linq;
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

	[ObservableProperty] private DataTable _dataTable;
	public DataView DataView => DataTable.DefaultView;

	public MainViewModel()
	{
		_files = new Files();
		DataTable = new DataTable();
	}

	[RelayCommand(CanExecute = nameof(CanExecute))]
	private void Clear()
	{
		Files = new Files();
		FolderName = string.Empty;
		DataTable = new DataTable();
		OnPropertyChanged(nameof(DataView));
	}

	[RelayCommand(CanExecute = nameof(CanExecute))]
	private void Save()
	{
		var list = Files.LangList.SelectMany(langClass => langClass.ResxFiles);
		foreach (DataRow row in DataTable.Rows)
		{
			var resxFile = list.Where(i => i.Group.ShortPath == (string)row.ItemArray[0]).ToList();
			for (var i = 0; i < resxFile.Count; i++)
			{
				var entry = resxFile[i].Values.First(j => j.Name == (string)row.ItemArray[1]);
				entry.Value = row.ItemArray[i + 2].ToString();
			}
		}
		Files.Save();
	}

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
			{
				Files = new Files(filePaths, FolderName);
				DataTable = new DataTable();
				DataTable.Columns.Add("Path");
				DataTable.Columns.Add("Key");
				foreach (var langClass in Files.LangList)
				{
					DataTable.Columns.Add(langClass.Language);
				}
				for (var i = 0; i < Files.LangList[0].EntryList.Count; i++)
				{
					var l = new object[Files.LangList.Count + 2];
					l[0] = Files.LangList[0].EntryList[i].ResxFile.Group.ShortPath;
					l[1] = Files.LangList[0].EntryList[i].Name;
					for (var j = 0; j < Files.LangList.Count; j++)
						l[j + 2] = Files.LangList[j].EntryList[i].Value;

					DataTable.Rows.Add(l);
				}
				OnPropertyChanged(nameof(DataView));
			}
		}
	}
}