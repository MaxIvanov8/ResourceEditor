using System.Collections;

namespace ResourceEditor.Models
{
    public class Entry
    {
        public Entry(DictionaryEntry entry, ResxFile resxFile)
        {
            Name = entry.Key.ToString();
            Value = entry.Value.ToString();
            ResxFile = resxFile;
        }

        public Entry(string nameValue, ResxFile resxFile)
        {
            Name = nameValue;
            Value = string.Empty;
            NoValue = true;
            ResxFile = resxFile;
        }

        public ResxFile ResxFile { get; }
        public string Name { get; }
        public string Value { get; set; }
        public bool NoValue { get; }
        public bool NeedToWrite=>!NoValue || Value!=string.Empty;
    }
}
