using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Engine.GameUtility;
using Engine.GameUtility.Camera;
using Engine.GameUtility.Map;
using System.Linq;
using Engine.GameUtility.Map.Elements;
using System.IO;
using Newtonsoft.Json;
using Engine.GameUtility.Map.Elements.Enemies.Guns;
using Myra.Graphics2D.UI;
using Myra;

namespace Engine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tex;
        private SpriteFont font;
        Model model;
        Model model2;
        Effect gameEffect;

        private Player player;
        VertexPositionColor[] vertices;
        Map _map;
    

        bool OldPushed = false;


        private int multiplayer = 1;
        bool TEST = false;

        public Game1()
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
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24;
            graphics.ToggleFullScreen();
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

            gameEffect = Content.Load<Effect>("Effect\\effect");
            tex = Content.Load<Texture2D>("character");
            font = Content.Load<SpriteFont>("File");


            _map = new Map(Content, GraphicsDevice);

            /* StreamReader reader = new StreamReader("Test.json");
              _map = JsonConvert.DeserializeObject<Map>(reader.ReadToEnd());
              reader.Close();*/

            SetUpVertices();
            player = new Player(Vector2.Zero);
            Director.InstanceDirector.Camera.SetDevice(this.GraphicsDevice);


            model = Content.Load<Model>("robot");
            model2 = Content.Load<Model>("gun");
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
            Vector3 positionOnPlane = PlaneControll.IntersectPoint(_castRay.Direction, _castRay.Position, Vector3.Up, new Vector3(1, 0, 1));


            if (TEST == false)
            {
                if (mouse.LeftButton == ButtonState.Pressed && OldPushed == false )
                {
                    _map.MapLayers[1].LayerClick((((int)positionOnPlane.X / 32)) * 32, (((int)positionOnPlane.Z / 32)) * 32);
                    OldPushed = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F1))
                {
                  // _map.PutElementOnMap(StandardElements.SpawnPiont, new Vector2(positionOnPlane.X, positionOnPlane.Z));
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F2))
                {
                    //_map.PutElementOnMap(StandardElements.FinishPoint, new Vector2(positionOnPlane.X, positionOnPlane.Z));
                }

                if (mouse.LeftButton == ButtonState.Released && OldPushed == true)
                {
                    OldPushed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F12))
                {
                    TEST = true;
                    _map.SaveMe();
                    _map.InitTestGame(player);
                }
            }
            else
            {

                if (mouse.RightButton == ButtonState.Pressed && player.AliveBoiiii == false)
                {
                    player.AliveBoiiii = true;
                    _map.InitTestGame(player);
                }
                
                player.Update(gameTime, positionOnPlane);
            }


            Director.InstanceDirector.Camera.Update(gameTime);

            base.Update(gameTime);
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
           /* spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default);
            spriteBatch.DrawString(font, $"Angle {gun.RotationZ} test ${player._controll.SideMultiplier}", Vector2.Zero, Color.Black);
            spriteBatch.End();*/

            if (TEST == true)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.CreateScale(2.0f) * Matrix.CreateTranslation(new Vector3(player.Position.X, 0, player.Position.Y));
                        effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                        effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                        effect.EnableDefaultLighting();
                        effect.LightingEnabled = true;
                        effect.VertexColorEnabled = true;
                    }

                    mesh.Draw();
                }
                
            }

                RasterizerState rs = new RasterizerState();
                rs.CullMode = CullMode.None;
                rs.FillMode = FillMode.Solid;
                GraphicsDevice.RasterizerState = rs;

                graphics.GraphicsDevice.BlendState = BlendState.Opaque;
                graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                gameEffect.CurrentTechnique = gameEffect.Techniques["Simplest"];
                gameEffect.Parameters["xView"].SetValue(Director.InstanceDirector.Camera.ViewMatrix);
                gameEffect.Parameters["xProjection"].SetValue(Director.InstanceDirector.Camera.ProjectionMatrix);
                gameEffect.Parameters["xWorld"].SetValue(Matrix.Identity);


                gameEffect.Parameters["xLightPos"].SetValue(Director.InstanceDirector.Camera.Position);
                gameEffect.Parameters["xLightPower"].SetValue(0.1f);
                gameEffect.Parameters["xAmbient"].SetValue(0.75f);


                _map.Draw(GraphicsDevice, gameEffect);


                base.Draw(gameTime);
            
        }
    }
}
