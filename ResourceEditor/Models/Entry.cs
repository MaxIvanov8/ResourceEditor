using System.Collections;

namespace ResourceEditor.Models;

public class Entry
{
	public Group Group { get; }
	public string Name { get; }
	public string Value { get; set; }

	private Entry(Group group)
	{
		Group = group;
	}

	public Entry(DictionaryEntry entry, Group group) :this(group)
	{
		Name = entry.Key.ToString();
		Value = entry.Value.ToString();
	}

	public Entry(string name, Group group) : this(group)
	{
		Name = name;
	}

	public bool IsEqual(Entry entry) => Group.IsEqual(entry.Group) && Name == entry.Name;
}