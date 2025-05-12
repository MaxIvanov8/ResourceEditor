using System.IO;

namespace ResourceEditor.Models;

public class Group
{
	public string FileName { get; }
	public string Language { get; }
	public string ShortPath { get; }

	public Group(string fileName, string folderName)
	{
		FileName = fileName;
		var split = Path.GetFileNameWithoutExtension(FileName).Split('.');
		Language = split.Length > 1 ? split[1] : "Default";
		ShortPath = Path.Combine(Path.GetDirectoryName(FileName).Replace(folderName, string.Empty), split[0]);
	}

	public Group(Group group, string language)
	{
		Language = language;
		ShortPath = group.ShortPath;
		FileName = language=="Default" ? Path.Combine(Path.GetDirectoryName(group.FileName), "Resources.resx") : $"{Path.Combine(Path.GetDirectoryName(group.FileName), Path.GetFileNameWithoutExtension(group.FileName))}.{language}.resx";
	}

	public bool IsEqual(Group other) => ShortPath == other.ShortPath;
}