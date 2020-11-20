using System.Numerics;

namespace GameServer{

    class Player{

        public int id;
        public string username;
        public Vector2 position;
        public int rotation;

        public Player(int _id, string _username, Vector2 _position){
            this.id = _id;
            this.username = _username;
            this.position = _position;
            rotation = 0; 
        }

    }
}