using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETGame
{

    public class PlayerClass
    {
        public int ID { get; set; }
        public string NickName { get; set; }
        public Vector2 CurrPosition { get; set; }

        public Vector2 StartPosition { get; set; }
        public Vector2 EndPosition { get; set; }
        public float interStep = 0;

        public PlayerClass()
        {
            StartPosition = Vector2.Zero;
        }

        public void Interpolate()
        {
            if (interStep <= 1.0f)
            {
                interStep += 0.30f;

                CurrPosition = Vector2.Lerp(StartPosition, EndPosition,interStep);
            }
        }
    }
}
