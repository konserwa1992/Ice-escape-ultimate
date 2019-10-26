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
        public int StopWatch { get; set; }

        public GameRoom(UserSession master,int maxPlayers)
        {
            Master = master;
            PlayersInRoomCollection.Add(Master);
            MaxPlayers = maxPlayers;
        }

        public bool Join(UserSession player)
        {
            if (PlayersInRoomCollection.Count <= MaxPlayers)
            {
                //Zmiana Parsowania pakietów na Room(Nie istnieje jeszcze)
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
                LoandMap(1);
                foreach (UserSession singlePlayer in PlayersInRoomCollection)
                {
                    singlePlayer.PacketSheetState = new GameOpcodeSheet();  
                    //Zmiana Parsowania pakietów na GRE(Nie istnieje jeszcze)
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

        public void Update(NetIncomingMessage msg)
        {
            //Tu update ze statusu jaki maja uzytkownicy
            foreach (UserSession user in PlayersInRoomCollection)
            {
                user.PacketSheetState.RecivedPacket(msg);
            }
        }
    }
}
