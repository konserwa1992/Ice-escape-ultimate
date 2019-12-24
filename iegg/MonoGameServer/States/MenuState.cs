using Lidgren.Network;
using multi.Network;
using System;
using System.Linq;

namespace Server.States
{
    public class MenuState : IGameState
    {
        public void Recive(NetIncomingMessage msg)
        {
            msg.Position = 0;
            short opcode = msg.ReadInt16();

            switch (opcode)
            {
                case JoinRoomPacket.OpCode:
                    {
                        JoinRoomPacket joinPacketInfo = new JoinRoomPacket("");
                        string name = msg.ReadString();

                        GameRoom
                            GameStateWithRooms = NetworkSessionContainer.NetworkSessions.GameRooms.Where(x => x.Room.Name == name).FirstOrDefault();

                        UserSession sendingUser = NetworkSessionContainer.NetworkSessions.UserSessions
                            .Where(x => x.Connection == msg.SenderConnection).FirstOrDefault();

                        if (GameStateWithRooms != null)
                        {
                            if (GameStateWithRooms.Room.Join(sendingUser)) // jak user sie nie podłączył, wysyła wiadomość osiągnięto limit graczy
                            {
                                Console.WriteLine($"Dołączył {sendingUser.Name} do pokoju {GameStateWithRooms.Room.Name} ");
                                sendingUser.UserGameState = new GameRoomState(sendingUser, GameStateWithRooms);
                            }
                        }
                        else
                        {
                            //Poinformowanie uzytkownika o nie odnaleźieniu pokoju
                        }

                        if(GameStateWithRooms.Room.RoomMember.Count==2)
                        {
                            Console.WriteLine($"{GameStateWithRooms.Room.Name} Wystartował ");
                            GameStateWithRooms.Start();
                        }

                        break;
                    }

                case CreateRoomPacket.OpCode:
                    {
                        CreateRoomPacket newRoom = new CreateRoomPacket();
                        msg.ReadAllProperties(newRoom);

                        if (!NetworkSessionContainer.NetworkSessions.GameRooms.Any(x =>
                            x.Room.RoomMember.Exists(y => y.Connection == msg.SenderConnection) == true))
                        {
                            UserSession masterSession =
                                NetworkSessionContainer.NetworkSessions.UserSessions.FirstOrDefault(x =>
                                    x.Connection == msg.SenderConnection);
                            GameRoom newGameRoom = new GameRoom(masterSession, newRoom.Name, 8);

                            newGameRoom.Room.Master = masterSession;
                            //Wyciek pamięci przed zmianą statusu ostatniego gracza trzeba usunąć element z listy
                            //Dać też heart beat jak sypnie to dowidzenia
                            masterSession.UserGameState = new GameRoomState(masterSession, newGameRoom);
                            NetworkSessionContainer.NetworkSessions.GameRooms.Add(newGameRoom);
                        }

                        break;
                    }
            }
        }

        public void Update()
        {
          //  throw new NotImplementedException();
        }
    }
}
