using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
    public class RevivedPlayerPacket
    {
        public const short OpCode = 6825;
        //ID gracza możliwe że w przyszłości będe chaił dodac im oddzielne kolory
        public int ID { get; set; }
    }
}
