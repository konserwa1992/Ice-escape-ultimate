using Engine.GameUtility;
using Engine.GameUtility.Camera;
using Engine.GameUtility.Controll.KeyStroke;
using Engine.GameUtility.Map;
using Engine.GameUtility.Physic;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using multi.Network;
using NETGame;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsGame
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private SpriteFont font;
        private Model model;
        private Effect gameEffect;
        private List<IKeyStroke> keys = new List<IKeyStroke>();
        private Line LineTEST = new Line(new Vector2(0, 0), new Vector2(64, 64));

        private Player player;
        private string PlayerName;
        private VertexPositionColor[] vertices;
        public Map Map { get; private set; }
        private Vector3 positionOnPlane;
        private bool OldPushed = false;
        private float movePacketInterval = 50;

        private int multiplayer = 1;
        private bool TEST = false;
        private bool collision = false;
        private string StandFloorDebug = "";

        /// TEST GRY PO SIECI
        /// </summary>
        private NetClient Client;
        private List<PlayerClass> OtherPlayerList = new List<PlayerClass>();
        private double akumulator = 0.0f;



        public Game1(string name)
        {
            graphics = new GraphicsDeviceManager(this);
            PlayerName = name;


#if ANDROID
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#else
            //   graphics.PreferredBackBufferWidth = 1920; // remove later
            //  graphics.PreferredBackBufferHeight = 1080; // remove later

            graphics.PreferredBackBufferWidth = 800; // remove later
            graphics.PreferredBackBufferHeight = 600; // remove later

            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            //    graphics.ToggleFullScreen();
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
#endif



            ////SIEC//////////////////////////////
            var config = new NetPeerConfiguration("application name");
            Client = new NetClient(config);
            Client.Start();
            Client.Connect(host: "192.168.100.10", port: 12345);

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

            gameEffect = Content.Load<Effect>("Effect\\effect");
            tex = Content.Load<Texture2D>("character");
            font = Content.Load<SpriteFont>("File");

            Map = new Map(Content, GraphicsDevice);


            SetUpVertices();
            player = new Player(Vector2.Zero);
            Engine.GameUtility.Physic.Point circle = new Engine.GameUtility.Physic.Point(new Vector2(positionOnPlane.X, positionOnPlane.Z));
            player.CollisionObject = circle;
            player.CollisionObject.OnCollision += new CollideDetected(delegate (ICollider item)
            {
                if (item.GetType() == typeof(Polygon))
                {
                    MessageBox.Show("Collide", "Circle<>Linse", new string[] { "walsiw" });
                }
            });

            player.CollisionObject.OnCollision += new CollideDetected(EventMethod);
            Director.InstanceDirector.Camera.SetDevice(GraphicsDevice);

            model = Content.Load<Model>("robot");

            StreamReader MapWriter = new StreamReader("pasta\\Map0.json");
            Map objectMap = JsonConvert.DeserializeObject<Map>(MapWriter.ReadToEnd(),
                new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            MapWriter.Close();
            Map = objectMap;
            Map.InitTestGame(player);


            BiStableKey key = new BiStableKey(Keys.Up);
            key.action += new ClickTrigger(delegate
            {
                foreach (PlayerClass otherPl in OtherPlayerList)
                {
                    otherPl.interstepAdd += 0.01f;
                }
            });
            keys.Add(key);

            BiStableKey key2 = new BiStableKey(Keys.Down);
            key2.action += new ClickTrigger(delegate
            {
                foreach (PlayerClass otherPl in OtherPlayerList)
                {
                    otherPl.interstepAdd -= 0.01f;
                }
            });
            keys.Add(key2);


            BiStableKey key3 = new BiStableKey(Keys.Right);
            key3.action += new ClickTrigger(delegate
            {
                movePacketInterval += 10;
            });
            keys.Add(key3);

            BiStableKey key4 = new BiStableKey(Keys.Left);
            key4.action += new ClickTrigger(delegate
            {
                foreach (PlayerClass otherPl in OtherPlayerList)
                {
                    movePacketInterval -= 10;
                }
            });
            keys.Add(key4);
            // MapWriter.Write(jsonSerialize);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState();


            /////////TEST
            akumulator += gameTime.ElapsedGameTime.TotalMilliseconds;


            foreach (PlayerClass otherPl in OtherPlayerList)
            {
                otherPl.Interpolate();
            }



         /*   if (akumulator > movePacketInterval && player.PlayerNetInfo != null && player.Forward != player.OldForward)
            {
                var newMSG = Client.CreateMessage();
                newMSG.Write((short)6066);
                newMSG.Write(player.Forward.X);
                newMSG.Write(player.Forward.Y);
                Client.SendMessage(newMSG, NetDeliveryMethod.UnreliableSequenced);
                akumulator = 0;
            }*/


            /*
             if (akumulator > movePacketInterval && player.PlayerNetInfo!=null)
             {
                 var newMSG = Client.CreateMessage();
                 newMSG.Write((short)6066);
                 newMSG.Write(player.Position.X);
                 newMSG.Write(player.Position.Y);
                 Client.SendMessage(newMSG, NetDeliveryMethod.UnreliableSequenced);
                 akumulator = 0;
             }*/
            /////////TEST

            NetIncomingMessage message;
            while ((message = Client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        {
                            // handle custom messages
                            if (message.LengthBytes >= 2)
                            {
                                short opcode;
                                opcode = message.ReadInt16();
                                if (opcode == 2000)
                                {
                                    PlayerClass SpawnPlayerPacket = new PlayerClass();
                                    SpawnPlayerPacket.ID = message.PeekInt32();
                                    //message.ReadAllProperties((object) player);
                                    player.PlayerNetInfo = SpawnPlayerPacket;

                                    var newMSG = Client.CreateMessage();

                                    JoinRoomPacket joinRoom = new JoinRoomPacket("TEST");
                                    newMSG.Write(JoinRoomPacket.OpCode);
                                    newMSG.Write(joinRoom.RoomName);
                                    Client.SendMessage(newMSG, NetDeliveryMethod.ReliableSequenced);
                                    akumulator = 0;
                                }
                                else if (opcode == 2620)
                                {
                                    PlayerClass playerTemp = new PlayerClass();
                                    message.ReadAllProperties(playerTemp);
                                    OtherPlayerList.Add(playerTemp);
                                }
                                else if (opcode == 6066)
                                {
                                    MovePacket movePacket = new MovePacket();
                                    message.ReadAllProperties(movePacket);
                                    PlayerClass player2Move = OtherPlayerList.FirstOrDefault(x => x.ID == movePacket.ID);
                                    if (player2Move != null)
                                    {
                                        player2Move.StartPosition = player2Move.EndPosition;
                                        player2Move.EndPosition = new Vector2(movePacket.X, movePacket.Y);
                                        player2Move.interStep = 0;
                                    }
                                }

                            }
                            break;
                        }

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages
                        switch (message.SenderConnection.Status)
                        {
                            case NetConnectionStatus.Connected:
                                {
                                    var newMSG = Client.CreateMessage();
                                    newMSG.Write((short)2000);
                                    newMSG.Write("Client");
                                    Client.SendMessage(newMSG, NetDeliveryMethod.UnreliableSequenced);
                                    break;
                                }
                        }
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        // handle debug messages
                        // (only received when compiled in DEBUG mode)

                        break;

                    /* .. */
                    default:
                        break;
                }
            }


            foreach (IKeyStroke key in keys)
            {
                key.Update();
            }



            if (IsActive)
            {
                Ray _castRay = ((BasicCamera)Director.InstanceDirector.Camera).CalculateCursorRay(mouse.X, mouse.Y);
                positionOnPlane = PlaneControll.IntersectPoint(_castRay.Direction, _castRay.Position, Vector3.Up, new Vector3(1, 0, 1));
                StandFloorDebug = Map.UpdatePlayerMovmentType(player);
                collision = Map.MapPath[0].FloorPolygon.IsCollide(player.CollisionObject);
            }
            //  player.Update(gameTime, positionOnPlane);

            if (mouse.RightButton == ButtonState.Pressed && player.AliveBoiiii == false && IsActive == true)
            {
                var newMSG = Client.CreateMessage();
                newMSG.Write((short)6066);
                newMSG.Write(positionOnPlane.X);
                newMSG.Write(positionOnPlane.Z);
                Client.SendMessage(newMSG, NetDeliveryMethod.UnreliableSequenced);
            }



            Director.InstanceDirector.Camera.Update(gameTime);
            Map.Update(gameTime);


            player.OldForward = player.Forward;
            base.Update(gameTime);
        }

        public void EventMethod(ICollider collider)
        {
            if (collider.GetType() == typeof(Polygon))
            {
                MessageBox.Show("Collide", "Circle<>Linse", new string[] { "walsiw" });
            }
        }

        private void SetUpVertices()
        {
            vertices = new VertexPositionColor[3];

            vertices[0].Position = new Vector3(0f, 0f, 0f);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(10f, 10f, 0f);
            vertices[1].Color = Color.Yellow;
            vertices[2].Position = new Vector3(10f, 0f, -5f);
            vertices[2].Color = Color.Green;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Stencil | ClearOptions.Target, Color.CornflowerBlue, 1f, 0);


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default);
            spriteBatch.DrawString(font, $"Collider {collision} Stand on {StandFloorDebug}  PlayerID {(player.PlayerNetInfo != null ? player.PlayerNetInfo.ID : 0)} isActive:{IsActive}", Vector2.Zero, Color.Black);
            spriteBatch.End();



            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            rs.FillMode = FillMode.WireFrame;

            GraphicsDevice.RasterizerState = rs;

           /* foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(2) * Matrix.CreateTranslation(new Vector3(player.Position.X, 0, player.Position.Y));
                    effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                    effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                    effect.EnableDefaultLighting();
                    effect.TextureEnabled = true;
                    effect.LightingEnabled = true;
                    effect.VertexColorEnabled = true;
                }

                mesh.Draw();
            }*/



            if (player.PlayerNetInfo != null)
            {
                int x = 1;
                foreach (PlayerClass otherPl in OtherPlayerList)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default);
                    spriteBatch.DrawString(font, $"Player position {otherPl.ID}", new Vector2(0, 20 + x), Color.Black);
                    spriteBatch.DrawString(font, $"X:  {otherPl.CurrPosition.X.ToString()}", new Vector2(0, 40 + x), Color.Black);
                    spriteBatch.DrawString(font, $"Z:  {otherPl.CurrPosition.Y.ToString()}", new Vector2(0, 60 + x), Color.Black);
                    spriteBatch.DrawString(font, $"interstepAdd:  {otherPl.interstepAdd}", new Vector2(0, 80 + x), Color.Black);
                    spriteBatch.DrawString(font, $"SendMovePacketInterval:  {movePacketInterval}", new Vector2(0, 90 + x), Color.Black);
                    spriteBatch.End();
                    x += 90;
                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = Matrix.CreateScale(2) * Matrix.CreateTranslation(new Vector3(otherPl.CurrPosition.X, 0, otherPl.CurrPosition.Y));
                            effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                            effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                            effect.EnableDefaultLighting();
                            effect.LightingEnabled = true;
                            effect.TextureEnabled = false;
                            effect.VertexColorEnabled = true;
                        }
                        mesh.Draw();
                    }
                }
            }


            graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            gameEffect.CurrentTechnique = gameEffect.Techniques["Simplest"];
            gameEffect.Parameters["xView"].SetValue(Director.InstanceDirector.Camera.ViewMatrix);
            gameEffect.Parameters["xProjection"].SetValue(Director.InstanceDirector.Camera.ProjectionMatrix);
            gameEffect.Parameters["xWorld"].SetValue(Matrix.Identity);


            gameEffect.Parameters["xLightPos"].SetValue(Director.InstanceDirector.Camera.Position);
            gameEffect.Parameters["xLightPower"].SetValue(0.1f);
            gameEffect.Parameters["xAmbient"].SetValue(1f);



            Map.Draw(GraphicsDevice, gameEffect);
            LineTEST.Draw(GraphicsDevice, gameEffect);
            base.Draw(gameTime);
        }
    }
}
