using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Engine.GameUtility.Map.Elements
{
    public class FinishPoint : IMapElement
    {
        public Vector2 Position { get; set; }
        private Texture2D texture { get; set; }
        private VertexPosTexNormal[] vertexCollection { get; set; }
        public string Key { get; set; } = "Finish";
        public Vector3 Rotation { get; set; } = new Vector3(0,0,0);
        public float Scale { get; set; } = 8.0f;
        public bool Hidden { get; set; } = true;

        public FinishPoint(Vector2 position)
        {
            Position = position;
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            if(vertexCollection == null)
            {
                Load();
            }

            Rotation += new Vector3(0,0.025f,0);
            foreach (EffectPass pass in ef.CurrentTechnique.Passes)
            {
                ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>("Finish"));
                ef.Parameters["xWorld"].SetValue(Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y,Rotation.X, Rotation.Z) * Matrix.CreateTranslation(new Vector3(this.Position.X, 0.5f + (((float)Math.Sin(Rotation.Y)) / 3.0f), this.Position.Y)));
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexCollection, 0, vertexCollection.Count() / 3, VertexPosTexNormal.VertexDeclaration);
            }
        }

        public void Load()
        {
            vertexCollection = new VertexPosTexNormal[6];

            VertexPosTexNormal singleVertex = new VertexPosTexNormal(new Vector3(-1, 0, -1), new Vector2(0, 0), new Vector3(0, 1, 0));
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
            throw new NotImplementedException();
        }
    }
}
