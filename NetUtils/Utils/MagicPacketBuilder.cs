namespace NetUtils.Utils
{
	public static class MagicPacketBuilder
    {
		public static byte[] Build(byte[] macByteArray)
		{
			var result = new byte[(Constants.MacAddressRepetitionsInMagicPacket + 1) * Constants.MacAddressLengthInBytes];
			for (var i = 0; i < Constants.MacAddressLengthInBytes; i++)
			{
				result[i] = 255;
			}
			for (var i = 0; i < Constants.MacAddressRepetitionsInMagicPacket; i++)
            {
				for (var j = 0; j < Constants.MacAddressLengthInBytes; j++)
                {
					result[((i * Constants.MacAddressLengthInBytes) + j) + Constants.MacAddressLengthInBytes] = macByteArray[j];
                }
            }
			return result;
		}
	}
}
