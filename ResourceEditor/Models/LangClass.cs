using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ResourceEditor.Models;

public class LangClass:ObservableObject
{
	public LangClass(ResxFile resxFile)
	{
		FilterName = string.Empty;
		ResxFiles = [resxFile];
		Language = resxFile.Group.Language;
	}

	public string Language { get; }
	public List<ResxFile> ResxFiles { get; set; }

	private string _filterName;
	public string FilterName
	{
		get => _filterName;
		set
		{
			_filterName = value;
			OnPropertyChanged();
			OnPropertyChanged(nameof(FilterValues));
		}
	}

	public IEnumerable<Entry> FilterValues => FilterName == string.Empty ? EntryList : EntryList.Where(item => item.Name.Contains(FilterName));

	public IEnumerable<Entry> EntryList
	{
		get
		{
			var result = new List<Entry>();
			foreach (var resxFile in ResxFiles)
				result.AddRange(resxFile.Values);

			return result.OrderBy(i=>i.Name).ThenBy(i=>i.ResxFile.Group.FileName);
		}
	}

	public void AddEntryToResxFile(Name name)
	{
		var resxFile = ResxFiles.FirstOrDefault(i => i.Group.IsEqual(name.Group));
		if(resxFile != null)
			resxFile.Values.Add(new Entry(name.Value, resxFile));
		else
		{
			var newResxFile = new ResxFile(new Group(name.Group, Language));
			newResxFile.Values.Add(new Entry(name.Value, newResxFile));
			ResxFiles.Add(newResxFile);
		}
	}
}