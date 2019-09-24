using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.GameUtility.Map.Elements.SpawnPoint;
using static Engine.GameUtility.Map.Map;

namespace Engine.GameUtility.Map
{
    /// <summary>
    /// Szajs od obliczania płaszczyzn sterowania.
    /// </summary>
   public class PlaneControll
    {
       public VertexPosTexNormal[] controlMap;

       public float A { get; set; }
       public float B { get; set; }
       public float C { get; set; }
       public float D { get; set; }

        public void CalculateByPoints(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            float[] vector1 = new float[] { v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z };
            float[] vector2 = new float[] { v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z };

            A = vector1[1] * vector2[2] + (-1 * vector2[1] * vector1[2]);
            B = -1*(vector1[0] * vector2[2] + (-1 * vector2[0] * vector1[2]));
            C = vector1[0] * vector2[1] + (-1 * vector2[0] * vector1[1]);

            D = 0 * A + 0 * B + C * 1;
        }

        public static Vector3 IntersectPoint(Vector3 rayVector, Vector3 rayPoint, Vector3 planeNormal, Vector3 planePoint)
        {
            var diff = rayPoint - planePoint;
            var prod1 = Vector3.Dot(diff,planeNormal);
            var prod2 = Vector3.Dot(rayVector,planeNormal);

            var prod3 = prod1 / prod2;
            return rayPoint - rayVector * prod3;
        }

    }
}
