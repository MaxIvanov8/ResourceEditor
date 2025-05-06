using System.Collections;

namespace ResourceEditor.Models;

public class Entry
{
	public ResxFile ResxFile { get; }
	public string Name { get; }
	public string Value { get; set; }
	public bool NoValue { get; }
	public bool NeedToWrite => !NoValue || Value != string.Empty;

	private Entry(ResxFile resxFile)
	{
		ResxFile = resxFile;
	}

	public Entry(DictionaryEntry entry, ResxFile resxFile):this(resxFile)
	{
		Name = entry.Key.ToString();
		Value = entry.Value.ToString();
	}

	public Entry(string nameValue, ResxFile resxFile) : this(resxFile)
	{
		Name = nameValue;
		Value = string.Empty;
		NoValue = true;
	}
}