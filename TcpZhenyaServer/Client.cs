using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpZhenyaServer
{
    public class Client
    {
        public NetworkStream Stream { get; set; }
        public byte[] Buffer { get; set; }
        public TcpClient TcpClient { get; set; }
        public string Str { get; set; }
    }
}
