using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Server
{
    internal class Program //Server
    {
        //建立HashSet用來儲存成功連線過的client
        static HashSet<TcpClient> clients = new HashSet<TcpClient>();
        static void Main(string[] args)
        {
            const int port = 4099;  //開一個port

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            var listener = new TcpListener(IPAddress.Any, port); //開一個listener來接IP

            var theThread = new Thread(HandleMessages); //建立一個執行緒來跑Messages管理器
            theThread.Start();

            try
            {
                Console.WriteLine("Server start at port {0}", port);
                listener.Start();  //由此開始accept

                while (true)
                {
                    Console.WriteLine("Waiting for a connection~~~~");
                    var client = listener.AcceptTcpClient();  //將listener開成client

                    var address = client.Client.RemoteEndPoint.ToString();  //從client取得地址
                    Console.WriteLine("Client has connected from {0}", address);
                    //System.Threading.Thread.Sleep(1000); //間隔1秒

                    lock (clients)
                    {
                        clients.Add(client);  //用來鎖定client的順序
                    }
                }

                //client.Close();
                //Console.WriteLine("Disconnect client{0}",address);

            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("Server shutdown");
            }
        }

        private static void HandleMessages() //建立一個Messages管理器的Function
        {
            while (true)
            {
                lock (clients)
                {
                    foreach (var client in clients)  //將存起來的Tcpclients按照順序Receive
                    {
                        try
                        {
                            if (client.Available > 0)

                            {
                                Receive(client);  //Call Read
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error: {0}", e);
                        }
                    }
                }
            }
        }
        private static void Receive(TcpClient client)
        {
            var stream = client.GetStream(); //接收client內丟過來的資料
            var address = client.Client.RemoteEndPoint.ToString(); //從client裡面撈出address

            var numBytes = client.Available;  //開一個numBytes來接收資料的總量
            if (numBytes == 0)  //檢查是否有傳
            {
                return;
            }
            //int[] a = new int[1]; 陣列範例
            var buffer = new byte[numBytes];  //開一個暫存用的容器 byte[numBytes]buffer
            var bytesRead = stream.Read(buffer, 0, numBytes); //開一個bytesRead來接收從buffer內抓出來的DATA

            var request = Encoding.ASCII.GetString(buffer).Substring(0, bytesRead); //用class Encoding內的ASCII來執行GetString
            Console.WriteLine("Text: {0} from {1}" + request, address);

        }
    }
}
