using Engine.GameUtility.Map;
using Lidgren.Network;
using multi.Network;
using Newtonsoft.Json;
using Server.Moduls;
using Server.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Server
{
    internal class GameRoom
    {
        #region Usunięte
        /*public List<UserSession> PlayersInRoomCollection = new List<UserSession>();
        public UserSession Master;
        public int MaxPlayers { get; private set; }
        public Map GameMap { get; set; }
        public string Name { get; set; }
        public IPacketOpCodeSheet PacketSheetState { set; get; }
        private Stopwatch SendPositionPacketInterval = new Stopwatch();


        public GameRoom(UserSession master, string name, int maxPlayers)
        {
            Master = master;
            Name = name;
            PlayersInRoomCollection.Add(Master);
            MaxPlayers = maxPlayers;
        }

        public bool Join(UserSession player)
        {
            if (PlayersInRoomCollection.Count <= MaxPlayers)
            {
                //Zmiana Parsowania pakietów na Room(Nie istnieje jeszcze) i można by przy tej akcji dać Pakiet zwrotny do clienta
                PlayersInRoomCollection.Add(player);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void Start(UserSession UserWhoWantStart)
        {
            if (UserWhoWantStart == Master)
            {
            //    LoandMap(1);
                foreach (UserSession singlePlayer in PlayersInRoomCollection)
                {
                    singlePlayer.PacketSheetState = new GameOpcodeSheet(singlePlayer,this);  
                    //Zmiana Parsowania pakietów na GRE(Nie istnieje jeszcze)
                }


                foreach (UserSession otherPlayers in PlayersInRoomCollection)
                {
                    foreach (UserSession playersList in PlayersInRoomCollection)
                    {
                        NetOutgoingMessage SendToCurrentPlayerAboutPlayers = playersList.Connection.Peer.CreateMessage();
                        SendToCurrentPlayerAboutPlayers.Write((short)2620);
                        SendToCurrentPlayerAboutPlayers.Write(otherPlayers.ID, 32);
                        SendToCurrentPlayerAboutPlayers.Write("konserwa");
                        playersList.Connection.SendMessage(SendToCurrentPlayerAboutPlayers, NetDeliveryMethod.UnreliableSequenced, SendToCurrentPlayerAboutPlayers.LengthBytes);
                    }
                }


                SendPositionPacketInterval.Start();
            }
        }

        void LoandMap(int id)
        {
            StreamReader MapWriter = new StreamReader("C:\\pasta\\Map0.json");
            GameMap = JsonConvert.DeserializeObject<Map>(MapWriter.ReadToEnd(),
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            MapWriter.Close();
        }

        public void RecivePacket()
        {

        }

        public void Update()
        {
            if (SendPositionPacketInterval.ElapsedMilliseconds > (100))
            {
                foreach (UserSession singlePlayer in PlayersInRoomCollection)
                {
                    singlePlayer.PacketSheetState.
                }

                SendPositionPacketInterval.Reset();
                SendPositionPacketInterval.Start();
            }


            foreach (UserSession singlePlayer in PlayersInRoomCollection)
            {
                singlePlayer.PacketSheetState.Update();
            }
            //Tu update ze statusu jaki maja uzytkownicy
        }*/
        #endregion

        public Room Room { get; private set; }
        private Stopwatch SendPositionInterval { get; set; } = new Stopwatch();
        private Stopwatch GameTimeInterval { get; set; } = new Stopwatch();
        public bool Started = false;
        public Map Map { get; private set; }

        public GameRoom(UserSession master, string name, int maxPlayers)
        {
            Room = new Room(name,master);
        }

        public void Start()
        {
            foreach(UserSession user in Room.RoomMember)
            {
                user.UserGameState = new InMatchState(user,this);

                foreach(UserSession spawnPlayer in Room.RoomMember)
                {
                    NetOutgoingMessage SendAllPlayersPositionToCurrentSession = user.Connection.Peer.CreateMessage();
                    SendAllPlayersPositionToCurrentSession.Write(SpawnPacket.OpCode);
                    SendAllPlayersPositionToCurrentSession.Write(spawnPlayer.ID);
                    user.Connection.SendMessage(SendAllPlayersPositionToCurrentSession, NetDeliveryMethod.UnreliableSequenced, SendAllPlayersPositionToCurrentSession.LengthBytes);
                }
            }

            StreamReader MapWriter = new StreamReader("pasta\\Map0.json");
            Map objectMap = JsonConvert.DeserializeObject<Map>(MapWriter.ReadToEnd(),
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            MapWriter.Close();

            SendPositionInterval.Start();
            Started = true;
        }


        public void Recive(NetIncomingMessage msg)
        {
            //Tu będzie można zrobić start Gry jak już wszyscy sie zaladuja 
        }

        public void Update()
        {
            if(Started)
            {
                if (SendPositionInterval.ElapsedMilliseconds > 100)
                {
                    foreach (UserSession spawnPlayer in Room.RoomMember)
                    {
                        ((InMatchState)spawnPlayer.UserGameState).SendMovePacket();
                    }
                    SendPositionInterval.Reset();
                    SendPositionInterval.Start();
                }

                if(GameTimeInterval.ElapsedMilliseconds > 16)
                {
                    foreach (UserSession spawnPlayer in Room.RoomMember)
                    {
                        ((InMatchState)spawnPlayer.UserGameState).Update();
                        Map.MapPath[0].FloorPolygon.IsCollide(spawnPlayer.CollisionObject);
                    }

                    GameTimeInterval.Reset();
                    GameTimeInterval.Start();
                }
            }
        }
    }
}
