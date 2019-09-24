using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GameUtility.Map.Elements;

namespace Engine.GameUtility.Physic
{
    public delegate void CollideDetected(ICollider collider);

    public interface ICollider
    {
        bool IsCollide(ICollider Collider);
        event CollideDetected OnCollision;
        string Name { get; set; }
    }
}
