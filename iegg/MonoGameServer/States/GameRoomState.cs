using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using NETGame;

namespace Server.States
{
    public class GameRoomState : IGameState
    {
        public UserSession User { get; set; }
        public GameRoom GameRoom { get; set; }

        public GameRoomState(UserSession user,GameRoom gameRoom)
        {
            User = user;
            GameRoom = gameRoom;
        }

        public void Recive(NetIncomingMessage msg)
        {
            msg.Position = 0;
            short opcode = msg.ReadInt16();

            switch (opcode)
            {
                case MovePacket.OpCode:
                    {
                        MovePacket move = new MovePacket();
                        move.X = msg.ReadFloat();
                        move.Y = msg.ReadFloat();


                        Console.WriteLine($"{User.ID} X:{move.X} Y:{move.Y}");
                        break;
                    }

                case RoomStartGamePacket.OpCode:
                    {
                        if (GameRoom.Room.Master == User)
                        {
                            GameRoom.Start();
                        }else
                        {
                            //logowanie że cos jest nie tak
                        }
                        break;
                    }
            }
        }

        public void Update()
        {
  
        }
    }
}
