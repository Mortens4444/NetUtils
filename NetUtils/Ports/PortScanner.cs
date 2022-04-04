using System.Net;
using System.Net.Sockets;

namespace NetUtils.Ports
{
    public class PortScanner
    {
		/// <summary>An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full.</summary>
		private const int WSAENOBUFS = 10055;

		/// <summary>An address incompatible with the requested protocol was used.</summary>
		private const int WSAEAFNOSUPPORT = 10047;

		/// <summary>The requested protocol has not been configured into the system, or no implementation for it exists.</summary>
		private const int WSAEPROTONOSUPPORT = 10043;

		private readonly IPAddress targetIp;
		private readonly PortIdentifier portIdentifier = new();

		public PortScanner(string targetIp)
        {
			this.targetIp = IPAddress.Parse(targetIp);
		}

        public void Scan(ushort fromPort, ushort toPort, AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
			var port = fromPort;
			do
			{
				try
				{
					var socketForPortTesting = new Socket(addressFamily, socketType, protocolType);
					{
						try
						{
							socketForPortTesting.BeginConnect(new IPEndPoint(targetIp, port), new AsyncCallback(ConnectCallback), socketForPortTesting);
							port++;
						}
						catch (SocketException ex)
						{
							if (ex.ErrorCode == WSAENOBUFS)
							{
								Console.Error.WriteLine($"{ex.GetType()} - {ex.Message}");
								Thread.Sleep(100);
							}
							else
							{
								port++;
							}
						}
					}
				}
				catch (SocketException ex)
				{
					if ((ex.ErrorCode == WSAEAFNOSUPPORT) || (ex.ErrorCode == WSAEPROTONOSUPPORT))
					{
						Console.Error.WriteLine($"{ex.GetType()} - {ex.Message}");
						break;
					}
				}
				catch
				{
					port++;
				}
			} while (port != toPort);
		}

		private void ConnectCallback(IAsyncResult result)
		{
			if (!result.IsCompleted || result.AsyncState == null)
			{
				return;
			}
            using var socket = (Socket)result.AsyncState;
            if (socket != null && socket.Connected && socket.RemoteEndPoint != null)
            {
                var endPointPort = (ushort)((IPEndPoint)socket.RemoteEndPoint).Port;
				var port = portIdentifier.Get(endPointPort);
				lock (this)
				{
					Console.Write(socket.ProtocolType);
					Console.WriteLine(port != null ? $": {port}" : $": {endPointPort}");
				}
            }
        }
	}
}
