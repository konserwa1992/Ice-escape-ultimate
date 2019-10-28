using Engine.GameUtility.Map;
using Lidgren.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.States;

namespace Server
{
    class GameRoom
    {
        public List<UserSession> PlayersInRoomCollection = new List<UserSession>();
        public UserSession Master;
        public int MaxPlayers { get; private set; }
        public Map GameMap { get; set; }
        public string Name { get; set; }

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

        public void Update()
        {
            foreach (UserSession singlePlayer in PlayersInRoomCollection)
            {
                singlePlayer.PacketSheetState.Update();
            }
            //Tu update ze statusu jaki maja uzytkownicy
        }
    }
}
