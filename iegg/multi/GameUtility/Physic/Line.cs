using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameUtility.Map;

namespace Engine.GameUtility.Physic
{
    public class Line : ICollider
    {
        public Vector2 Point1, Point2;
        public string Name { get; set; }
        public event CollideDetected OnCollision;

        public Line(Vector2 point1, Vector2 point2)
        {
            this.Point1 = point1;
            this.Point2 = point2;
        }

        private bool InteractionWithPoint(ICollider Collider)
        {
            Point point = (Point)Collider;
            float lineLen = Vector2.Distance(Point1, Point2);

            float d1 = Vector2.Distance(point.Position, Point1);
            float d2 = Vector2.Distance(point.Position, Point2);

            float precision = 1f;

            if (d1 + d2 >= lineLen - precision && d1 + d2 <= lineLen + precision)
            {
                return true;
            }

            return false;
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef, PrimitiveType primityiveType = PrimitiveType.LineList)
        {
            foreach (EffectPass pass in ef.CurrentTechnique.Passes)
            {
                // ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name));
                ef.Parameters["xWorld"].SetValue(Matrix.Identity);

                VertexPositionColor[] pointsArray = new VertexPositionColor[]{ new VertexPositionColor(new Vector3(Point1.X,2f, Point1.Y),Color.Red), new VertexPositionColor(new Vector3(Point2.X, 2f, Point2.Y), Color.Red )};

                pass.Apply();
                graphicsDevice.DrawUserPrimitives(primityiveType, pointsArray.ToArray(), 0, pointsArray.Count() / 2, VertexPosTexNormal.VertexDeclaration);
            }
        }

        private bool InteractionWithCircle(ICollider Collider)
        {
            Circle CollisionObject = (Circle) Collider;



            if (new Point(Point1).IsCollide(Collider) ||
                new Point(Point2).IsCollide(Collider))
                return true;

            float lenOfSegment = Vector2.Distance(Point1, Point2);
            float dotProduct = (((CollisionObject.Position.X - Point1.X) * (Point2.X - Point1.X)) + ((CollisionObject.Position.Y - Point1.Y) * (Point2.Y - Point1.Y))) / (float)Math.Pow(lenOfSegment, 2);

            float closestX = Point1.X + (dotProduct * (Point2.X - Point1.X));
            float closestY = Point1.Y + (dotProduct * (Point2.Y - Point1.Y));

            bool isOnSegment = InteractionWithPoint(new Point(new Vector2(closestX,closestY)));
            if(!isOnSegment) return false;

            float distX = closestX - CollisionObject.Position.X;
            float distY = closestY - CollisionObject.Position.Y;
            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= CollisionObject.Radius)
                return true;
            return false;
        }

        public bool IsCollide(ICollider Collider)
        {
            Type colliderType = Collider.GetType();
            if (colliderType == typeof(Point))
            {
                return InteractionWithPoint(Collider);
            }else if (colliderType == typeof(Circle))
            {
                return InteractionWithCircle(Collider);
            }

            return false;
        }

        public ICollider YouHaveCollidetWith(ICollider collided)
        {
            throw new NotImplementedException();
        }

    }
}
