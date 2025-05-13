using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ResourceEditor.Models;

namespace ResourceEditor;

public partial class MainViewModel : ObservableObject
{
	private const string ProgramMessage = "ProgramMessage";
	[ObservableProperty]
	private string _folderName;

	[ObservableProperty]
	private string _text;

	[ObservableProperty] private bool _addEmptyEntries;

	[ObservableProperty] private DataTable _dataTable;
	public DataView DataView => DataTable.DefaultView;

	private List<LangClass> _langList;

	public MainViewModel()
	{
		_langList = [];
		DataTable = new DataTable();
		Text = "Choose folder with resource files";
	}

	[RelayCommand]
	private void Clear()
	{
		_langList.Clear();
		FolderName = string.Empty;
		DataTable.Clear();
		Text = "Choose folder with resource files";
		Update();
	}

	private void Update()
	{
		OnPropertyChanged(nameof(DataView));
		OnPropertyChanged(nameof(CanExecute));
	}


	[RelayCommand]
	private void Save()
	{
		var list = _langList.SelectMany(langClass => langClass.ResxFiles);
		foreach (DataRow row in DataTable.Rows)
		{
			var resxFile = list.Where(i => i.Group.ShortPath == (string)row.ItemArray[0]).ToList();
			for (var i = 0; i < resxFile.Count; i++)
			{
				var entry = resxFile[i].Values.First(j => j.Name == (string)row.ItemArray[1]);
				entry.Value = row.ItemArray[i + 2].ToString();
			}
		}
		foreach (var langClassResxFile in _langList.SelectMany(langClass => langClass.ResxFiles))
			langClassResxFile.Save(AddEmptyEntries);
		MessageBox.Show("Data saved");
	}


	[RelayCommand]
	private async Task ChooseFolder()
	{
		var dialog = new OpenFolderDialog();
		if (dialog.ShowDialog() == true)
		{
			var filePaths = Directory.GetFiles(dialog.FolderName, "*.resx", SearchOption.AllDirectories);
			if (filePaths.Length == 0)
				MessageBox.Show("There are no resource files in this directory", ProgramMessage);
			else
			{
				var filesList = new List<ResxFile>();
				foreach (var filePath in filePaths)
				{
					try
					{
						filesList.Add(new ResxFile(filePath, dialog.FolderName));
					}
					catch (Exception e)
					{
						// ignored
					}
				}

				if (filesList.Count == 0)
				{
					MessageBox.Show("There are no text resource files in this directory", ProgramMessage);
					return;
				}

				if (filesList.Count != filePaths.Length)
					MessageBox.Show("Some resource files contains not only text", ProgramMessage);

				Text = "Your files are loading...";

				await Task.Run(() =>
				{
					foreach (var resxFile in filesList)
					{
						var t = _langList.FirstOrDefault(i => i.Language == resxFile.Group.Language);
						if (t == null) _langList.Add(new LangClass(resxFile));
						else t.ResxFiles.Add(resxFile);
					}
					var entryList = new List<Entry>();

					foreach (var resxFile in filesList)
						entryList.AddRange(resxFile.Values.Where(entry => !entryList.Any(item => item.IsEqual(entry))));

					foreach (var entry in entryList)
						foreach (var lang in _langList.Where(lang => !lang.EntryList.Any(i => i.IsEqual(entry))))
							lang.AddEntryToResxFile(entry);

					_langList = _langList.OrderBy(i => i.Language).ToList();
					var defaultLang = _langList.FirstOrDefault(i => i.Language == "Default");
					if (defaultLang != null)
					{
						_langList.Remove(defaultLang);
						_langList.Insert(0, defaultLang);
					}

					DataTable = new DataTable();
					DataTable.Columns.Add("Path");
					DataTable.Columns.Add("Key");
					DataTable.Columns[0].ReadOnly = true;
					DataTable.Columns[1].ReadOnly = true;
					foreach (var langClass in _langList)
						DataTable.Columns.Add(langClass.Language);
					for (var i = 0; i < _langList[0].EntryListOrdered.Count; i++)
					{
						var l = new object[_langList.Count + 2];
						l[0] = _langList[0].EntryListOrdered[i].Group.ShortPath;
						l[1] = _langList[0].EntryListOrdered[i].Name;
						for (var j = 0; j < _langList.Count; j++)
							l[j + 2] = _langList[j].EntryListOrdered[i].Value;

						DataTable.Rows.Add(l);
					}
				});

				FolderName = dialog.FolderName;
				Update();
			}
		}
	}

	public bool CanExecute => _langList.Count > 0;
}