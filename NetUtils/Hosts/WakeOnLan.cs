using NetUtils.Utils;
using System.Net;
using System.Net.Sockets;

namespace NetUtils.Hosts
{
    internal class WakeOnLan
	{
		public static int WakeOnLAN(string macAddress, UInt16 port)
		{
			if (macAddress == null)
			{
				throw new ArgumentNullException(nameof(macAddress));
			}

			var macByteArray = MacAddresToByteArray.ToByteArray(macAddress);
			var magicPacket = MagicPacketBuilder.Build(macByteArray);
			var endPoint = new IPEndPoint(IPAddress.Broadcast, port);

			int sentBytes;
			var clientSocket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			try
			{
				clientSocket.Connect(endPoint);
				sentBytes = clientSocket.Send(magicPacket, 0, magicPacket.Length, SocketFlags.None);
				clientSocket.Close();
			}
			catch (SocketException)
			{
				if (clientSocket != null)
				{
					clientSocket.Close();
				}
				throw;
			}
			return sentBytes;
		}
	}
}
