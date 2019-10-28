using Lidgren.Network;
using Microsoft.Xna.Framework;
using NETGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.States
{
    class GameOpcodeSheet : IPacketOpCodeSheet
    {
        public GameRoom UserRoom {get; set; }
        public Vector2 ForwardVector { get; set; } = Vector2.Zero;
        public PlayerClass playerInfo { get; set; } = new PlayerClass();
        public UserSession Current { get; set; }
        private Stopwatch SendPositionPacketInterval = new Stopwatch();

        public GameOpcodeSheet(UserSession current, GameRoom room)
        {
            this.Current = current;
            UserRoom = room;

            SendPositionPacketInterval.Start();
        }

        float sin = 0;

        public void SendMovePacket()
        {
            if (SendPositionPacketInterval.ElapsedMilliseconds > (100))
            {
                sin+=0.55f;
                playerInfo.CurrPosition = new Vector2(sin*5, -(float)Math.Cos(sin) * 32);
                //To spawn
                foreach (UserSession otherPlayers in UserRoom.PlayersInRoomCollection)
                {
                        NetOutgoingMessage SendToCurrentPlayerAboutPlayers = otherPlayers.Connection.Peer.CreateMessage();
                        SendToCurrentPlayerAboutPlayers.Write(MovePacket.OpCode);
                        SendToCurrentPlayerAboutPlayers.Write(Current.ID);
                        SendToCurrentPlayerAboutPlayers.Write(playerInfo.CurrPosition.X);
                        SendToCurrentPlayerAboutPlayers.Write(playerInfo.CurrPosition.Y);
                        otherPlayers.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                        Console.WriteLine($"ID:{Current.ID} POS: X{playerInfo.CurrPosition.X};Y{playerInfo.CurrPosition.Y}");
                    }
                SendPositionPacketInterval.Reset();
                SendPositionPacketInterval.Start();
            }

        }
        

        public bool Process(NetIncomingMessage msg)
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

                     //   ForwardVector = new Vector2(move.X, move.Y);

                        /*foreach (UserSession otherPlayers in UserRoom.PlayersInRoomCollection)
                        {
                            if (otherPlayers.Connection != msg.SenderConnection && otherPlayers.ID != Current.ID)
                            {
                                NetOutgoingMessage SendToCurrentPlayerAboutPlayers = otherPlayers.Connection.Peer.CreateMessage();
                                SendToCurrentPlayerAboutPlayers.Write(MovePacket.OpCode);
                                SendToCurrentPlayerAboutPlayers.Write(Current.ID);
                                SendToCurrentPlayerAboutPlayers.Write(move.X);
                                SendToCurrentPlayerAboutPlayers.Write(move.Y);
                                otherPlayers.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                                //Console.WriteLine($"ID:{Current.ID} POS: X{move.X};Y{move.Y}");
                            }
                        }*/
                        break;
                    }
                case 2620:
                    {


                        break;
                    }
            }

            

            return false;
        }

        public void Update()
        {
            SendMovePacket();
        }
    }
}
