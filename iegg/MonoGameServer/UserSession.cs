using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NETGame;
using Server.States;
using Microsoft.Xna.Framework;
using Engine.GameUtility.Physic;

namespace Server
{
    public class UserSession
    {
        public int ID { get; set; }
        public NetConnection Connection { get; set; }
        public IGameState UserGameState { set; get; }
        public string Name;
        public Vector2 position;
        public ICollider CollisionObject { get; set; }

        public UserSession()
        {
            Random rand = new Random();
            position = new Vector2(rand.Next(0, 256), rand.Next(0, 256));
            CollisionObject = new Circle(position, 15.0f);
            CollisionObject.OnCollision += new CollideDetected(delegate (ICollider item)
            {
                if (item.GetType() == typeof(Polygon))
                {
                    Console.WriteLine($"User out of map {ID}");
                }
            });
        }

        public UserSession(int id,string name,NetConnection connection)
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
            Name = name;
            ID = id;
            Random rand = new Random();
            position = new Vector2(rand.Next(0, 256), rand.Next(0, 256));
        }
    }
}
