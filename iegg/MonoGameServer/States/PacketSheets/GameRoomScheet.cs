using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server.States.PacketSheets
{
    public class GameRoomScheet : IPacketOpCodeSheet
    {
        public GameRoom UserRoom { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UserSession Current { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        public bool Process(IGameState gameState, NetIncomingMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
