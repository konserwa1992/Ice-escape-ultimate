using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace multi.GameUtility.AdditionalMath
{
	class MyMath
	{
		/// <summary>
		/// Metoda sprawdzająca po której stronie znajduje sie wektor B względem wektora A
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		public static float AngleDir(Vector2 A, Vector2 B)
		{
			return -A.X * B.Y + A.Y * B.X;
		}

		/// <summary>
		/// Obracanie wektora w stopniach[-180:0,0:180]
		/// </summary>
		/// <param name="v"></param>
		/// <param name="degrees"></param>
		/// <returns></returns>
		public static Vector2 Rotate(Vector2 v, float degrees)
		{
			return RotateRadians(v, MathHelper.ToRadians(degrees));
		}

		public static Vector2 RotateRadians(Vector2 v, float radians)
		{
			var ca = (double)Math.Cos(radians);
			var sa = (double)Math.Sin(radians);
			return new Vector2((float)(ca * v.X - sa * v.Y), (float)(sa * v.X + ca * v.Y));
		}
	}

}
