using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using NETGame;

namespace Server.States
{
    class InMatchState : IGameState
    {
        public UserSession User { get; set; }
        public GameRoom GameRoom { get; set; }
        private float sin = 0.0f;
        
        public InMatchState(UserSession user, GameRoom gameRoom)
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
                        User.position = new Vector2(move.X, move.Y);
                        Console.WriteLine($"{User.ID} X:{move.X} Y:{move.Y}");
                        break;
                    }
            }
        }

        public void SendMovePacket()
        {
            sin += 0.25f;
            Vector2 v = new Vector2(sin * 5, -(float)Math.Cos(sin) * 32);
            foreach (UserSession otherPlayers in GameRoom.Room.RoomMember)
            {
                NetOutgoingMessage SendAllPlayersPositionToCurrentSession = User.Connection.Peer.CreateMessage();
                SendAllPlayersPositionToCurrentSession.Write(MovePacket.OpCode);
                SendAllPlayersPositionToCurrentSession.Write(otherPlayers.ID);
                SendAllPlayersPositionToCurrentSession.Write(otherPlayers.position.X);
                SendAllPlayersPositionToCurrentSession.Write(otherPlayers.position.Y);
                User.Connection.SendMessage(SendAllPlayersPositionToCurrentSession, NetDeliveryMethod.UnreliableSequenced, SendAllPlayersPositionToCurrentSession.LengthBytes);
               //Console.WriteLine($"ID:{User.ID} UPDATE_USER_ID:{otherPlayers.ID}");
            }
        }

        public void Update()
        {
        }
    }
}
