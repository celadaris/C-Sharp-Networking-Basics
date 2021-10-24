using System;
using System.Net;
using System.Net.Sockets;

namespace UdpAsyncServer
{
    class ServerFile
    {
        static void Main(string[] args)
        {
            Network network = new Network();
            network.StartRec();
            Console.ReadLine();
            
        }
    }

    class Network
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 23000);

        public void StartRec()
        {
            socket.EnableBroadcast = true;
            try
            {
                SocketAsyncEventArgs socketAsync = new SocketAsyncEventArgs();
                socketAsync.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 23000);
                socketAsync.SetBuffer(new byte[2], 0, 2);
                int retryCount = 0;

                if (!socket.IsBound)
                {
                    socket.Bind(endPoint);
                }

                socketAsync.Completed += ReceiveCompletedCallback;

                if (!socket.ReceiveAsync(socketAsync))
                {
                    Console.WriteLine("there was an error, oh no!\n\n" + socketAsync.SocketError);
                    if (retryCount++ >= 10)
                    {
                        return;
                    }

                    else
                    {
                        StartRec();
                    }
                }
            }
            catch (Exception excp)
            {

                Console.WriteLine(excp);
            }
        }

        private void ReceiveCompletedCallback(object sender, SocketAsyncEventArgs e)
        {
            Console.WriteLine(e.RemoteEndPoint + " ------ " + e.BytesTransferred);

            Array.Clear(e.Buffer, 0, e.Count);


            //start receiving data again
            (sender as Socket).ReceiveFromAsync(e);
            
        }
    }
}
