namespace NetUtils.Ports
{
	public static class PortProvider
	{
        private static Random rnd = new Random(Environment.TickCount);

		/// <summary>
		/// Gets a free port.
		/// </summary>
		/// <param name="fromPort"></param>
		/// <param name="toPort"></param>
		/// <returns>Number of the port.</returns>
		public static int GetFreePort(int fromPort = 1024, int toPort = UInt16.MaxValue)
		{
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
