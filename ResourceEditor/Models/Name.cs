namespace ResourceEditor.Models
{
    public class Name
    {
        public string Value { get; }
        public Group Group { get; }

        public Name(string value, Group group)
        {
            Value = value;
            Group = group;
        }
    }
}
