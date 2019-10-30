using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server.States
{
    class GameRoomState : IGameState
    {
        public IPacketOpCodeSheet PacketParserSheet { get; set; }
        public UserSession User { get; set; }

        public GameRoomState(UserSession user)
        {
            User = user;
        }

        public void Recive(NetIncomingMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
