using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
        }

        public static void UdpPlayerMoved(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Player (ID: {_fromClient}) just moved.");

            float _myX = _packet.ReadFloat();
            float _myY = _packet.ReadFloat();
            float _myZ = _packet.ReadFloat();
            Server.clients[_fromClient].player.position = new Vector3(_myX, _myY, _myZ);

            ServerSend.udpPlayerMovement(Server.clients[_fromClient].player);
        }
    }
}
