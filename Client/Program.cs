using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string hostIP = "127.0.0.1";  //host
            const int port = 4099;

            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            var client = new TcpClient();  //建一個client

            try
            {
                Console.WriteLine("連server囉 {0}:{1}", hostIP, port);  //連server囉
                client.Connect(hostIP, port);  //把IP往指定的port丟進去

                if (!client.Connected)
                {
                    Console.WriteLine("連不上啦"); //連線失敗
                    return;
                }

                Console.WriteLine("連上去囉");  //連線成功

                Console.WriteLine("隨便打幾個字");  //等待鍵入

                while (true)
                {
                    var msg = Console.ReadLine();
                    Send(client, msg);
                    Console.WriteLine("說: " + msg);
                }

                //var counter = 6;
                //while (counter > 0 )
                //{
                //    counter--;
                //    var msg = "倒數" + counter + "秒";
                //    Send(client, msg);
                //    Console.WriteLine("阿肥說:"+msg);
                //    System.Threading.Thread.Sleep(1000); //間隔一秒
                //}
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                client.Close();
                Console.WriteLine("不連囉"); //Disconnected
            }

        }
        private static void Send(TcpClient client, string msg)
        {
            var requestBuffer = System.Text.Encoding.ASCII.GetBytes(msg);

            client.GetStream().Write(requestBuffer, 0, requestBuffer.Length);
        }
    }
}
