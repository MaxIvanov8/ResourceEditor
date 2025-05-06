using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources.NetStandard;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ResourceEditor.Models;

public class ResxFile:ObservableObject
{
	public List<Entry> Values { get; }
	public Group Group { get; }

	private ResxFile()
	{
		Values = [];
	}

	public ResxFile(Group group):this()
	{
		Group = group;
	}

	public ResxFile(string fileName, string folderName) : this()
	{
		Group = new Group(fileName, folderName);
		var resReader = new ResXResourceReader(fileName);
		foreach (DictionaryEntry d in resReader)
			Values.Add(new Entry(d,this));
		resReader.Close();
	}

	public void Save()
	{
		if(Values.Any(d => d.NeedToWrite))
		{
			var resWriter = new ResXResourceWriter(Group.FileName);
			foreach (var d in Values.Where(d => d.NeedToWrite))
				resWriter.AddResource(d.Name, d.Value);
			resWriter.Close();
		}
	}
}