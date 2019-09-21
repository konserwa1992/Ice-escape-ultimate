using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using multi.GameUtility;
using multi.GameUtility.Camera;
using multi.GameUtility.Map;
using System.Linq;
using multi.GameUtility.Map.Elements;
using System.IO;
using Newtonsoft.Json;
using multi.GameUtility.Map.Elements.Enemies.Guns;
using Myra.Graphics2D.UI;
using Myra;
using multi.GameUtility.Menu.Editor;
using tainicom.Aether.Animation;
using multi.GameUtility.Physic;
using multi.GameUtility.Controll.KeyStroke;
using System.Collections.Generic;
using System.Runtime.Remoting;
using multi.GameUtility.Map.Elements.FloorType;

namespace multi
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Editor : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tex;
        Texture2D planeParms;
        Texture2D specialPoints;
        Texture2D pointer;
        private SpriteFont font;
        Model model;
        Animations _animations;
        Effect gameEffect;
        ObjectInspector objectInspector;
        List<IKeyStroke> keys = new List<IKeyStroke>();
        Line LineTEST= new Line(new Vector2(0,0), new Vector2(64,64)) ;

        private Player player;
        VertexPositionColor[] vertices;
        public Map Map { get; private set; }
        private Vector3 positionOnPlane;

        bool OldPushed = false;


        private int multiplayer = 1;
        bool TEST = false;
        private bool collision = false;
        private string StandFloorDebug = "";
        public Editor()
        {
            graphics = new GraphicsDeviceManager(this);


#if ANDROID
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#else
            graphics.PreferredBackBufferWidth = 1920; // remove later
            graphics.PreferredBackBufferHeight = 1080; // remove later
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        //    graphics.ToggleFullScreen();
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
#endif
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

            objectInspector = new ObjectInspector(this);

            gameEffect = Content.Load<Effect>("Effect\\effect");
            tex = Content.Load<Texture2D>("character");
            font = Content.Load<SpriteFont>("File");


            Map = new Map(Content, GraphicsDevice);


            SetUpVertices();
            player = new Player(Vector2.Zero);
            multi.GameUtility.Physic.Point circle = new multi.GameUtility.Physic.Point(new Vector2(positionOnPlane.X, positionOnPlane.Z));
            player.CollisionObject = circle;
            player.CollisionObject.OnCollision += new CollideDetected(delegate(ICollider item)
            {
                if (item.GetType() == typeof(Polygon))
                {
                    MessageBox.Show("Collide", "Circle<>Linse", new string[] { "walsiw" });
                }
            });
            player.CollisionObject.OnCollision += new CollideDetected(EventMethod);
            Director.InstanceDirector.Camera.SetDevice(this.GraphicsDevice);


            model = Content.Load<Model>("robot");



            BiStableKey tempKey = new BiStableKey(Keys.Space);
            tempKey.action += new ClickTrigger(delegate
            {
                IMapElement el = Map.GetMapElementByName<IMapElement>(objectInspector.selectedID);
                if (el != null)
                    Map.GetMapElementByName<IMapElement>(objectInspector.selectedID).Position = new Vector2(positionOnPlane.X, positionOnPlane.Z);
                else
                {
                    IFloor floor = Map.GetMapElementByName<IFloor>(objectInspector.selectedID);
                    floor.FloorPolygon
                        .AddPoint(new VertexPositionColor(positionOnPlane + new Vector3(0, 1, 0), Color.BlueViolet));
                }
            });

            keys.Add(tempKey);


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

            Ray _castRay = ((BasicCamera)Director.InstanceDirector.Camera).CalculateCursorRay(mouse.X, mouse.Y);
            positionOnPlane = PlaneControll.IntersectPoint(_castRay.Direction, _castRay.Position, Vector3.Up, new Vector3(1, 0, 1));


            foreach (IKeyStroke key in keys)
            {
                key.Update();
            }

          //  TEST = true;
            if (TEST == false)
            {
                if (mouse.LeftButton == ButtonState.Pressed && OldPushed == false && !objectInspector.Desktop.IsMouseOverGUI)
                {
                    Map.MapLayers[1].LayerClick((((int)positionOnPlane.X / 32)) * 32, (((int)positionOnPlane.Z / 32)) * 32);
                    OldPushed = true;
                }

                if (mouse.RightButton == ButtonState.Pressed && !objectInspector.Desktop.IsMouseOverGUI)
                {
                   /* IMapElement el = Map.GetMapElementByName<IMapElement>(objectInspector.selectedID);
                    if (el != null && el.GetType() != typeof(SpawnPoint) )
                        (el as Gun).SetLookingDirection(positionOnPlane);*/

                    Circle circle = new Circle(new Vector2(positionOnPlane.X, positionOnPlane.Z), 15);

                    collision = Map.MapPath[0].FloorPolygon.IsCollide(player.CollisionObject);
                }


                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                    StreamReader MapWriter = new StreamReader("C:\\pasta\\Map0.json");
                    object objectMap = JsonConvert.DeserializeObject<Map>(MapWriter.ReadToEnd(),
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                    MapWriter.Close();
                    Map = (Map) objectMap;
                    Map.InitTestGame(player);
                    TEST = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F2))
                {
                   // Map.PutElementOnMap(StandardElements.FinishPoint, new Vector2(positionOnPlane.X, positionOnPlane.Z));

                    if(!File.Exists("C:\\pasta\\Map0.json"))
                    {
                        StreamWriter MapWriter = new StreamWriter("C:\\pasta\\Map0.json",false);
                        string jsonSerialize = JsonConvert.SerializeObject(
                        Map,
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                        MapWriter.Write(jsonSerialize);
                        MapWriter.Flush();
                        MapWriter.Close();
                    }
                }

                if (mouse.LeftButton == ButtonState.Released && OldPushed == true)
                {
                    OldPushed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F12))
                {
                    TEST = true;
                    Map.SaveMe();
                    Map.InitTestGame(player);
                }
            }
            else
            {

                if (mouse.RightButton == ButtonState.Pressed && player.AliveBoiiii == false)
                {
                    player.AliveBoiiii = true;
                }

                StandFloorDebug = Map.UpdatePlayerMovmentType(player);
                collision = Map.MapPath[0].FloorPolygon.IsCollide(player.CollisionObject);
                player.Update(gameTime, positionOnPlane);
            }


            Director.InstanceDirector.Camera.Update(gameTime);
            Map.Update(gameTime);
            base.Update(gameTime);
        }

        public void EventMethod(ICollider collider)
        {
            if (collider.GetType() == typeof(Polygon))
            {
                MessageBox.Show("Collide", "Circle<>Linse", new string[]{"walsiw"});
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


            if (player.AliveBoiiii == false)
            {


                //player.Position = _map._mapElementCollection.Find(x=>x.GetType() == typeof(SpawnPoint)).Position;
            }
                  spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default);
                   spriteBatch.DrawString(font, $"Collider {collision} Stand on {StandFloorDebug}", Vector2.Zero, Color.Black);
                   spriteBatch.End();




            if (TEST == true)
            {
                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                rs.FillMode = FillMode.WireFrame;
            
                GraphicsDevice.RasterizerState = rs;

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.CreateScale(2) * Matrix.CreateTranslation(new Vector3(player.Position.X, 0, player.Position.Y));
                        effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                        effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;
                        effect.VertexColorEnabled = true;
                    }

                    mesh.Draw();
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
            objectInspector.Draw();
            LineTEST.Draw(GraphicsDevice, gameEffect);
            base.Draw(gameTime); 
        }
    }
}
