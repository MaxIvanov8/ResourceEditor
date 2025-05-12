using System.Collections.Generic;
using System.Linq;

namespace ResourceEditor.Models;

public class LangClass(ResxFile resxFile)
{
	public string Language { get; } = resxFile.Group.Language;
	public List<ResxFile> ResxFiles { get; set; } = [resxFile];

	public List<Entry> EntryListOrdered
	{
		get
		{
			var result = new List<Entry>();
			foreach (var resxFile in ResxFiles)
				result.AddRange(resxFile.Values);
			return result.OrderBy(i=>i.Name).ThenBy(i=>i.ResxFile.Group.FileName).ToList();
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