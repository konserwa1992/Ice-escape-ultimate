using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Server.States
{
    interface IGameState
    {
        void Recive(NetIncomingMessage msg);
        void Update();
    }
}
