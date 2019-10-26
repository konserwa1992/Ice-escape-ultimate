using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{ 
    interface IPacket
    {
       short OpCode { get; set; }
    }
}
