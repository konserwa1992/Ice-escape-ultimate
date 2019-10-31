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
        public float interstepAdd = 0.1666666f;

        public PlayerClass()
        {
            StartPosition = Vector2.Zero;
        }

        public void Interpolate()
        {
            interStep += interstepAdd;
            CurrPosition = Vector2.Lerp(StartPosition, EndPosition,MathHelper.Clamp(interStep,0.0f,1.0f));
        }
    }
}
