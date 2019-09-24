using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class UserSession
    {
        public int ID { get; set; }
        public NetConnection Connection { get; set; }

        public UserSession(int id,NetConnection connection)
        {
            Connection = connection;
            /* BigInteger l_retval = 0;
             Guid guid = Guid.NewGuid();
             byte[] ba = guid.ToByteArray();
             int i = ba.Count();
             foreach (byte b in ba)
             {
                 l_retval += b * BigInteger.Pow(256, --i);
             }

             ID = l_retval.e;*/
            ID = id;
        }
    }
}
