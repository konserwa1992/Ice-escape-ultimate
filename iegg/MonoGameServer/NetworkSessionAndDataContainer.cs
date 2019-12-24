using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class NetworkSessionContainer
    {
        private readonly static NetworkSessionContainer Instance = new NetworkSessionContainer();

        public static NetworkSessionContainer NetworkSessions
        {
            get { return Instance; }
        }

        public List<UserSession> UserSessions = new List<UserSession>();
        public  List<GameRoom> GameRooms = new List<GameRoom>();
    }
}
