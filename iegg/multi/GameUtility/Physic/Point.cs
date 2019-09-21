using System.CodeDom;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace multi.GameUtility.Physic
{
    internal class Point : ICollider
    {
        public Vector2 Position { get; set; }
        public string Name { get; set; }

        public Point(Vector2 position)
        {
            this.Position = position;
        }

        public event CollideDetected OnCollision;

        private bool CircleCollision(ICollider Collider)
        {
            Circle circle = (Circle)Collider;
            float distance = Vector2.Distance(circle.Position, Position);

            if (distance <= circle.Radius)
            {
                return true;
            }

            return false;
        }

        private bool PolygonCollision(ICollider Collider)
        {
            Polygon polygon = (Polygon)Collider;
            return polygon.IsCollide(Collider);
        }


        public bool IsCollide(ICollider Collider)
        {
            if (typeof(Circle) == Collider.GetType())
            {
                return CircleCollision(Collider);
            }
            else if(typeof(Polygon) == Collider.GetType())
            {
                PolygonCollision(Collider);
            }
            
            return false;
        }
    }
}
