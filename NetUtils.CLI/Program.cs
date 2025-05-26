using NetUtils.Hosts;
using NetUtils.Ports;
using System.Globalization;
using System.Net.Sockets;

[assembly: CLSCompliant(true)]
namespace NetUtils.CLI
{
    public static class Program
    {
        private const string Help = "help";
        private const string Discoverhosts = "discoverhosts";
        private const string Portscan = "portscan";
        private const string From = "from";
        private const string To = "to";
        private const string Port = "port";
        private const string Portinfo = "portinfo";
        private const string Ping = "ping";

        public static void Main(string[] args)
        {
            if (ArgUtils.IsParamUsed(args, Help))
            {
                Console.WriteLine(Resources.PortScanHelp);
                Console.WriteLine(Resources.PortSearchHelp);
                Console.WriteLine(Resources.PortInfoHelp);
                Console.WriteLine(Resources.DiscoverHostsHelp);
                Console.WriteLine(Resources.PingHelp);
            }
            
            if (ArgUtils.IsParamUsed(args, Discoverhosts))
            {
                HostDiscoveryService.Discovery();
            }

            if (ArgUtils.IsParamUsed(args, Portscan))
            {
                var targetIp = ArgUtils.GetNextArg(args, Portscan);
                var portScanner = new PortScanner(targetIp);

                ushort fromPort = UInt16.MinValue;
                if (ArgUtils.IsParamUsed(args, From))
                {
                    fromPort = UInt16.Parse(ArgUtils.GetNextArg(args, From), CultureInfo.InvariantCulture);
                }

                ushort toPort = UInt16.MaxValue;
                if (ArgUtils.IsParamUsed(args, To))
                {
                    toPort = UInt16.Parse(ArgUtils.GetNextArg(args, To), CultureInfo.InvariantCulture);
                }

                portScanner.Scan(fromPort, toPort, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            if (ArgUtils.IsParamUsed(args, Port))
            {
                var searchCriteria = ArgUtils.GetNextArg(args, Port);
                var ports = PortIdentifier.Search(searchCriteria, StringComparison.OrdinalIgnoreCase);
                var result = string.Join(Environment.NewLine, ports.Select(port => port.ToString()));
                Console.WriteLine(result);
            }

            if (ArgUtils.IsParamUsed(args, Portinfo))
            {
                var port = UInt16.Parse(ArgUtils.GetNextArg(args, Portinfo), CultureInfo.InvariantCulture);
                var portInfo = PortIdentifier.Get(port);
                Console.WriteLine(portInfo != null ? portInfo : "Port not found in the database");
            }

            if (ArgUtils.IsParamUsed(args, Ping))
            {
                var ipAddress = ArgUtils.GetNextArg(args, Ping);
                using var pingSender = new PingSender();
                pingSender.PingReplyArrived += (object sender, PingReplyArrivedEventArgs e) =>
                    {
                        Console.WriteLine($"Success: {e.Success} - {e.StatusMessage}");
                        Environment.Exit(0);
                    };
                pingSender.SendAsync(ipAddress);
            }
        }
    }
}