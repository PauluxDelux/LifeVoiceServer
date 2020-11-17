using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace GameServer
{
    class Server
    {

        public static int game_max_players
        {
            get;
            private set;
        }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;
        public static int server_port
        {
            get;
            private set;
        }

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void Start(int _game_max_players, int _port)
        {
            game_max_players = _game_max_players;
            server_port = _port;

            Console.WriteLine($"Starting server ...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, server_port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            udpListener = new UdpClient(server_port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server started on Port {server_port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

            for (int i = 1; i <= game_max_players; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(tcpClient);
                    return;
                }
            }

            Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} failed to connect: Server is full.");
        }

        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                using (Packet packet = new Packet(data))
                {
                    int clientId = packet.ReadInt();

                    if (clientId == 0)
                    {
                        return;
                    }

                    if (clients[clientId].udp.endPoint == null)
                    {
                        clients[clientId].udp.Connect(clientEndPoint);
                        return;
                    }

                    if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        clients[clientId].udp.HandleData(packet);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error while receiving UDP data: {exception}");
            }
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    Console.WriteLine($"[DEBUG] UDP packet length: {_packet.Length()}");
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error while sending data to client {_clientEndPoint} through UDP: {exception}");
            }
        }
        private static void InitializeServerData()
        {
            for (int i = 1; i <= game_max_players; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int) ClientPackets.udpTestReceived, ServerHandle.UDPTestReceived }
            };
            Console.WriteLine($"Initialized packets");
        }
    }
}