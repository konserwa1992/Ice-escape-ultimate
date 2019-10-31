using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameUtility.Map.Elements;
using Engine.GameUtility.Map.Elements.Enemies.Guns;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Engine.GameUtility.Control;
using Engine.GameUtility.Physic;
using Engine.GameUtility.Map.Elements.FloorType;

namespace Engine.GameUtility.Map
{
    public enum StandardElements
    {
        SpawnPiont,FinishPoint,Gun,Ignore,NormalIce,ReverseIce
    }

    public class Map
    {
        public List<Layer> MapLayers = new List<Layer>();

        public List<IMapElement> MapElements { get; private set; } = new List<IMapElement>();
        public List<IFloor> MapPath { get; private set; } = new List<IFloor>();
        /* public SpawnPoint StartPoint;
         public FinishPoint FinishPoint;*/

        private int terrainWidth = 10;
        private int terrainHeight = 10;


        public void SaveMe()
        {
            StreamWriter writer = new StreamWriter("Test.json");
            writer.Write(JsonConvert.SerializeObject(this));
            writer.Close();
        }

        public Map(ContentManager content,GraphicsDevice device)
        {           
            MapLayers.Add(new Layer(terrainWidth, terrainHeight, 0.0f,64,"DefaultGround",content));
            MapLayers.Add(new Layer(terrainWidth, terrainHeight, 0.01f, 64, "RegularIceSheet", content));

            //StartPoint = new SpawnPoint(new Vector2(-100, -100));
           // FinishPoint = new FinishPoint(new Vector2(-100, -100));
        }

        public string UpdatePlayerMovmentType(Player player)
        {
            ICollider playerCollider = new Physic.Point(player.Position);

            IFloor standFloor=null;
            foreach (var collisionPath in MapPath)
            {
                //Tak jak się umówiłem sam ze sobą Pierw powinno być bezwładność,odwrócone ślizganie,ślizganie,chodzenie normalne
                if (collisionPath.FloorPolygon.IsCollide(playerCollider))
                {
                    
                    if (typeof(NormalControll) == collisionPath.GetType() || (standFloor== null))
                    {
                        standFloor = collisionPath;
                    }
                }
            }

            if (standFloor == null)
            {
                return "jebanycheater";
            }
            else
            {
                player.SetControllType(standFloor.ControllType);
                return standFloor.GetType().ToString();
;           }
        }

        public void InitTestGame(Player player)
        {
            SpawnPoint StartPoint = (SpawnPoint)MapElements.Where(x => typeof(SpawnPoint) == x.GetType()).FirstOrDefault();
            if (StartPoint != null)
            {
                if (StartPoint.Position.X > 0 && StartPoint.Position.Y > 0)
                {
                    player.Position = StartPoint.Position;
                }
                else
                {
                  //  GraphicsDebugMessage test = new GraphicsDebugMessage();
                  //  test.Message = "huj";
                }
            }
        }


        /// <summary>
        /// Jeżeli element jest dodany to zwracamy jego klucz
        /// </summary>
        /// <param name="el"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string AddElementOnMap(StandardElements el, string name)
        {
            if (!MapElements.Exists(x => x.Key == name))
            {
                if (el == StandardElements.Gun)
                {
                    MapElements.Add(new Gun(name));
                    return name;
                }else if(el == StandardElements.SpawnPiont)
                {
                    if (MapElements.FirstOrDefault(x => x.GetType() == typeof(SpawnPoint)) == null)
                    {
                        MapElements.Add(new SpawnPoint(name));
                        return name;
                    }
                    else
                    {
                        return "There is already spawn point";
                    }
                }
            }

            if (el == StandardElements.ReverseIce)
            {
                if (MapPath.FirstOrDefault(x => x.Key == name) == null)
                {
                    MapPath.Add(new RegularIce(name));
                    return name;
                }
                else
                {
                    return "There is already spawn point";
                }
            }
            else if (el == StandardElements.NormalIce)
            {
                if (MapPath.FirstOrDefault(x => x.Key== name) == null)
                {
                    MapPath.Add(new RegularIce(name));
                    return name;
                }
                else
                {
                    return "There is already spawn point";
                }
            }

            return "Coś poszło nie tak";
        }
       

        public T GetMapElementByName<T>(string name)
        {
            if (typeof(T) == typeof(IMapElement))
            {
                return (T)MapElements.FirstOrDefault(x => x.Key == name);
            }else 
            {
                return (T)MapPath.FirstOrDefault(x => x.Key == name);
            }

        }

        public void Update(GameTime gameTime)
        {
            foreach (var element in MapElements)
            {
                element.Update(gameTime);
            }
        }
        
        public void DrawMapElements(GraphicsDevice graphicsDevice, Effect ef)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            rs.FillMode = FillMode.Solid;
            graphicsDevice.RasterizerState = rs;
            foreach (var element in MapElements)
            {
                element.Draw(graphicsDevice, ef);
            }

            foreach (var element in MapPath)
            {
                element.Draw(graphicsDevice, ef);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            rs.FillMode = FillMode.Solid;
            graphicsDevice.RasterizerState = rs;

            foreach (var element in MapLayers)
            {
                element.Draw(graphicsDevice, ef);
            }

           
            DrawMapElements(graphicsDevice, ef);
        }
    }
}
