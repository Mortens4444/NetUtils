using NetUtils.Utils;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace NetUtils.Hosts
{
    public class HostDiscoveryService
	{
		private const int IpAddressLength = 30;
		private const int HostnameLength = 50;
		private const int MacAddressLength = 25;
		private const int NicLength = 20;
		private const int NicTypeLength = 20;
		private const int NicDescLength = 50;
		private const int SpeedLength = 20;

		public static void Discovery()
		{
			var hostname = Dns.GetHostName();
			var ips = Dns.GetHostEntry(hostname);

			Console.Write("IP Address".PadRight(IpAddressLength));
			Console.Write("Hostname".PadRight(HostnameLength));
			Console.Write("MAC Address".PadRight(MacAddressLength));
			Console.Write("Network interface".PadRight(NicLength));
			Console.Write("NIC type".PadRight(NicTypeLength));
			Console.Write("NIC Description".PadRight(NicDescLength));
			Console.Write("Speed".PadRight(SpeedLength));
			Console.WriteLine();

			foreach (var ip in ips.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetworkV6)
				{
					DiscoverHost(ip);
				}
				else
				{
					var subnet = ip.ToString()[..ip.ToString().LastIndexOf('.')];
					Parallel.For(1, 255, (int i) =>
                    {
						DiscoverHost(IPAddress.Parse($"{subnet}.{i}"));
					});
				}
			}
            Console.WriteLine("Discovery finished...");
		}

		private static void DiscoverHost(IPAddress ipAddress)
		{
			try
			{
				string ipAddressStr = ipAddress.ToString();
				var stringBuilder = new StringBuilder();
				stringBuilder.Append(ipAddressStr.PadRight(IpAddressLength));
				var hostname = HostnameProvider.Get(ipAddressStr);
				stringBuilder.Append(hostname.PadRight(HostnameLength));

				var phyhicalAddress = PhysicalAddressProvider.Get(ipAddress);
				stringBuilder.Append(PhysicalAddressToStringConverter.ToString(phyhicalAddress).PadRight(MacAddressLength));
				
				if (LocalIpAddressChecker.IsLocal(ipAddressStr))
				{
					var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
						.FirstOrDefault(nic => nic.GetIPProperties().UnicastAddresses
							.FirstOrDefault(ipInfo => ipInfo.Address.ToString() == ipAddressStr) != null);

					if (networkInterface != null)
					{
						stringBuilder.Append(networkInterface.Name.PadRight(NicLength));
						stringBuilder.Append(networkInterface.NetworkInterfaceType.ToString().PadRight(NicTypeLength));
						stringBuilder.Append(networkInterface.Description.PadRight(NicDescLength));
						stringBuilder.Append(HumanReadableValueFormatter.FormatValue(networkInterface.Speed, true).PadRight(SpeedLength));

						var stats = networkInterface.GetIPv4Statistics();
						var previousSentBytes = stats.BytesSent;
						var previousReceivedBytes = stats.BytesReceived;
					}
				}

				if (!String.IsNullOrEmpty(hostname))
                {
					Console.WriteLine(stringBuilder.ToString());
                }
			}
			catch { }
		}
	}
}
