using System.Numerics;

namespace GameServer{

    class Player{

        public int id;
        public string username;
        public Vector3 position;
        
        public Player(int _id, string _username, Vector3 _position){
            this.id = _id;
            this.username = _username;
            this.position = _position;
        }

    }
}