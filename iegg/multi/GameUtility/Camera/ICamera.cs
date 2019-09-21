using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Camera
{
    interface ICamera
    {
        Vector3 Position { get; set; }
        Vector3 CameraTarget { get; set; }
        Matrix ViewMatrix { get; set; }
        Matrix ProjectionMatrix { get; set; }
        void SetDevice(GraphicsDevice device);

        void SetupCamera();
        void Update(GameTime _gametime);
    }
}
