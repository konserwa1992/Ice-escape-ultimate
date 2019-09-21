using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map
{
    public struct VertexPosTexNormal
    {
        public int Taken;
        public Vector3 position;
        public Vector2 texCoord;
        public Vector3 Normal;
        


        public VertexPosTexNormal(Vector3 position, Vector2 texCoord, Vector3 normal, int taken = 0)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.Normal = normal;
            this.Taken = taken;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
             (
                 new VertexElement(sizeof(int), VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                 new VertexElement(sizeof(int) + (sizeof(float) * 3), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                 new VertexElement(sizeof(int)+(sizeof(float) * 5), VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
             );

        public void SetTaken()
        {
            if (Taken == 0)
                Taken = 1;
            else
                Taken = 0;
        }
    }
}
