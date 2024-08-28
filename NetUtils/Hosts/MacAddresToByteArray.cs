using System.Globalization;
using System.Text.RegularExpressions;

namespace NetUtils.Hosts
{
    public static class MacAddressToByteArray
    {
		public static byte[] ToByteArray(string macString)
		{
			if (macString == null)
			{
				throw new ArgumentNullException(nameof(macString));
			}
			var mac = Regex.Replace(macString, "[^0-9A-Fa-f]", String.Empty);
			if (mac.Length != Constants.MacAddressLengthInBytes * 2)
            {
				throw new ArgumentException("Incorrect MAC address", nameof(macString));
            }

			var result = new byte[Constants.MacAddressLengthInBytes];
			for (var i = 0; i < result.Length; i++)
			{
				var hexa = new string(new[] { mac[i * 2], mac[i * 2 + 1] });
				result[(i)] = Byte.Parse(hexa, NumberStyles.HexNumber);
			}
			return result;
		}
	}
}
