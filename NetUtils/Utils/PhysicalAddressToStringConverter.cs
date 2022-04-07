﻿using System.Net.NetworkInformation;
using System.Text;

namespace NetUtils.Utils
{
    public class PhysicalAddressToStringConverter
	{
		public static string ToString(PhysicalAddress? physicalAddress, char separator = ':')
		{
			if (physicalAddress == null)
            {
				return String.Empty;
            }
			string mac = physicalAddress.ToString();
			var friendlyMacAddress = new StringBuilder();
			for (int i = 0; i < mac.Length; i += 2)
			{
				if (i > 0)
				{
					friendlyMacAddress.Append(separator);
				}
				friendlyMacAddress.Append(mac[i]);
				friendlyMacAddress.Append(mac[i + 1]);
			}

			return friendlyMacAddress.ToString();
		}
	}
}
