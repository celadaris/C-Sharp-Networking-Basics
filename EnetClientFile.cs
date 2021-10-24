using System;
using System.Threading;
using ENet;
using NetStack.Serialization;

namespace EnetClient
{
    class EnetClientFile
    {
		static Event netEvent;
		static Peer peer;


		static void SendMethod()
        {
			Packet packet = default(Packet);
			byte[] data = new byte[20];
			BitBuffer bitBuffer = new BitBuffer();
			bitBuffer.AddUInt(69);
			bitBuffer.ToArray(data);
			bitBuffer.Clear();
			packet.Create(data, data.Length, PacketFlags.None);
			peer.Send(0, ref packet);
			Console.WriteLine("sent...");
		}

        static void Main(string[] args)
        {
			ENet.Library.Initialize();

			using (Host client = new Host())
			{
				Address address = new Address();

				address.SetHost("localhost");
				address.Port = 23002;
				client.Create();

				peer = client.Connect(address);

				while (!Console.KeyAvailable)
				{
					bool polled = false;

					//SendMethod();

					while (!polled)
					{
						if (client.CheckEvents(out netEvent) <= 0)
						{
							if (client.Service(5, out netEvent) <= 0)
								break;

							polled = true;
						}


						switch (netEvent.Type)
						{
							case EventType.None:
								break;
								
							case EventType.Connect:
								Console.WriteLine("Client connected to server");

								SendMethod();
								break;

							case EventType.Disconnect:
								Console.WriteLine("Client disconnected from server");
								break;

							case EventType.Timeout:
								Console.WriteLine("Client connection timeout");
								break;

							case EventType.Receive:
								Console.WriteLine("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
								
								BitBuffer bitBuffer = new BitBuffer();
								byte[] readbuff = new byte[20];
								netEvent.Packet.CopyTo(readbuff);
								bitBuffer.FromArray(readbuff, readbuff.Length);
								Console.WriteLine(bitBuffer.ReadUInt());
								bitBuffer.Clear();

								netEvent.Packet.Dispose();
								break;
						}
					}
				}

                peer.DisconnectNow(0);
				client.Flush();
				ENet.Library.Deinitialize();
			}
		}
    }
}
