using Lidgren.Network;
using NETGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.States
{
    class GameOpcodeSheet : IPacketOpCodeSheet
    {
        public GameRoom UserRoom {get; set; }
        public UserSession Current { get; set; }

        public GameOpcodeSheet(UserSession current, GameRoom room)
        {
            this.Current = current;
            UserRoom = room;


            foreach (UserSession otherPlayers in UserRoom.PlayersInRoomCollection)
            {
                foreach(UserSession user in UserRoom.PlayersInRoomCollection)
                {
                    if (user != otherPlayers) {
                        NetOutgoingMessage SendToCurrentPlayerAboutPlayers = otherPlayers.Connection.Peer.CreateMessage();
                        SendToCurrentPlayerAboutPlayers.Write((short)2620);
                        SendToCurrentPlayerAboutPlayers.Write(user.ID, 32);
                        SendToCurrentPlayerAboutPlayers.Write("konserwa");
                        otherPlayers.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                    }
                    //Console.WriteLine($"Wysyłam pakiet od {otherPlayers.ID} wysyłam dane o {session.ID}");
                }


                //  Console.WriteLine(BitConverter.ToString(SendToCurrentPlayerAboutPlayers.Data));
            }
        }

        public bool Process(NetIncomingMessage msg)
        {
            msg.Position = 0;
            short opcode = msg.ReadInt16();

            if (opcode == MovePacket.OpCode)
            {
                MovePacket move = new MovePacket();
                move.X = msg.ReadFloat();
                move.Y = msg.ReadFloat();

                foreach (UserSession otherPlayers in UserRoom.PlayersInRoomCollection)
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
                }
            }

            return false;
        }
    }
}
