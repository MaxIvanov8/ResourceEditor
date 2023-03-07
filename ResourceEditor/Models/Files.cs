using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ResourceEditor.Models
{
    public class Files: INotifyPropertyChanged
    {
        private readonly List<ResxFile> _filesList;
        public ObservableCollection<Name> Names { get; }
        public List<LangClass> LangList { get; }
        private string _nameFilter;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterNames));
                foreach (var langClass in LangList)
                    langClass.FilterName = _nameFilter;
            }
        }

        public IEnumerable<Name> FilterNames => _nameFilter == string.Empty
            ? Names
            : Names.Where(item => item.Value.Contains(_nameFilter));

        public Files()
        {
            LangList = new List<LangClass>();
            _filesList = new List<ResxFile>();
            NameFilter = string.Empty;
            Names = new ObservableCollection<Name>();
        }

        public Files(IEnumerable<string> pathList, string folderName) : this()
        {
            foreach (var file in pathList.Select(path => new ResxFile(path, folderName)))
                _filesList.Add(file);

            foreach (var resxFile in _filesList)
            {
                var t = LangList.FirstOrDefault(i => i.Language == resxFile.Group.Language);
                if(t==null) LangList.Add(new LangClass(resxFile));
                else t.ResxFiles.Add(resxFile);
            }

            foreach (var resxFile in _filesList)
            foreach (var resValue in resxFile.Values)
            {
                if (Names.Any(item =>
                        item.Value == resValue.Name && item.Group.IsEqual(resValue.ResxFile.Group)))
                    continue;
                Names.Add(new Name(resValue.Name, resValue.ResxFile.Group));
            }

            foreach (var name in Names)
            foreach (var lang in LangList)
            {
                var t = lang.EntryList.FirstOrDefault(i =>
                    i.ResxFile.Group.IsEqual(name.Group) && i.Name == name.Value);
                if (t == null)
                    lang.AddEntryToResxFile(name);
            }

            LangList = LangList.OrderBy(i => i.Language).ToList();
            var defaultLang = LangList.FirstOrDefault(i => i.Language == "Default");
            if (defaultLang != null)
            {
                LangList.Remove(defaultLang);
                LangList.Insert(0, defaultLang);
            }

            Names = new ObservableCollection<Name>(Names.OrderBy(item => item.Value)
                .ThenBy(item => item.Group.FileName));
        }

        public bool ListNotEmpty => _filesList.Count > 0;

        public void Save()
        {
            foreach (var langClass in LangList)
            {
                foreach (var langClassResxFile in langClass.ResxFiles)
                {
                    langClassResxFile.Save();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
