using Microsoft.Xna.Framework.Graphics;
using Engine.GameUtility.Control;
using Engine.GameUtility.Physic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.GameUtility.Map.Elements.FloorType
{
    public interface IFloor
    {
        Polygon FloorPolygon { get; set; }
        IControll ControllType { get; set; }
        string Key { get; set; }
        bool Hidden { get; set; }

        void Draw(GraphicsDevice graphicsDevice, Effect ef);
        void Update(GameTime gameTime);
    }
}
