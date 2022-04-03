using System.Text;

namespace NetUtils.Ports
{
    public class Service
    {
        public string Name { get; }

        public string Description { get; }

        public string Alias { get; }

        public Service(string name, string description = "", string alias = "")
        {
            Name = name;
            Description = description;
            Alias = alias;
        }

        public bool IsMatch(string searchCriteria, StringComparison stringComparison)
        {
            return Name.Contains(searchCriteria, stringComparison) ||
                Description.Contains(searchCriteria, stringComparison) ||
                Alias.Contains(searchCriteria, stringComparison);
        }

        public override string ToString()
        {
            var result = new StringBuilder(Name);
            if (Description != String.Empty)
            {
                result.Append($" Description: {Description}");
            }
            if (Alias != String.Empty)
            {
                result.Append($" Alias: {Alias}");
            }
            return result.ToString().TrimStart(' ');
        }
    }
}
