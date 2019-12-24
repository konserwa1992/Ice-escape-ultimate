using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.GameUtility.Physic
{
    public class Circle : ICollider
    {
        public float Radius;
        public Vector2 Position;
        public string Name { get; set; }

        public event CollideDetected OnCollision;

        public Circle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public Circle(ref Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        private bool PolygonCollision(ICollider collider)
        {
           //Odgrzewam kod z klasy Polygon
           Polygon polygon = (Polygon) collider;
           return polygon.IsCollide(this);
        }

        private bool LineCollision(ICollider collider)
        {
            Line line = (Line) collider;
            bool collision = line.IsCollide(this);

            if (collision)
            {
                this.OnCollision(collider);
            }

            return collision;
        }

        public bool IsCollide(ICollider collider)
        {
            YouHaveCollidetWith(collider);
            return false;
        }

        public ICollider YouHaveCollidetWith(ICollider collider)
        {
            OnCollision(collider);
            return collider;
        }
    }
}
