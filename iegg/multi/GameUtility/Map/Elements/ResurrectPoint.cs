using Engine.GameUtility;
using Engine.GameUtility.Map;
using Engine.GameUtility.Map.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map.Elements
{
    public class ResurrectPoint : IMapElement
    {
        public Vector2 Position { get; set; }
        public Vector3 Rotation { get; set ; }
        private VertexPosTexNormal[] vertexCollection { get; set; }
        public float Scale { get; set; }
        public string Key { get; set; }
        public bool Hidden { get; set; }

        public ResurrectPoint(string name)
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
                Rotation += new Vector3(0, 0.025f, 0);
                foreach (EffectPass pass in ef.CurrentTechnique.Passes)
                {
                    ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>("Resurrect"));
                    ef.Parameters["xWorld"].SetValue(Matrix.CreateScale(8) * Matrix.CreateFromYawPitchRoll(Rotation.Y, 0, 0) * Matrix.CreateTranslation(new Vector3(this.Position.X, 1f + (((float)Math.Sin(Rotation.Y)) / 5f), this.Position.Y)));
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexCollection, 0, vertexCollection.Count() / 3, VertexPosTexNormal.VertexDeclaration);
                }
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
            //throw new NotImplementedException();
        }
    }
}
