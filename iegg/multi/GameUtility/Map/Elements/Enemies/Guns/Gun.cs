using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using multi.GameUtility.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map.Elements.Enemies.Guns
{
    class Gun : IMapElement
    {
        public float RotationSpeed { get; set; }
        public int ShootInterval { get; set; }
        public float BulletSpeed { get; set; }
        public bool TrackTarget { get; set; }
        public float AutoTargetingDistance { get; set; }

        public Vector3 Forward { get; set; } = new Vector3(1, 0, 0);
        public Vector2 Position { get; set; }
        public string Key { get; set; }
        public Vector3 LookAtDirection {get; set;} = new Vector3(1, 0, 0);
        public float Interval { get; set; } = 2.0f;
        public float Delay { get; set; } = 2.0f;
        public Vector3 Rotation { get; set; }
        public float Scale { get; set; } = 1.0f;
        public float CurrInterval { get; set; }
        public bool Hidden { get; set; } = true;

        private List<Bullet> BulletList = new List<Bullet>();

        public Gun(string name)
        {
            Key = name;
            CurrInterval = Interval;
        }

        public void SetLookingDirection(Vector3 Direction)
        {
           // Direction.Normalize();
            this.LookAtDirection = Direction;
        }

        public void Update(GameTime gameTime)
        {
            var vector2Player = LookAtDirection - new Vector3(Position.X, 0, Position.Y);
            vector2Player.Normalize();

            Rotation = new Vector3(0,0, (float)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(new Vector2(Forward.X, Forward.Z), new Vector2(vector2Player.X, vector2Player.Z))))));
            if(vector2Player.Z>0)
            {
                Rotation = Rotation * -1;
            }else
            {
                Rotation = Rotation * 1;
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Delay <= 0)
            {
                CurrInterval -= elapsed;
                if (CurrInterval <= 0)
                {
                    CurrInterval = Interval;
                    SpawnBullet(vector2Player);
                }
            }
            else
            {
                Delay -= elapsed;
            }


            foreach (Bullet bullet in BulletList)
            {
                bullet.Update(gameTime);
            }
        }


        private void SpawnBullet(Vector3 CurrentLookDirection)
        {
            BulletList.Add(new Bullet(this.Position, CurrentLookDirection));
        }

        public void Load()
        {
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            foreach (ModelMesh mesh in ContentContainer.GetObjectFromContainer<Model>("Gun").Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(Rotation.Z), 0, 0) * Matrix.CreateTranslation(Position.X, 0, Position.Y);
                    effect.View = Director.InstanceDirector.Camera.ViewMatrix;
                    effect.Projection = Director.InstanceDirector.Camera.ProjectionMatrix;
                    effect.LightingEnabled = true;
                }
                mesh.Draw();
            }


            foreach (Bullet bullet in BulletList)
            {
                bullet.Draw(graphicsDevice,ef);
            }
        }
    }
}
