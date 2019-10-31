using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Moduls
{
    class Room
    {
        public List<UserSession> RoomMember = new List<UserSession>();
        public UserSession Master;
        public string Name;
        public int MaxPlayers = 8;

        public Room(string name, UserSession master)
        {
            Master = master;
            Name = name;
        }


        public bool Join(UserSession user)
        {
            if (RoomMember.Count < MaxPlayers)
            {
                RoomMember.Add(user);
                return true;
            }
            return false;
        }

        public void Remove(UserSession user)
        {
            RoomMember.Remove(user);
        }
    }
}
