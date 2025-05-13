using System.Collections.Generic;
using System.Linq;

namespace ResourceEditor.Models;

public class LangClass(ResxFile resxFile)
{
	public List<ResxFile> ResxFiles { get; } = [resxFile];
	public string Language => ResxFiles[0].Group.Language;

	public List<Entry> EntryList
	{
		get
		{
			var result = new List<Entry>();
			foreach (var resxFile in ResxFiles)
				result.AddRange(resxFile.Values);
			return result;
		}
	}

	public List<Entry> EntryListOrdered => EntryList.OrderBy(i=>i.Name).ThenBy(i=>i.Group.FileName).ToList();

	public void AddEntryToResxFile(Entry entry)
	{
		var resxFile = ResxFiles.FirstOrDefault(i => i.Group.IsEqual(entry.Group));
		if(resxFile != null)
			resxFile.Values.Add(new Entry(entry.Name, resxFile.Group));
		else
		{
			
			ResxFiles.Add(new ResxFile(entry, Language));
		}
	}
}