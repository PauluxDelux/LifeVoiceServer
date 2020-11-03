using System;
using System.Collections.Generic;

namespace GameServer
{
    class ServerHandle
    {

        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int clientIdCheck = _packet.ReadInt();
            string username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected with success and is now player {_fromClient}.");
            if (_fromClient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: {_fromClient}) has assumed the wrong ID: ({clientIdCheck}).");
            }
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {   
            Console.WriteLine("DEBUG UDP PACKET");
            Console.WriteLine($"Received a Packet Length: {_packet.Length()}.");

            _packet.ReadInt();
            string message = _packet.ReadString();
            Console.WriteLine($"UDP packet received: {message}.");
        }

        public static void UDPAtlernative(int _fromClient, Packet _packet)
        {
            string message = _packet.ReadString();
            Console.WriteLine($"UDP Alternative packet received: {message}.");
        }

    }
}