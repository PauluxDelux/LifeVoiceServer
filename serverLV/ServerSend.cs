using System;
using System.Collections;

namespace GameServer
{
    class ServerSend
    {

        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.game_max_players; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void SendTCPDataToAllExceptOne(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.game_max_players; i++)
            {
                if (i != _toClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.game_max_players; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void SendUDPDataToAllExceptOne(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.game_max_players; i++)
            {
                if (i != _toClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packet
        public static void Welcome(int _toClient, string _message)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(_message);
                packet.Write(_toClient);

                SendTCPData(_toClient, packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet packet = new Packet((int)ServerPackets.udpTest))
            {
                packet.Write("A test packet to check UDP protocol");

                SendUDPData(_toClient, packet);
            }
        }
        #endregion
    }
}