using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
   public class JoinRoomPacket
    {
        public const short OpCode = 3754;
        public string RoomName { get; set; }

        public JoinRoomPacket(string roomName)
        {
            this.RoomName = RoomName;
        }
    }
}
