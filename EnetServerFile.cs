using System;
using ENet;

namespace EnetServer
{
    class EnetServerFile
    {
		static byte[] buffer = new byte[1024];

		static void Main(string[] args)
        {
			ENet.Library.Initialize();

			Host server = new Host();
			
				Address address = new Address();

				address.Port = 23002;
				server.Create(address, 100);

				Event netEvent;

				while (!Console.KeyAvailable)
				{
					bool polled = false;

					while (!polled)
					{
						if (server.CheckEvents(out netEvent) <= 0)
						{
							if (server.Service(15, out netEvent) <= 0)
								break;

							polled = true;
						}

						switch (netEvent.Type)
						{
							case EventType.None:
								break;

							case EventType.Connect:
								Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
								break;

							case EventType.Disconnect:
								Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
								break;

							case EventType.Timeout:
								Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
								break;

							case EventType.Receive:
								Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
								//netEvent.Packet.CopyTo(buffer);
								//Console.WriteLine(buffer[0]);
								netEvent.Packet.Dispose();
								break;
						}
					}
				}

				server.Flush();
				ENet.Library.Deinitialize();
			

		}
    }
}
