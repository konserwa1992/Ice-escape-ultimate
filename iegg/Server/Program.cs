using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using multi.Network;
using NETGame;
using Server.States;


namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new NetPeerConfiguration("application name")
            { Port = 12345 };
            var server = new NetServer(config);
            server.Start();
     
            // server

            NetIncomingMessage msg;

            GameRoom newGameRoom = new GameRoom(null, "TEST", 8);
            NetworkSessionContainer.NetworkSessions.GameRooms.Add(newGameRoom);


            while (true)
            {
                //Ustawić odświerzanie co 16.666ms
                foreach (UserSession session in NetworkSessionContainer.NetworkSessions.UserSessions)
                {
                    session.UserGameState.Update();
                }

                if ((msg = server.ReadMessage())==null) continue;


                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        {
                            Console.WriteLine("Connected");
                            break;
                        }
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.StatusChanged:
                        {
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            break;
                        }
                    case NetIncomingMessageType.Data:
                        {

                            short opcode = msg.ReadInt16();
                            if(opcode==2000) //Logowanie użytkownika
                            {
                               unsafe
                                {
                                    if (!NetworkSessionContainer.NetworkSessions.UserSessions.Exists(x => x.Connection == msg.SenderConnection))
                                    {
                                        UserSession session = new UserSession();
                                        TypedReference tr = __makeref(session);
                                        IntPtr ptr = **(IntPtr**) (&tr);
                                        Console.WriteLine(ptr);
                                        session.Connection = msg.SenderConnection;
                                        session.ID = ptr.ToInt32();
                                        session.Name = msg.ReadString();

                                        NetOutgoingMessage outMessage = session.Connection.Peer.CreateMessage();
                                        outMessage.Write((short) 2000);
                                        outMessage.Write(session.ID);
                                        session.Connection.SendMessage(outMessage, NetDeliveryMethod.UnreliableSequenced,
                                            outMessage.LengthBytes);
                                        session.UserGameState = new MenuState();
                                        NetworkSessionContainer.NetworkSessions.UserSessions.Add(session);
                                        // Musze dorobić jakąś obsługe menu
                                     
                                    }
                                    else
                                    {
                                        //Zaimplementować że jest już taki gość
                                    }
                                   
                                    //TO NIE SPAWN A LOGIN
                                    /*foreach (UserSession otherPlayers in UsersSessions.Sessions)
                                    {
                                        NetOutgoingMessage informAboutPlayer = session.Connection.Peer.CreateMessage();
                                        informAboutPlayer.Write((short)2620);
                                        informAboutPlayer.Write(otherPlayers.ID, 32);
                                        informAboutPlayer.Write("konserwa");
                                        session.Connection.SendMessage(informAboutPlayer, NetDeliveryMethod.UnreliableSequenced, informAboutPlayer.LengthBytes);
                                        Console.WriteLine($"Wysyłam pakiet od {session.ID} wysyłam dane o {otherPlayers.ID}");
                                        //Console.WriteLine(BitConverter.ToString(informAboutPlayer.Data));


                                        NetOutgoingMessage SendToCurrentPlayerAboutPlayers = otherPlayers.Connection.Peer.CreateMessage();
                                        SendToCurrentPlayerAboutPlayers.Write((short)2620);
                                        SendToCurrentPlayerAboutPlayers.Write(session.ID, 32);
                                        SendToCurrentPlayerAboutPlayers.Write("konserwa");
                                        otherPlayers.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                                        Console.WriteLine($"Wysyłam pakiet od {otherPlayers.ID} wysyłam dane o {session.ID}");
                                      //  Console.WriteLine(BitConverter.ToString(SendToCurrentPlayerAboutPlayers.Data));
                                    }*/

                                }
                            }
                            else
                            {
                           //     NetworkSessionContainer.NetworkSessions.UserSessions.Find(x => x.Connection == msg.SenderConnection).UserGameState.Recive(msg);
                                foreach(UserSession user in NetworkSessionContainer.NetworkSessions.UserSessions)
                                {
                                    if(user.Connection == msg.SenderConnection)
                                    {
                                        user.UserGameState.Recive(msg);
                                    }
                                }
                            }




                            break;
                        }
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    default:
                        {
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;
                        }
                }

                //server.Recycle(msg);
            }

        }
    }
}
