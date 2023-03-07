using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources.NetStandard;

namespace ResourceEditor.Models
{
    public class ResxFile:INotifyPropertyChanged
    {
        public List<Entry> Values { get; }
        public Group Group { get; }

        public ResxFile(Group group)
        {
            Values = new List<Entry>();
            Group = group;
        }

        public ResxFile(string fileName, string folderName)
        {
            Group = new Group(fileName, folderName);
            Values = new List<Entry>();
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

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
