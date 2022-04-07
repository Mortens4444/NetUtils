using System.Net;

namespace NetUtils.Hosts
{
    public class HostnameProvider
	{
		public static string Get(string ip)
		{
			string result = string.Empty;
			try
			{
				IPHostEntry hostInfo;
				try
				{
					if (Environment.OSVersion.Version.Major > 5) // From Windows Vista
					{
						hostInfo = Dns.GetHostEntry(ip);
					}
					else
					{
#pragma warning disable CS0618 // Type or member is obsolete
                        hostInfo = Dns.GetHostByAddress(ip);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
				}
				catch
				{
					hostInfo = Dns.GetHostEntry(ip);
				}
				result = hostInfo.HostName;
			}
			catch { }
			return result;
		}
	}
}
