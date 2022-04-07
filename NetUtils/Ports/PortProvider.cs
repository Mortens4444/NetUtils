namespace NetUtils.Ports
{
	public static class PortProvider
	{
		/// <summary>
		/// Gets a free port.
		/// </summary>
		/// <param name="fromPort"></param>
		/// <param name="toPort"></param>
		/// <returns>Number of the port.</returns>
		public static int GetFreePort(int fromPort = 1024, int toPort = UInt16.MaxValue)
		{
			var rnd = new Random(Environment.TickCount);
			int port;

			do
			{
				port = rnd.Next(fromPort, toPort);
			}
			while (!PortChecker.IsPortAvailable(port));

			return port;
		}
	}
}
