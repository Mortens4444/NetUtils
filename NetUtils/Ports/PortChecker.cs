using System.Net.NetworkInformation;

namespace NetUtils.Ports
{
    public class PortChecker
    {
		/// <summary>
		/// Checks if a port is available.
		/// </summary>
		/// <param name="port">Number of the port to be checked.</param>
		/// <returns>True if the port is free.</returns>
		public static bool IsPortAvailable(int port)
		{
			var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			var activeTcpConnections = ipGlobalProperties.GetActiveTcpConnections();
			return activeTcpConnections == null || activeTcpConnections.Any(tcpConnection => tcpConnection.LocalEndPoint.Port != port);
		}
	}
}
