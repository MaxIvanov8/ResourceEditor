using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources.NetStandard;

namespace ResourceEditor.Models;

public class ResxFile
{
	public List<Entry> Values { get; }
	public Group Group { get; }

	private ResxFile()
	{
		Values = [];
	}

	public ResxFile(Entry entry, string lang) : this()
	{
		Group = new Group(entry.Group, lang);
		Values.Add(new Entry(entry.Name, Group));
	}

	public ResxFile(string fileName, string folderName) : this()
	{
		Group = new Group(fileName, folderName);
		var resReader = new ResXResourceReader(fileName);
		foreach (DictionaryEntry d in resReader)
			Values.Add(new Entry(d, Group));
		resReader.Close();
	}

	public void Save(bool addEmptyEntries)
	{
		if (!addEmptyEntries && Values.All(item => string.IsNullOrEmpty(item.Value))) return;
		var resWriter = new ResXResourceWriter(Group.FileName);
		var list = addEmptyEntries ? Values : Values.Where(item => !string.IsNullOrEmpty(item.Value)).ToList();
		foreach (var d in list)
			resWriter.AddResource(d.Name, d.Value);
		resWriter.Close();
	}
}