using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows;
using UnityEngine;

namespace TcpConsoleApp
{
    class Server
    {
        private static TcpListener _listener;
        public static List<TcpClient> _clients = new List<TcpClient>();

        public event Action<TcpClient> OnConnectEvent;
        public event Action<Byte[]> receiveCallBack;

        //接続準備、接続待機
        public Server()
        {
            _listener = new TcpListener(IPAddress.Any, 30000);
            _listener.Start();
            Console.WriteLine("Listen");
        }

        //接続要求待ち
        public void Accept()
        {
            _listener.AcceptTcpClientAsync().ContinueWith(task =>
            {
                TcpClient client = task.Result;
                _clients.Add(client);
                OnConnectEvent(client);
                Receive(client);
                Accept();
            });
        }

        //データ受信
        public async void Receive(TcpClient client)
        {
            bool isError = false;
            NetworkStream nStream = client.GetStream();
            using (MemoryStream mStream = new MemoryStream())
            {
                byte[] gdata = new byte[256];
                do
                {
                    int dataSize = await nStream.ReadAsync(gdata, 0, gdata.Length);
                    if (dataSize == 0) isError = true;
                    await mStream.WriteAsync(gdata, 0, dataSize);
                } while (nStream.DataAvailable);

                byte[] receiveBytes = mStream.GetBuffer();

                byte[] data = new byte[mStream.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = receiveBytes[i];
                }

                if (isError) return;
                receiveCallBack?.Invoke(data);

                if (data.Length == 0)
                {
                    Console.WriteLine($@"Passive Closing : {client.Client.RemoteEndPoint}");
                    client.Close();
                }
                Receive(client);
            }
        }

        public static void Close()
        {
            foreach (var cl in _clients)
            {
                cl.Close();
            }
            Console.WriteLine("listenStop");
        }
    }
}