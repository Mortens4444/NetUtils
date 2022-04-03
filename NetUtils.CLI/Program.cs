using NetUtils.Ports;

[assembly: CLSCompliant(true)]
namespace NetUtils.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (ArgUtils.IsParamUsed(args, "port"))
            {
                var searchCriteria = ArgUtils.GetNextArg(args, "port");
                var portIdentifier = new PortIdentifier();
                var ports = portIdentifier.Search(searchCriteria, StringComparison.OrdinalIgnoreCase);
                var result = string.Join(Environment.NewLine, ports.Select(port => port.ToString()));
                Console.WriteLine(result);
            }
        }
    }
}