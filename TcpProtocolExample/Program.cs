using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpProtocolExample
{
    class Program
    {
        static NetworkStream networkStream;
        static string resultMessage = string.Empty;
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 1011);
            Console.Title = "Client";
            networkStream = tcpClient.GetStream();

            byte[] buffer = new byte[1024];
            networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallBack), buffer);

            while (true)
            {
                string str = Console.ReadLine();
                str += "<EOF>";
                byte[] message = Encoding.UTF8.GetBytes(str);
                networkStream.Write(message, 0, message.Length);
            }
        }

        static void ReadCallBack(IAsyncResult result)
        {
            int size = networkStream.EndRead(result);
            if (size > 0)
            {
                // Read
                byte[] receivedData = (byte[])result.AsyncState;
                resultMessage += Encoding.UTF8.GetString(receivedData).Trim('\0');

                if(resultMessage.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine("Zhenya says:" + resultMessage.Substring(0 , resultMessage.Length - 5));
                    resultMessage = string.Empty;
                }
                byte[] buffer = new byte[1024];
                networkStream.BeginRead(buffer, 0, size, new AsyncCallback(ReadCallBack), buffer);

            }
        }
    }
}
