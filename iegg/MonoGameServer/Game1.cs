using Engine.GameUtility;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Server;
using Server.States;
using System;

namespace MonoGameServer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float timer = 0;
        NetIncomingMessage msg;
        private NetPeerConfiguration config;
        private NetServer server;
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            config = new NetPeerConfiguration("application name")
                { Port = 12345 };
            server = new NetServer(config);
            server.Start();


            GameRoom newGameRoom = new GameRoom(null, "TEST", 8);
            NetworkSessionContainer.NetworkSessions.GameRooms.Add(newGameRoom);

            GameRoom newGameRoom2 = new GameRoom(null, "TEST2", 8);
            NetworkSessionContainer.NetworkSessions.GameRooms.Add(newGameRoom2);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentContainer.LoadContent(Content);

            font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        public void FixedLoop()
        {
            foreach (GameRoom gameRoom in NetworkSessionContainer.NetworkSessions.GameRooms)
            {
                gameRoom.Update();
            }

            if ((msg = server.ReadMessage()) == null) return;


            switch (msg.MessageType)
            {
                case NetIncomingMessageType.ConnectionApproval:
                    {
                        //Console.WriteLine("Connected");
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
                        if (opcode == 2000) //Logowanie użytkownika
                        {
                            unsafe
                            {
                                if (!NetworkSessionContainer.NetworkSessions.UserSessions.Exists(x => x.Connection == msg.SenderConnection))
                                {
                                    UserSession session = new UserSession();
                                    TypedReference tr = __makeref(session);
                                    IntPtr ptr = **(IntPtr**)(&tr);
                                    Console.WriteLine(ptr);
                                    session.Connection = msg.SenderConnection;
                                    // session.ID = ptr.ToInt32();
                                    Random randID = new Random();
                                    session.ID = randID.Next(1000000); //martwić sie tym będe później
                                    session.Name = msg.ReadString();

                                    NetOutgoingMessage outMessage = session.Connection.Peer.CreateMessage();
                                    outMessage.Write((short)2000);
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
                            foreach (UserSession user in NetworkSessionContainer.NetworkSessions.UserSessions)
                            {
                                if (user.Connection == msg.SenderConnection)
                                {
                                    user.UserGameState.Recive(msg);
                                }
                            }
                        }

                        break;
                    }
                case NetIncomingMessageType.ErrorMessage:
                    //Console.WriteLine(msg.ReadString());
                    break;
                default:
                    {
                        //Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                    }
            }
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            FixedLoop();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            int Y = 0;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default);
            foreach (GameRoom room in NetworkSessionContainer.NetworkSessions.GameRooms)
            {
                spriteBatch.DrawString(font, $"Room name {room.Room.Name}", new Vector2(0,Y), Color.Black);
                Y += 12;
                foreach (UserSession session in room.Room.RoomMember)
                {
                    spriteBatch.DrawString(font, $"Nazwa Gracza {session.Name} Pozycja {session.position}", new Vector2(0, Y), Color.Black);
                    Y += 12;
                }
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
