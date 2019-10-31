using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
    public class SpawnPacket
    {
        public const short OpCode = 2620;
        public int ID;
        public string Name;
    }
}
