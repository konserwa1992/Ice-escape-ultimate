using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.GameUtility.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameUtility.Physic
{
   public class Polygon:ICollider
    {
        List<VertexPositionColor> points = new List<VertexPositionColor>();
        public event CollideDetected OnCollision;
        public string Name { get; set; }

        public Polygon(string name)
        {
            this.Name = name;
        }

        public List<VertexPositionColor> Points
        {
            get
            {
                return points;
            }
        }



        public void AddPoint(VertexPositionColor position)
        {
            if (points.Count == 0)
            {
                points.Add(position);
            }
            else
            {
                if (points.Count == 1)
                {
                    points.Add(position);
                }
                else
                {
                    if (points.Count > 2)
                    {
                        points.RemoveAt(points.Count-1);
                        points.RemoveAt(points.Count - 1);
                    }

                    points.Add(points[points.Count - 1]);
                    points.Add(position);
                    points.Add(position);
                    points.Add(points[0]);
                }
            }
        }


        public void RemovePoint(int index)
        {
            points.RemoveAt(index);
        }

       public void Draw(GraphicsDevice graphicsDevice, PrimitiveType primityiveType = PrimitiveType.LineList)
        {
            if (points.Count() >= 2)
            {
                BasicEffect basicEffect;
                basicEffect = new BasicEffect(graphicsDevice);
                basicEffect.VertexColorEnabled = true;
                basicEffect.View = Camera.Director.InstanceDirector.Camera.ViewMatrix;
                basicEffect.Projection = Camera.Director.InstanceDirector.Camera.ProjectionMatrix;
                basicEffect.CurrentTechnique.Passes[0].Apply();
                basicEffect.VertexColorEnabled = true;
                var rayArray = points.ToArray();
                graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, rayArray, 0,
                    rayArray.Length / 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef, PrimitiveType primityiveType = PrimitiveType.LineList)
        {
            foreach (EffectPass pass in ef.CurrentTechnique.Passes)
            {
                // ef.Parameters["xTexture"].SetValue(ContentContainer.GetObjectFromContainer<Texture2D>(TileSheetData.Name));
                ef.Parameters["xWorld"].SetValue(Matrix.Identity);

                pass.Apply();
                graphicsDevice.DrawUserPrimitives(primityiveType, points.ToArray(), 0, points.Count() / 2, VertexPositionColor.VertexDeclaration);
            }
        }

        private bool CircleCollision(ICollider collider)
        {
            Circle interactionObject = (Circle)collider;

            List<VertexPositionColor> filteredPoints = new List<VertexPositionColor>();


            //Zamiast liczyć wszystko liczymy tylko takie odcinki które mogłyby zakleszczyc sie z punktem(graczem)
            for (int i = 0; i < points.Count; i += 2)
            {
                Vector3 segmentDirection = points[i].Position - points[i + 1].Position;
                if (segmentDirection.X > 0)
                {
                    float x1 = interactionObject.Position.X - interactionObject.Radius;
                    float x2 = interactionObject.Position.X + interactionObject.Radius;
                    if ((points[i].Position.X <= x1 && x2 <= points[i + 1].Position.X) ||
                        (points[i + 1].Position.X <= x2 && x2 <= points[i].Position.X) ||
                        (x1 >= points[i].Position.X && points[i + 1].Position.X >= x1) ||
                        (points[i].Position.X >= x1 && x2 >= points[i + 1].Position.X))
                    {
                        VertexPositionColor p0 = points[i];
                        VertexPositionColor p1 = points[i + 1];
                        points[i] = new VertexPositionColor(p0.Position, Color.AliceBlue);
                        points[i + 1] = new VertexPositionColor(p1.Position, Color.AliceBlue);
                        filteredPoints.Add(points[i]);
                        filteredPoints.Add(points[i + 1]);
                    }
                    else
                    {
                        VertexPositionColor p0 = points[i];
                        VertexPositionColor p1 = points[i + 1];
                        points[i] = new VertexPositionColor(p0.Position, Color.Purple);
                        points[i + 1] = new VertexPositionColor(p1.Position, Color.Purple);
                    }
                }
                else
                {
                    float x1 = interactionObject.Position.X + interactionObject.Radius;
                    float x2 = interactionObject.Position.X - interactionObject.Radius;
                    if ((points[i].Position.X <= x1 && x2 <= points[i + 1].Position.X) ||
                        (points[i + 1].Position.X <= x2 && x2 <= points[i].Position.X) ||
                        (x1 >= points[i].Position.X && points[i + 1].Position.X >= x1) ||
                        (points[i].Position.X >= x1 && x2 >= points[i + 1].Position.X))
                    {
                        VertexPositionColor p0 = points[i];
                        VertexPositionColor p1 = points[i + 1];
                        points[i] = new VertexPositionColor(p0.Position, Color.AliceBlue);
                        points[i + 1] = new VertexPositionColor(p1.Position, Color.AliceBlue);
                        filteredPoints.Add(points[i]);
                        filteredPoints.Add(points[i + 1]);
                    }
                    else
                    {
                        VertexPositionColor p0 = points[i];
                        VertexPositionColor p1 = points[i + 1];
                        points[i] = new VertexPositionColor(p0.Position, Color.Purple);
                        points[i + 1] = new VertexPositionColor(p1.Position, Color.Purple);
                    }
                }
            }

            if (filteredPoints.Count >= 2)
            {
                bool isCollide = false;
                for (int i = 0; i < filteredPoints.Count; i += 2)
                {
                    Vector2 point1 = new Vector2(filteredPoints[i].Position.X, filteredPoints[i].Position.Z);
                    Vector2 point2 = new Vector2(filteredPoints[i+1].Position.X, filteredPoints[i+1].Position.Z);

                    Line line = new Line(point1,point2);

                    isCollide = line.IsCollide(collider);

                    if (isCollide)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsCollide(ICollider collider)
        {
            if (collider.GetType() == typeof(Circle))
            {
               return CircleCollision(collider);
            }
            else if(collider.GetType() == typeof(Point))
            {
                return PointCollision(collider);
            }

            return false;
        }

        private bool PointCollision(ICollider collider)
        {
            bool isInside = false;
            Point point = (Point) collider;

            for (int i = 0; i < points.Count; i += 2)
            {
                if (((points[i].Position.Z > point.Position.Y) != (points[i + 1].Position.Z > point.Position.Y)) &&
                    (point.Position.X <
                     (points[i + 1].Position.X - points[i].Position.X) * (point.Position.Y - points[i].Position.Z) /
                     (points[i + 1].Position.Z - points[i].Position.Z) + points[i].Position.X))
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }

        public ICollider YouHaveCollidetWith(ICollider collided)
        {
            throw new NotImplementedException();
        }
    }
}
