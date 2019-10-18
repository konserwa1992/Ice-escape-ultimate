using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETGame
{
    public class MovePacket
    {
        public const short OpCode = 6066;
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
