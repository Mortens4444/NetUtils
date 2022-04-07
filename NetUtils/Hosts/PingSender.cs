using System.Text;
using System.Net.NetworkInformation;

namespace NetUtils.Hosts
{
    public class PingSender : IDisposable
    {
        private readonly Ping ping = new();
        private int disposed = 0;
        
        public event PingReplyArrivedEventHandler? PingReplyArrived;        
        public delegate void PingReplyArrivedEventHandler(object sender, PingReplyArrivedEventArgs e);

        public PingSender()
        {
            ping.PingCompleted += (object sender, PingCompletedEventArgs e) =>
            {
                PingReplyArrived?.Invoke(this, new PingReplyArrivedEventArgs(e.Reply));
                DisposePing();
            };
        }

        ~PingSender()
        {
            DisposeObject();
        }

        public void Dispose()
        {
            DisposeObject();
            GC.SuppressFinalize(this);
        }

        public void DisposeObject()
        {
            if (Interlocked.Exchange(ref disposed, 1) != 0)
            {
                return;
            }

            DisposePing();
        }

        public void SendAsync(string ipAddress, string data = "ping")
        {
            SendAsync(ipAddress, (int)TimeSpan.FromSeconds(10).TotalMilliseconds, data);
        }

        public void SendAsync(string ipAddress, int timeout, string data = "ping")
        {
            try
            {
                ping.SendAsync(ipAddress, timeout, Encoding.ASCII.GetBytes(data), null);
                Thread.Sleep(timeout);
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        private void DisposePing()
        {
            ((IDisposable)ping).Dispose();
        }
    }
}
