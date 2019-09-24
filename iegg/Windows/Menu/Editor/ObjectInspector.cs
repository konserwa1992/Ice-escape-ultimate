using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameUtility.Map.Elements;
using Engine.GameUtility.Menu.Editor.Windows;
using Engine.GameUtility.Menu.Editor.Windows.Special;
using Engine.GameUtility.Map.Elements.Enemies.Guns;
using Engine.GameUtility.Map.Elements.FloorType;

namespace EditorEngine.GameUtility.Menu.Editor
{
    /// <summary>
    /// Lib is fine, just this code is trash.
    /// when you grow up your heart die
    /// </summary>
    public class ObjectInspector
    {
        public Desktop Desktop;
        public string selectedID { get; private set; }
        private Window inspectorWindow;

        public ObjectInspector(object editor)
        {
            MyraEnvironment.Game = (Game)editor;
            MyraEnvironment.DrawWidgetsFrames = true;
            Desktop = new Desktop();
           

            inspectorWindow = new Window()
            {
                Id="wObjectInspector",
                Width = 200,
               Height = 600
            };

            Panel grid = new Panel()
            {
                Id= "GridAddElement",
                Width = 200,
                Height = 600
            };


            Button bAddNewEnemy = new Button()
            {
                Id = "bNewEnemy",
                Width = 200, Top = 30,Text= "AddEnemy"
            };

            ListBox lObjectList = new ListBox()
            {
                Id = "lObjectList",
                Width = 200,
                Top = 60
            };

            lObjectList.SelectedIndexChanged += new EventHandler(delegate (object sender, EventArgs args)
            {
                selectedID = lObjectList.SelectedItem.Id;
                IMapElement MapElement = ((EditorEngine.Editor)MyraEnvironment.Game).Map.GetMapElementByName<IMapElement>(selectedID);
                if (MapElement != null)
                {
                    if (typeof(Gun) == MapElement.GetType())
                        Desktop.Widgets.Add(new GunInspector(MapElement));
                }
                else
                {
                    IFloor NewFloorElement =
                        ((EditorEngine.Editor) MyraEnvironment.Game).Map.GetMapElementByName<IFloor>(selectedID);

                }
            });


            bAddNewEnemy.Click += new EventHandler(delegate(object sender,EventArgs args)
            { 
                Desktop.Widgets.Add(new AddNewMapElementWindow(this));
            });



            grid.Widgets.Add(lObjectList);
            grid.Widgets.Add(bAddNewEnemy);
            grid.Widgets.Add(lObjectList);
            inspectorWindow.Content = grid;
            Desktop.Widgets.Add(inspectorWindow);
        }



        public void Draw()
        {
            List<IMapElement> mapElementRefference = ((EditorEngine.Editor)MyraEnvironment.Game).Map.MapElements;
            List<IFloor> mapFloorTypeRefference = ((EditorEngine.Editor)MyraEnvironment.Game).Map.MapPath;
            Desktop.Bounds = new Rectangle(0, 0, MyraEnvironment.Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
             MyraEnvironment.Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

     //       ((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.Clear();
            if (mapElementRefference != null)
            {
            //    ((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.Clear();
                foreach (IMapElement element in mapElementRefference)
                {
                    if (((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.FirstOrDefault(x => x.Id == element.Key) == null)
                    {
                        ((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.Add(new ListItem()
                        {
                            Id = element.Key,
                            Text = element.Key
                        });
                    }
                }
            }
            if (mapFloorTypeRefference != null)
            {
                //    ((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.Clear();
                foreach (IFloor element in mapFloorTypeRefference)
                {
                    if (((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.FirstOrDefault(x => x.Id == element.Key) == null)
                    {
                        ((ListBox)inspectorWindow.Content.FindWidgetById("lObjectList")).Items.Add(new ListItem()
                        {
                            Id = element.Key,
                            Text = element.Key
                        });
                    }
                }
            }


            Desktop.Render();
        }
    }
}
