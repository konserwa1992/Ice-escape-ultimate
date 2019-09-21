using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using multi.GameUtility.Physic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map.Elements
{
    delegate int OnInteract(params object[] interactObjects);
    delegate int OnCollision(ICollider collider);

    public interface IMapElement
    {
        Vector2 Position { get; set; }
        Vector3 Rotation { get; set; }
        float Scale { get; set; }
        string Key { get; set; }
        bool Hidden { get; set; }

        void Load();
        void Update(GameTime gameTime);
        void Draw(GraphicsDevice graphicsDevice, Effect ef);
    }
}
