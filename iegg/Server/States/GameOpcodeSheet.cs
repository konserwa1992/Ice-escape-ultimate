using Lidgren.Network;
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
        public UserSession Current { get; set; }

        private Stopwatch SendPositionInterval { get; set; }


        public GameOpcodeSheet()
        {
            SendPositionInterval.Start();
        }

        public void SendPlayerPositions()
        {
            if (SendPositionInterval.ElapsedMilliseconds > (1000.0f / 60.0f))
            {
                    MovePacket move = new MovePacket();

                    foreach (UserSession players in UserRoom.PlayersInRoomCollection)
                    {
                            NetOutgoingMessage SendToCurrentPlayerAboutPlayers = players.Connection.Peer.CreateMessage();
                            SendToCurrentPlayerAboutPlayers.Write((short)6066);
                            SendToCurrentPlayerAboutPlayers.Write(Current.ID);
                            SendToCurrentPlayerAboutPlayers.Write(move.X);
                            SendToCurrentPlayerAboutPlayers.Write(move.Y);
                            players.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                    }

                SendPositionInterval.Reset();
                SendPositionInterval.Start();
            }
        }


        public void Update()
        {
            SendPlayerPositions();
        }

        public bool RecivedPacket(NetIncomingMessage msg)
        {
            short opcode = msg.ReadInt16();

            if (opcode == 6066)
            {
                MovePacket move = new MovePacket();
                move.X = msg.ReadInt32();
                move.Y = msg.ReadInt32();

                foreach (UserSession otherPlayers in UserRoom.PlayersInRoomCollection)
                {
                    if (otherPlayers.Connection != msg.SenderConnection && otherPlayers.ID != Current.ID)
                    {
                        NetOutgoingMessage SendToCurrentPlayerAboutPlayers = otherPlayers.Connection.Peer.CreateMessage();
                        SendToCurrentPlayerAboutPlayers.Write((short)6066);
                        SendToCurrentPlayerAboutPlayers.Write(Current.ID);
                        SendToCurrentPlayerAboutPlayers.Write(move.X);
                        SendToCurrentPlayerAboutPlayers.Write(move.Y);
                        otherPlayers.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                    }
                }
            }

            return false;
        }
    }
}
