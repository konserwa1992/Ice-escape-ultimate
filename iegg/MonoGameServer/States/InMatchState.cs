using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GameUtility.Map.Elements.FloorType;
using Engine.GameUtility.Physic;
using Lidgren.Network;
using multi.Network;
using Microsoft.Xna.Framework;
using NETGame;

namespace Server.States
{
    public class InMatchState : IGameState
    {
        public UserSession User { get; set; }
        public GameRoom GameRoom { get; set; }
        private float sin = 0.0f;

        private Vector2 DestinationVector = Vector2.Zero;
        private Vector2 Forward = new Vector2(1, 0);
        private int SideMultiplier = 0;
        private Vector2 ClickPosition;
        public Circle PlayerCollide;
        private float prev { get; set; }



        //Usunać bo potem bedzie stały ląd
        private bool PlayerIsReady = false;

        public InMatchState(UserSession user, GameRoom gameRoom)
        {
            User = user;
            GameRoom = gameRoom;
            PlayerCollide = new Circle(User.position, 5);
            PlayerCollide.OnCollision += new CollideDetected(delegate (ICollider item)
                {
                    int i = 0;
                }
            );
        }

        public void Recive(NetIncomingMessage msg)
        {
            msg.Position = 0;
            short opcode = msg.ReadInt16();

            switch (opcode)
            {
                case MovePacket.OpCode:
                    {
                        MovePacket move = new MovePacket();
                        move.X = msg.ReadFloat();
                        move.Y = msg.ReadFloat();
                        //User.position = new Vector2(move.X, move.Y);

                        ClickPosition = new Vector2(move.X, move.Y);

                        DestinationVector = (ClickPosition - User.position);
                        DestinationVector = Vector2.Normalize(DestinationVector);

                        var dotProduct = Vector2.Dot(DestinationVector, Forward);

                        if (Engine.GameUtility.AdditionalMath.MyMath.AngleDir(Forward, DestinationVector) >= 0)
                        {
                            SideMultiplier = -1;
                        }
                        else
                        {
                            SideMultiplier = 1;
                        }

                        PlayerIsReady = true;

                        Console.WriteLine($"{User.ID} X:{move.X} Y:{move.Y}");
                        break;
                    }
            }
        }

        public bool CheckIfPlayerIsDead()
        {
            foreach (IFloor path in GameRoom.Map.MapPath)
            {
                if (path.FloorPolygon.IsCollide(PlayerCollide))
                {
                    User.Alive = false;
                    SendDeathPacket();
                    return true;
                }
                else if (User.Alive != false)
                {
                    User.Alive = true;
                    return false;
                }
            }
            return false;
        }


        public void SendMovePacket()
        {

                sin += 0.25f;
                Vector2 v = new Vector2(sin * 5, -(float) Math.Cos(sin) * 32);
                foreach (UserSession player in GameRoom.Room.RoomMembers)
                {
                    NetOutgoingMessage SendAllPlayersPositionToCurrentSession = User.Connection.Peer.CreateMessage();
                    SendAllPlayersPositionToCurrentSession.Write(MovePacket.OpCode);
                    SendAllPlayersPositionToCurrentSession.Write(player.ID);
                    SendAllPlayersPositionToCurrentSession.Write(player.position.X);
                    SendAllPlayersPositionToCurrentSession.Write(player.position.Y);
                    User.Connection.SendMessage(SendAllPlayersPositionToCurrentSession,
                        NetDeliveryMethod.UnreliableSequenced, SendAllPlayersPositionToCurrentSession.LengthBytes);
                    //Console.WriteLine($"ID:{User.ID} UPDATE_USER_ID:{otherPlayers.ID}");
                }
        }

        public void TestIfPlayerIsRespawned()
        {
            foreach (UserSession player in GameRoom.Room.RoomMembers)
            {
                if (((InMatchState) player.UserGameState).PlayerCollide.IsCollide(this.PlayerCollide))
                {
                    User.Alive = true;
                    PlayerIsReady = true;
                }
            }
        }


        public void SendDeathPacket()
        {
            foreach (UserSession player in GameRoom.Room.RoomMembers)
            {
                NetOutgoingMessage SendAllPlayersPositionToCurrentSession = User.Connection.Peer.CreateMessage();
                SendAllPlayersPositionToCurrentSession.Write(ResurrectPointAddPacket.OpCode);
                SendAllPlayersPositionToCurrentSession.Write(player.ID);
                SendAllPlayersPositionToCurrentSession.Write(player.position.X);
                SendAllPlayersPositionToCurrentSession.Write(player.position.Y);
                User.Connection.SendMessage(SendAllPlayersPositionToCurrentSession,
                    NetDeliveryMethod.UnreliableSequenced, SendAllPlayersPositionToCurrentSession.LengthBytes);
                //Console.WriteLine($"ID:{User.ID} UPDATE_USER_ID:{otherPlayers.ID}");
            }
        }

        public void Update()
        {
            PlayerCollide.UpdatePosition(User.position);
            if (DestinationVector != Vector2.Zero)
            {
                var angle = (int)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(Forward, (Vector2)DestinationVector))));


                if (angle > 12.5f)
                {
                    DestinationVector = (ClickPosition - User.position);
                    DestinationVector = Vector2.Normalize(DestinationVector);
                    Forward = Engine.GameUtility.AdditionalMath.MyMath.Rotate(Forward, SideMultiplier * 5.0f);
                    prev = angle;
                }
                else
                {
                    Forward = Engine.GameUtility.AdditionalMath.MyMath.Rotate(Forward, SideMultiplier * angle);
                    angle = (int)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(Forward, (Vector2)DestinationVector))));
                    DestinationVector = Vector2.Zero;
                }
            }


            if (User.Alive == true && PlayerIsReady == true)
            {
                CheckIfPlayerIsDead();
                User.position += this.Forward * 1.6f * 1;
            }
            else
            {
                TestIfPlayerIsRespawned();
            }
        }
    }
}
