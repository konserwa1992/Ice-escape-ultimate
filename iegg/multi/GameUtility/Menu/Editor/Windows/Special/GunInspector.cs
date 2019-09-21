using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using multi.GameUtility.Camera;
using multi.GameUtility.Map;
using multi.GameUtility.Map.Elements;
using multi.GameUtility.Map.Elements.Enemies.Guns;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Menu.Editor.Windows.Special
{
    class GunInspector : Window
    {
        IMapElement CurrentMapElement;

        public GunInspector(IMapElement name)
        {
            CurrentMapElement = name;
            Panel panel = new Panel()
            {
                Width = 120,
                Height = 125
            };


            TextBlock lShootInterval = new TextBlock()
            {
                Text = "ShootInterval",
                Width = 110
            };

            TextField tShootInterval = new TextField()
            {
                Text = "1000",
                Top = 25,
                Width = 110
            };

            TextBlock lFirsShootDelay = new TextBlock()
            {
                Text = "ShootInterval",
                Top = 50,
                Width = 110
            };

            TextField tFirsShootDelay = new TextField()
            {
                Text = "1000",
                Top = 75,
                Width = 110
            };


            Button bSave = new Button()
            {
                Text = "Save",
                Left=45,
                Top = 100
            };

            bSave.Click += new EventHandler(delegate (object sender, EventArgs arg)
             {
                 float interval = 0;
                 float delay = 0;
                 if (float.TryParse(tShootInterval.Text,  out interval)==true && float.TryParse(tFirsShootDelay.Text, out delay) ==true)
                 {
                     Gun selectedObjectinstance = (Gun)CurrentMapElement;
                     selectedObjectinstance.Interval = interval;
                     selectedObjectinstance.Delay = delay;

                     var messageBox = Dialog.CreateMessageBox("Success", "Change have been saved.");
                     messageBox.ShowModal(this.Desktop);

                     this.Close();
                 }
                 else
                 {
                     var messageBox = Dialog.CreateMessageBox("Error", "Invalid data type");
                     messageBox.ShowModal(this.Desktop);
                 }
             });

            panel.Widgets.Add(lShootInterval);
            panel.Widgets.Add(tShootInterval);
            panel.Widgets.Add(lFirsShootDelay);
            panel.Widgets.Add(tFirsShootDelay);
            panel.Widgets.Add(bSave);
            this.Content = panel;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                // Map.PutElementOnMap(StandardElements.SpawnPiont, );
                MouseState mouse = Mouse.GetState();
                if (CurrentMapElement != null)
                {
                    Ray _castRay = ((BasicCamera)Director.InstanceDirector.Camera).CalculateCursorRay(mouse.X, mouse.Y);
                    Vector3 positionOnPlane = PlaneControll.IntersectPoint(_castRay.Direction, _castRay.Position, Vector3.Up, new Vector3(1, 0, 1));

                    CurrentMapElement.Position = new Vector2(positionOnPlane.X, positionOnPlane.Z);

                }

            }
        }
    }
}
