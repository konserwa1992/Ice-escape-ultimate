using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using multi.GameUtility.Camera;

namespace multi.GameUtility.Map.Elements.Enemies.Guns
{
    class Bullet : IMapElement
    {
        public Vector2 Position { get; set; }
        public string Key { get; set; }
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public float Scale { get; set; } = 1.0f;
        public float Speed { get; set; } = 20f;
        public Vector3 DirectionOfMove { get; set; }
        public bool Hidden { get; set; } = true;


        /*
         * HARDCODE IS HERE
         */
        public Bullet(Vector2 pos,  Vector3 shootDirection)
        {
            Position = pos;
            this.DirectionOfMove =(shootDirection);
            this.DirectionOfMove.Normalize();


          /*  var vector2Player = shootDirection - new Vector3(Position.X, 0, Position.Y);
            vector2Player.Normalize();

            Rotation = new Vector3(0, 0, (float)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(new Vector2(1,0), new Vector2(vector2Player.X, vector2Player.Z))))));
            if (vector2Player.Z > 0)
            {
                Rotation = Rotation * -1;
            }
            else
            {
                Rotation = Rotation * 1;
            }*/
        }


        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            foreach (ModelMesh mesh in ContentContainer.GetObjectFromContainer<Model>("Bullet").Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(0.5f)* Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Rotation.X), MathHelper.ToRadians(Rotation.Y),MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position.X, 0, Position.Y);
                    effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                    effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                    effect.LightingEnabled = true;
                }

                mesh.Draw();
            }
        }

        public void Load()
        {
        }

        public void Update(GameTime gameTime)
        {
            this.Position = this.Position + ((new Vector2(DirectionOfMove.X, DirectionOfMove.Z) * Speed)* (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
