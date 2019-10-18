using Lidgren.Network;
using multi.Network;
using System;
using System.Linq;

namespace Server.States
{
    internal class MenuOpcodeSheet : IPacketOpCodeSheet
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

                        GameRoom room = NetworkSessionContainer.NetworkSessions.GameRooms.Where(x => x.Name == joinPacketInfo.RoomName).FirstOrDefault();

                        //TESTA JAK NIE MA POKOJU TWORZYMY GO NASTEPNE CLIENTY BEDA SIE DO NIEGO ŁACZYC
                        if(room == null)
                        {
                            room = new GameRoom(NetworkSessionContainer.NetworkSessions.UserSessions.Find(x => x.Connection == msg.SenderConnection), joinPacketInfo.RoomName, 8);
                            NetworkSessionContainer.NetworkSessions.GameRooms.Add(room);
                            Console.WriteLine($"Gracz stworzył pokój o nazwie {room.Name} ID {room.Master.ID}");
                            break;
                        }

                        UserSession sendingUser = NetworkSessionContainer.NetworkSessions.UserSessions
                            .Where(x => x.Connection == msg.SenderConnection).FirstOrDefault();

                        if (room != null)
                        {
                            if (room.Join(sendingUser)) // jak user sie nie podłączył, wysyła wiadomość osiągnięto limit graczy
                            {
                                Console.WriteLine($"Dołączył do pokoju {sendingUser.ID}");
                                //Poinformowanie uzytkownika o tym że pokuj jest pełny

                                room.Start(room.Master); //USUNAĆ
                            }
                        }
                        else
                        {
                            //Poinformowanie uzytkownika o nie odnaleźieniu pokoju
                        }

                        break;
                    }
                case CreateRoom.OpCode:
                    {
                        CreateRoom createRoomPacke = new CreateRoom();
                        createRoomPacke.Private=msg.ReadBoolean();
                        createRoomPacke.Name = msg.ReadString();
                        Console.WriteLine("próba ytworzenia room");
                        if (!NetworkSessionContainer.NetworkSessions.GameRooms.Any(x=>x.Master.Connection==msg.SenderConnection || x.PlayersInRoomCollection.Any(y=>y.Connection== msg.SenderConnection)))
                        {
                            Console.WriteLine($"Stworzono room \nNazwa:{createRoomPacke.Name}\nPrzez:{NetworkSessionContainer.NetworkSessions.UserSessions.Where(x=>x.Connection==msg.SenderConnection).FirstOrDefault().ID}");
                        }
                        break;
                    }
            }
            return true;
        }
    }
}
