using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using multi.Network;

namespace Server.States
{
    class MenuOpcodeSheet : IPacketOpCodeSheet
    {
        public GameRoom UserRoom { get; set; }
        public UserSession Current { get; set; }

        public bool RecivedPacket(NetIncomingMessage msg)
        {
            short opcode = msg.ReadInt16();
            if (opcode == 3754) //Dołączanie do pokoju
            {

                JoinRoomPacket joinPacketInfo = new JoinRoomPacket();
                msg.ReadAllProperties((object)joinPacketInfo);

                GameRoom room = NetworkSessionContainer.NetworkSessions.GameRooms.Where(x => x.Name == joinPacketInfo.RoomName).FirstOrDefault();
                UserSession sendingUser = NetworkSessionContainer.NetworkSessions.UserSessions
                    .Where(x => x.Connection == msg.SenderConnection).FirstOrDefault();
                if (room != null)
                {
                    if (!room.Join(sendingUser)) // jak user sie nie podłączył, wysyła wiadomość osiągnięto limit graczy
                    {
                        //Poinformowanie uzytkownika o tym że pokuj jest pełny

                    }
                }
                else
                {
                    //Poinformowanie uzytkownika o nie odnaleźieniu pokoju
                }
            }

            return true;
        }
    }
}
