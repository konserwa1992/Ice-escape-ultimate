using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.GameUtility.Camera
{
    public class BasicCamera: BaseCamera
    {
        public Ray CalculateCursorRay(float X, float Y)
        {
            Vector3 nearSource = new Vector3(X, Y, 0f);
            Vector3 farSource = new Vector3(X, Y, 1f);

            Vector3 nearPoint = base.device.Viewport.Unproject(nearSource, base.ProjectionMatrix, base.ViewMatrix, Matrix.Identity);
            Vector3 farPoint = base.device.Viewport.Unproject(farSource, base.ProjectionMatrix, base.ViewMatrix, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }



        public override void Update(GameTime _gametime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if(mouse.XButton1 == ButtonState.Pressed)
            {
                Vector3 moveVector = (this.CameraTarget - this.Position);
                moveVector.Normalize();
                this.Position += moveVector;
            }

            if (mouse.XButton2 == ButtonState.Pressed)
            {
                Vector3 moveVector = (this.CameraTarget - this.Position);
                moveVector.Normalize();
                this.Position -= moveVector;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                Position += new Vector3(1, 0, 0);
                CameraTarget += new Vector3(1, 0, 0);
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                Position -= new Vector3(1, 0, 0);
                CameraTarget -= new Vector3(1, 0, 0);
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                Position += new Vector3(0, 0, -1);
                CameraTarget += new Vector3(0, 0, -1);
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                Position += new Vector3(0, 0, 1);
                CameraTarget += new Vector3(0, 0, 1);
            }

            SetupCamera();
        }
    }
}
