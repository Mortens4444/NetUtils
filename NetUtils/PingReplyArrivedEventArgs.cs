using System.Net.NetworkInformation;

namespace NetUtils
{
	public class PingReplyArrivedEventArgs : EventArgs
	{
		public PingReplyArrivedEventArgs(PingReply? pingReply)
		{
			PingReply = pingReply;
			if ((pingReply != null) && (pingReply.Address != null))
			{
				Sender = pingReply.Address.ToString();
				Success = pingReply.Status == IPStatus.Success;
				StatusMessage = GetIPStatusDescription(pingReply.Status);
			}
			else
			{
				Sender = String.Empty;
				Success = false;
				StatusMessage = GetIPStatusDescription(IPStatus.Unknown);
			}
		}

		public string Sender { get; private set; }

		public PingReply? PingReply { get; private set; }

		public bool Success { get; private set; }

		public string StatusMessage { get; private set; }

		public override string ToString()
		{
			return StatusMessage;
		}

		private static string GetIPStatusDescription(IPStatus status)
		{
            string result = status switch
            {
                IPStatus.Success => "The ping request found the host.",
                IPStatus.DestinationNetworkUnreachable => "The ICMP echo request failed because the network that contains the destination computer is not reachable.",
                IPStatus.DestinationHostUnreachable => "The ICMP echo request failed because the destination computer is not reachable.",
                IPStatus.DestinationProtocolUnreachable => "The ICMP echo request failed because the destination computer that is specified in an ICMP echo message is not reachable,\nbecause it does not support the packet's protocol, or the ICMP echo request failed because contact with the destination computer is administratively prohibited.",
                IPStatus.DestinationPortUnreachable => "The ICMP echo request failed because the port on the destination computer is not available.",
                IPStatus.NoResources => "The ICMP echo request failed because of insufficient network resources.",
                IPStatus.BadOption => "The ICMP echo request failed because it contains an invalid option.",
                IPStatus.HardwareError => "The ICMP echo request failed because of a hardware error.",
                IPStatus.PacketTooBig => "The ICMP echo request failed because the packet containing the request is larger than the maximum transmission unit (MTU) of\na node (router or gateway) located between the source and destination.\nThe MTU defines the maximum size of a transmittable packet.",
                IPStatus.TimedOut => "The ICMP echo Reply was not received within the allotted time. The default time allowed for replies is 5 seconds.\nYou can change this value using the Send or SendAsync methods that take a timeout parameter.",
                IPStatus.BadRoute => "The ICMP echo request failed because there is no valid route between the source and destination computers.",
                IPStatus.TtlExpired => "The ICMP echo request failed because its Time to Live (TTL) value reached zero, causing the forwarding node (router or gateway) to discard the packet.",
                IPStatus.TtlReassemblyTimeExceeded => "The ICMP echo request failed because the packet was divided into fragments for transmission and all of the fragments were not\nreceived within the time allotted for reassembly. RFC 2460 (available at www.ietf.org)\nspecifies 60 seconds as the time limit within which all packet fragments must be received.",
                IPStatus.ParameterProblem => "The ICMP echo request failed because a node (router or gateway) encountered problems while processing the packet header.\nThis is the status if, for example, the header contains invalid field data or an unrecognized option.",
                IPStatus.SourceQuench => "The ICMP echo request failed because the packet was discarded. This occurs when the source computer's output queue has insufficient storage space,\nor when packets arrive at the destination too quickly to be processed.",
                IPStatus.BadDestination => "The ICMP echo request failed because the destination IP address cannot receive ICMP echo requests\nor should never appear in the destination address field of any IP datagram.\nFor example, calling Send and specifying IP address \"000.0.0.0\" returns this status.",
                IPStatus.DestinationUnreachable => "The ICMP echo request failed because the destination computer that is specified in an ICMP echo message is not reachable;\nthe exact cause of problem is unknown.",
                IPStatus.TimeExceeded => "The ICMP echo request failed because its Time to Live (TTL) value reached zero, causing the forwarding node\n(router or gateway) to discard the packet.",
                IPStatus.BadHeader => "The ICMP echo request failed because the header is invalid.",
                IPStatus.UnrecognizedNextHeader => "The ICMP echo request failed because the Next Header field does not contain a recognized value.\nThe Next Header field indicates the extension header type (if present) or the protocol above the IP layer, for example, TCP or UDP.",
                IPStatus.IcmpError => "The ICMP echo request failed because of an ICMP protocol error.",
                IPStatus.DestinationScopeMismatch => "The ICMP echo request failed because the source address and destination address\nthat are specified in an ICMP echo message are not in the same scope.\nThis is typically caused by a router forwarding a packet using an interface\nthat is outside the scope of the source address. Address scopes (link-local, site-local, and global scope) determine where on the network an address is valid.",
                IPStatus.Unknown => "The ICMP echo request failed for an unknown reason.",
                _ => "The ping request could not find the target host.",
            };
            return result;
		}
	}
}
