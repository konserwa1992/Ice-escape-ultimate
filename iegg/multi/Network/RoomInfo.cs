using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.Network
{
    class RoomInfo
    {
        public int RoomID;
        public string Name;
        public string MasterName; //Pamiętać Shared służy po to aby dzielic clienta i serwer. Znaczy to tyle że shared nie dzieli kodu od Serwera.
    }
}
