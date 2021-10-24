using System;
using System.Net;
using System.Net.Sockets;

namespace UDP_Async_Client
{
    class ClientFile
    {
        static void Main(string[] args)
        {
            Network netCode = new Network();
            netCode.SendData();
        }
    }

    class Network
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23000);
        byte[] buffer = { 0, 1 };

        public void SendData()
        {
            SocketAsyncEventArgs asocket = new SocketAsyncEventArgs();

            try
            {
                asocket.SetBuffer(buffer, 0, buffer.Length);
                asocket.RemoteEndPoint = ipep;
                socket.SendToAsync(asocket);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                

                asocket.Completed += DoneSending;
            }
            catch (Exception excp)
            {

                Console.WriteLine(excp);
            }
        }

        private void DoneSending(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine("Data sent..");
        }
    }
}
