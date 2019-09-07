using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using Newtonsoft.Json;
using UnityEngine;

namespace TcpConsoleApp
{
    class Client
    {
        private static TcpClient _client;
        private static NetworkStream nstream;

        //相手とのTCP接続
        public void Connect(out TcpClient _client ,string ip, int port)
        {

            try
            {
                //接続要求
                _client = new TcpClient(ip, port);
                Debug.Log("Connect");
            }
            catch
            {
                Debug.Log("Error");
                _client = null;
                return ;
            }
            
        }

        public void Send(TcpClient client, Byte[] sendBytes)
        {
            //ファイルの送信
            nstream = client.GetStream();
            {
                nstream.ReadTimeout = 15000;
                nstream.WriteTimeout = 15000;
                
                nstream.WriteAsync(sendBytes, 0, sendBytes.Length).ContinueWith(task =>
                {
                    Console.WriteLine("send");
                    Console.WriteLine("sendeBytes" + sendBytes.Length.ToString() + "Bytes");
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }


        public static void Close()
        {
            try
            {
                nstream.Close();
                _client.Close();
                Console.WriteLine("切断した");
            }
            catch
            {
                Console.WriteLine("aaa");
            }
        }
    }

    class SomeSend : Client
    {
//        public void Sendf(string ip, int port, string filename)
//        {
//            //connect
//            Client client = new Client();
//            var cl = client.Connect(ip, port);
//            Console.WriteLine("connected...");
//            if (cl == null)
//            {
//                Sendf(ip, port, filename);
//
//            }
//            else
//            {
//                //FileByteGet
//                var fe = new FileExchange();
//                var fileBytes = fe.LoadFile(filename);
//                //send
//                client.Send(cl, fileBytes);
//            }
//        }

        public void SendJ(Client client,TcpClient cl ,String jsonString)
        {
            if (cl == null)
            {
                //タスクの一時中断
                Task.Delay(1000);
            }
            else
            {
                //FileByteGet
                var jsonBytes = Encoding.Default.GetBytes(jsonString);
                //send
                client.Send(cl, jsonBytes);
            }
        }
    }
}
