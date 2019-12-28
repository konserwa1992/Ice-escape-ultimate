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
        
        public void UpdatePosition(Vector2 position)
        {
            Position = position;
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

            return collision;
        }

        private bool CircleCollision(ICollider collider)
        {
            Circle circleCollider = (Circle) collider;
            float distanceBettwen = (float)Math.Sqrt(
            ((Position.X - circleCollider.Position.X) * (Position.X - circleCollider.Position.X))
            +((Position.Y - circleCollider.Position.Y) * (Position.Y - circleCollider.Position.Y))
                );
            bool collision = false;

            if (distanceBettwen <= this.Radius + circleCollider.Radius)
            {
                collision = true;
            }

            return collision;
        }


        /// <summary>
        /// Dopisać testy jednostkowe
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public bool IsCollide(ICollider collider)
        {
            bool isCollide = false;
            if (typeof(Circle) == collider.GetType())
            {
                isCollide=CircleCollision(collider);
            }
            else if(typeof(Line) == collider.GetType())
            {
                isCollide = LineCollision(collider);
            }
            else if (typeof(Polygon) == collider.GetType())
            {
                isCollide = PolygonCollision(collider);
            }

            if (isCollide == true &&  OnCollision != null)
            {
                OnCollision(collider);
            }
            
            return isCollide;
        }

        public ICollider YouHaveCollidetWith(ICollider collider)
        {
            
            return collider;
        }
    }
}
