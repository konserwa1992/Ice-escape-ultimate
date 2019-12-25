using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
    /// <summary>
    /// ŁAZARZ
    /// Informacja dla graczy o tym że jeden z drużyny zginą
    /// w tym miejscu pojawi sie punkt który będzie umożliwiał wskrzeszenie
    /// to czy sie wskrzesza zależeć będzie od serwera
    /// </summary>
    public class ResurrectPointAddPacket
    {
        public const short OpCode = 6824;
        //ID gracza możliwe że w przyszłości będe chaił dodac im oddzielne kolory
        public int ID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
