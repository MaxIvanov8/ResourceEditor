using System.IO;

namespace ResourceEditor.Models
{
    public class Group
    {
        public Group(string fileName, string folderName)
        {
            FileName = fileName;
            var t = FileName.Replace(folderName, string.Empty);
            var resName = Path.Combine(Path.GetDirectoryName(t), Path.GetFileNameWithoutExtension(t));
            var split = resName.Split('.');
            Language = split.Length > 1 ? split[1] : "Default";
            ShortPath = split[0];
        }

        public Group(Group group, string language)
        {
            Language = language;
            ShortPath = group.ShortPath;
            FileName= $"{Path.Combine(Path.GetDirectoryName(group.FileName), Path.GetFileNameWithoutExtension(group.FileName))}.{language}.resx";
        }

        public string FileName { get; }
        public string Language { get; }
        public string ShortPath { get; }

        public bool IsEqual(Group other) => ShortPath == other.ShortPath;
    }
}
