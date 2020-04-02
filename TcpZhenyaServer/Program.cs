using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpZhenyaServer
{
    class Program
    {
        static List<Client> clients = new List<Client>();
        static string receiveMessage = string.Empty;
        static void Main(string[] args)
        {
            Console.Title = "Server";
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(iPAddress, 1011);
            listener.Start();
            while (true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(Process));
                thread.Start(listener.AcceptTcpClient());
            }
        }

        static void Process(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream ns = tcpClient.GetStream();

            Client clientobj = new Client()
            {
                Buffer = new byte[1024],
                Stream = ns,
                TcpClient = tcpClient
            };

            clients.Add(clientobj);

            ns.BeginRead(clientobj.Buffer, 0, clientobj.Buffer.Length, new AsyncCallback(ReadMessage), clientobj);
        }

        static void ReadMessage(IAsyncResult aresult)
        {

            Client clientobj = (Client)aresult.AsyncState;
            int size = clientobj.Stream.EndRead(aresult);
            if (size > 0)
            {
                if (clientobj.Stream.CanRead)
                {

                    clientobj.Str += Encoding.UTF8.GetString(clientobj.Buffer , 0 ,size);

                    if(clientobj.Str.IndexOf("<EOF>") > -1)
                    {
                        Console.WriteLine("Client says:" + clientobj.Str);
                        byte[] sendData = Encoding.UTF8.GetBytes(clientobj.Str);
                        for (int i = 0; i < clients.Count; i++)
                        {
                            clients[i].Stream.Write(sendData, 0, sendData.Length);
                        }
                        clientobj.Str = string.Empty;
                    }
                }
                clientobj.Stream.BeginRead(clientobj.Buffer, 0, clientobj.Buffer.Length, new AsyncCallback(ReadMessage), clientobj);
            }
        }
    }
}
