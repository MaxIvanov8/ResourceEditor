namespace ResourceEditor.Models;

public class Name(string value, Group group)
{
	public string Value { get; } = value;
	public Group Group { get; } = group;
}