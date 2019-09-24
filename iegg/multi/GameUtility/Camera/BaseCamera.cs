using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.GameUtility.Camera
{
   public class BaseCamera : ICamera
    {
        public Vector3 Position { get; set; } = new Vector3(0, 200,200);
        public Vector3 CameraTarget { get; set; } = new Vector3(0, 0, 0);
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix { get; set; }
        protected GraphicsDevice device { get; set; }

        public void SetDevice(GraphicsDevice device)
        {
            this.device = device;
        }

        public void SetupCamera()
        {
            ViewMatrix = Matrix.CreateLookAt(Position, CameraTarget, Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 1000.0f);
        }


        public virtual void Update(GameTime _gametime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if(keyboard.IsKeyDown(Keys.A))
            {
                Position += new Vector3(1, 0, 0);
                CameraTarget += new Vector3(1, 0, 0);
            }

            SetupCamera();

        }
    }
}
