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

        public static void Welcome(int _toClient, string _message)
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome))
            {
                packet.Write(_message);
                packet.Write(_toClient);

                SendTCPData(_toClient, packet);
            }
        }
    }
}