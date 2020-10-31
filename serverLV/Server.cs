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
        public static int server_port
        {
            get;
            private set;
        }

        private static TcpListener tcpListener;

        public static void Start(int _game_max_players, int _port)
        {
            game_max_players = _game_max_players;
            server_port = _port;

            Console.WriteLine($"Starting server ...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, server_port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Server started on Port {server_port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
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

        private static void InitializeServerData()
        {
            for (int i = 1; i <= game_max_players; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}