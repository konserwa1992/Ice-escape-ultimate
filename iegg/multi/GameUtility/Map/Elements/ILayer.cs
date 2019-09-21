using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using multi.GameUtility.Map.Elements.SheetInfo;

namespace multi.GameUtility.Map.Elements
{
    interface ILayer
    {
        VertexPosTexNormal[] TileElements { get; set; }
        Sheet TileSheetData { get; set; }
        int W { get; set; }
        int H { get; set; }

        void LoadMapData(int terrainWidth, int terrainHeight, float terrainZ, int r);
        void LoadTitle(string sheet);
        void Update();
        void Draw(GraphicsDevice graphicsDevice, Effect ef);
    }
}
