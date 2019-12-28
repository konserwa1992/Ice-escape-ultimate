using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GameUtility.Physic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.GameUtility.Map.Elements
{
    public class SpawnPoint : IMapElement
    {
        public Vector2 Position { get; set; } = new Vector2(0, 0);
        private VertexPosTexNormal[] vertexCollection {get;set;}
        public string Key { get; set; }
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public float Scale { get; set; } = 8.0f;
        public bool Hidden { get; set; } = true;


        public ICollider Collider {get;private set;}
        //Effect effect;

        public SpawnPoint(string name)
        {
            this.Key = name;
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            if (vertexCollection == null)
            {
                Load();
            }

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullClockwiseFace;
            rs.FillMode = FillMode.Solid;
            graphicsDevice.RasterizerState = rs;

            if (Position.X > 0 && Position.Y > 0)
            {
                Rotation += new Vector3(0,0.025f,0);
                foreach (EffectPass pass in ef.CurrentTechnique.Passes)
                {
                    ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>("Spawn"));
                    ef.Parameters["xWorld"].SetValue(Matrix.CreateScale(8) * Matrix.CreateFromYawPitchRoll(Rotation.Y,0,0) * Matrix.CreateTranslation(new Vector3(this.Position.X, 1f + (((float)Math.Sin(Rotation.Y)) / 5f), this.Position.Y)));
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexCollection, 0, vertexCollection.Count() / 3, VertexPosTexNormal.VertexDeclaration);
                }
            }
        }

        public void Load()
        {
            Collider = new Circle(Position,32);

            vertexCollection = new VertexPosTexNormal[6];

            VertexPosTexNormal singleVertex = new VertexPosTexNormal(new Vector3(-1, 0, -1),new Vector2(0, 0), new Vector3(0,1,0));
            //singleVertex.Color = Color.White;
            vertexCollection[0] = singleVertex;

            singleVertex = new VertexPosTexNormal(new Vector3(-1, 0, 1), new Vector2(0, 1), new Vector3(0, 1, 0));
           // singleVertex.Color = Color.White;
            vertexCollection[1] = singleVertex;

            singleVertex = new VertexPosTexNormal(new Vector3(1, 0, -1), new Vector2(1, 0), new Vector3(0, 1, 0));
           // singleVertex.Color = Color.White;
            vertexCollection[2] = singleVertex;

            singleVertex = new VertexPosTexNormal(new Vector3(-1, 0, 1), new Vector2(0, 1), new Vector3(0, 1, 0));
           // singleVertex.Color = Color.White;
            vertexCollection[3] = singleVertex;

            singleVertex = new VertexPosTexNormal(new Vector3(1, 0, 1), new Vector2(1, 1), new Vector3(0, 1, 0));
           // singleVertex.Color = Color.White;
            vertexCollection[4] = singleVertex;

            singleVertex = new VertexPosTexNormal(new Vector3(1, 0, -1), new Vector2(1, 0), new Vector3(0, 1, 0));
           // singleVertex.Color = Color.White;
            vertexCollection[5] = singleVertex;
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
