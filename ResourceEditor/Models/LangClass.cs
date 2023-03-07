using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ResourceEditor.Models
{
    public class LangClass:INotifyPropertyChanged
    {
        public LangClass(ResxFile resxFile)
        {
            FilterName = string.Empty;
            ResxFiles = new List<ResxFile> { resxFile };
            Language = resxFile.Group.Language;
        }

        public string Language { get; }
        public List<ResxFile> ResxFiles { get; set; }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set
            {
                _filterName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterValues));
            }
        }

        public IEnumerable<Entry> FilterValues
        {
            get
            {
                if (FilterName == string.Empty) return EntryList;
                return EntryList.Where(item => item.Name.Contains(FilterName));
            }
        }

        public IEnumerable<Entry> EntryList
        {
            get
            {
                var result = new List<Entry>();
                foreach (var resxFile in ResxFiles)
                    result.AddRange(resxFile.Values);

                return result.OrderBy(i=>i.Name).ThenBy(i=>i.ResxFile.Group.FileName);
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
