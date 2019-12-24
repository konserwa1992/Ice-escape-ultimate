using Lidgren.Network;
using multi.Network;
using System;
using System.Linq;

namespace Server.States
{
    public class MenuOpcodeSheet : IPacketOpCodeSheet
    {
        public GameRoom UserRoom { get; set; }
        public UserSession Current { get; set; }


        public MenuOpcodeSheet()
        {
            //Narazie nie obsługiwane najpierw zrobie dołączanie do pokoju a potem bede sie
           /* NetOutgoingMessage roomList = Current.Connection.Peer.CreateMessage();
            roomList.WriteAllProperties(NetworkSessionContainer.NetworkSessions.GameRooms);*/
        }


        /// <summary>
        /// Medoda wysyła do użytkownika liste dostępnych pokoji z Adresem OpCode 9047
        /// </summary>
        public void GetRoomList()
        {

        }

        public bool Process(NetIncomingMessage msg)
        {
            msg.Position = 0;
            short opcode = msg.ReadInt16();
   
            switch (opcode)
            {
                case JoinRoomPacket.OpCode:
                    {
                        JoinRoomPacket joinPacketInfo = new JoinRoomPacket(msg.ReadString());

                        GameRoom room= NetworkSessionContainer.NetworkSessions.GameRooms.FirstOrDefault();//= NetworkSessionContainer.NetworkSessions.GameRooms.Where(x => x.Name == joinPacketInfo.RoomName).FirstOrDefault();

                        UserSession sendingUser = NetworkSessionContainer.NetworkSessions.UserSessions
                            .Where(x => x.Connection == msg.SenderConnection).FirstOrDefault();

                        if (room != null)
                        {
                           /*if (room.Join(sendingUser)) // jak user sie nie podłączył, wysyła wiadomość osiągnięto limit graczy
                            {
                                Console.WriteLine($"Dołączył do pokoju {sendingUser.ID}");
                                //Poinformowanie uzytkownika o tym że pokuj jest pełny

                                room.Start(room.Master); //USUNAĆ
                            }*/
                        }
                        else
                        {
                            //Poinformowanie uzytkownika o nie odnaleźieniu pokoju
                        }

                        break;
                    }
                case CreateRoomPacket.OpCode:
                    {
                        CreateRoomPacket createRoomPacke = new CreateRoomPacket();
                        createRoomPacke.Private=msg.ReadBoolean();
                        createRoomPacke.Name = msg.ReadString();
                        Console.WriteLine("próba ytworzenia room");
                       // if (!NetworkSessionContainer.NetworkSessions.GameRooms.Any(x=>x.Master.Connection==msg.SenderConnection || x.PlayersInRoomCollection.Any(y=>y.Connection== msg.SenderConnection)))
                        {
                            Console.WriteLine($"Stworzono room \nNazwa:{createRoomPacke.Name}\nPrzez:{NetworkSessionContainer.NetworkSessions.UserSessions.Where(x=>x.Connection==msg.SenderConnection).FirstOrDefault().ID}");
                        }
                        break;
                    }
            }
            return true;
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public bool Process(IGameState gameState, NetIncomingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
