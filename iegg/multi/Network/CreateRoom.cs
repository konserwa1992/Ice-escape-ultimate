using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
    public class CreateRoom
    {
        public CreateRoom()
        {

        }

        public CreateRoom(string name, bool privateRoom)
        {
            Private = privateRoom;
            Name = name;
        }

        public const short OpCode = 8008;
        public bool Private { get; set; }
        public string Name { get; set; }
    }
}
