using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.States
{
    public interface IPacketOpCodeSheet
    {
        bool Process(IGameState gameState, NetIncomingMessage msg);
    } 
}
