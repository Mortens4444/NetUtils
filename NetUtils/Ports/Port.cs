using System.Text;

namespace NetUtils.Ports
{
    public class Port
    {
        public ushort Number { get; }
        
        public List<Service> Services { get; }

        public Port(ushort number, List<Service> services)
        {
            Number = number;
            Services = services;
        }

        public bool IsMatch(string searchCriteria, StringComparison stringComparison)
        {
            return Number.ToString().Contains(searchCriteria, stringComparison) ||
                Services.Any(service => service.IsMatch(searchCriteria, stringComparison));
        }

        public override string ToString()
        {
            var result = new StringBuilder(Number.ToString());
            if (Services != null && Services.Any())
            {
                var services = String.Join(", ", Services.Select(service => service.ToString()));
                result.Append($": {services}");
            }
            return result.ToString();
        }
    }
}
