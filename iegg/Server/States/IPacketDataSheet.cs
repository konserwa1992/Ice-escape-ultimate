using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.States
{
    interface IPacketOpCodeSheet
    {
        GameRoom UserRoom { get; set; }
        UserSession Current { get; set; }

        bool Process(NetIncomingMessage msg);
    } 
}
