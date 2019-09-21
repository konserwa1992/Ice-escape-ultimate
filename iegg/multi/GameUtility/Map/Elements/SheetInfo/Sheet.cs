using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map.Elements.SheetInfo
{
    public class Sheet
    {
        public string Name { get; set; }
        public string TextureName { get; set; }
        public int TileElementHeight { set; get; }
        public int TileElementWeight { set; get; }
        public TileElement[] TileElements { get; set; }

        //private Texture2D SpriteSheet {get;set;}

       // public Texture2D Texture { get { return SpriteSheet; } }

        public void Load(ContentManager content)
        {
           
        }
}
}
