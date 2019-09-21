using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Camera
{
    class Director
    {
        private static Director _instance = new Director() ;
        public ICamera Camera { get; private set; } = new BasicCamera() ;

        private Director()
        {

        }

        public static Director InstanceDirector
        {
            get
            {
                return _instance;
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
        }

        public void SetCamera(ICamera camera)
        {
            this.Camera = camera;
        }
    }
}
