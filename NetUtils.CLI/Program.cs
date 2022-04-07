using NetUtils.Hosts;
using NetUtils.Ports;
using System.Globalization;
using System.Net.Sockets;

[assembly: CLSCompliant(true)]
namespace NetUtils.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (ArgUtils.IsParamUsed(args, "help"))
            {
                Console.WriteLine(Resources.PortScanHelp);
                Console.WriteLine(Resources.PortSearchHelp);
                Console.WriteLine(Resources.PortInfoHelp);
                Console.WriteLine(Resources.DiscoverHostsHelp);
                Console.WriteLine(Resources.PingHelp);
            }
            
            if (ArgUtils.IsParamUsed(args, "discoverhosts"))
            {
                HostDiscoveryService.Discovery();
            }

            if (ArgUtils.IsParamUsed(args, "portscan"))
            {
                var targetIp = ArgUtils.GetNextArg(args, "portscan");
                var portScanner = new PortScanner(targetIp);

                ushort fromPort = UInt16.MinValue;
                if (ArgUtils.IsParamUsed(args, "from"))
                {
                    fromPort = UInt16.Parse(ArgUtils.GetNextArg(args, "from"), CultureInfo.InvariantCulture);                    
                }

                ushort toPort = UInt16.MaxValue;
                if (ArgUtils.IsParamUsed(args, "to"))
                {
                    toPort = UInt16.Parse(ArgUtils.GetNextArg(args, "to"), CultureInfo.InvariantCulture);
                }

                portScanner.Scan(fromPort, toPort, AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            if (ArgUtils.IsParamUsed(args, "port"))
            {
                var searchCriteria = ArgUtils.GetNextArg(args, "port");
                var ports = PortIdentifier.Search(searchCriteria, StringComparison.OrdinalIgnoreCase);
                var result = string.Join(Environment.NewLine, ports.Select(port => port.ToString()));
                Console.WriteLine(result);
            }

            if (ArgUtils.IsParamUsed(args, "portinfo"))
            {
                var port = UInt16.Parse(ArgUtils.GetNextArg(args, "portinfo"), CultureInfo.InvariantCulture);
                var portInfo = PortIdentifier.Get(port);
                Console.WriteLine(portInfo != null ? portInfo : "Port not found in the database");
            }

            if (ArgUtils.IsParamUsed(args, "ping"))
            {
                var ipAddress = ArgUtils.GetNextArg(args, "ping");
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