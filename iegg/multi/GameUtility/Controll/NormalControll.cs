using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using multi.GameUtility.Physic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace multi.GameUtility.Control
{

    /// <summary>
    /// Pisałem ten kod pod gre 2D niestety nie mam pomysłu na lepsze rozwiązanie wiec zakładam że 2D zadziala na 3D.Also nie chce mi sie przepisywać tego na 3D.
    /// Poprostu trzeba to jeszcze obudować do 3d.
    /// </summary>
	class NormalControll : IControll
	{
		public Vector2 ClickPosition { get; set; }
		public Vector2 DestinationVector { get; set; } = Vector2.Zero;
		public int SideMultiplier { get; set; } = 0;
		public float GroundSpeed { get; set; } = 1.6f; 
	    private ButtonState _oldButtonPushed;
        private float prev { get; set; }



        public string TESTANGLE { get; set; }="";

		public void Update(GameTime time, Player player,Vector3 planePosition)
		{
			var mouse = Mouse.GetState();

			if (mouse.RightButton == ButtonState.Pressed && _oldButtonPushed == ButtonState.Released)
			{
              
                ClickPosition = new Vector2(planePosition.X, planePosition.Z);
                    DestinationVector = (ClickPosition - player.Position);
                    DestinationVector = Vector2.Normalize(DestinationVector);

                    var dotProduct = Vector2.Dot(DestinationVector, player.Forward);

                    if (AdditionalMath.MyMath.AngleDir(player.Forward, DestinationVector) >= 0)
                    {
                        SideMultiplier = -1;
                    }
                    else
                    {
                        SideMultiplier = 1;
                    }
        
			}


			if (DestinationVector != Vector2.Zero)
			{
				var angle = (int)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(player.Forward, (Vector2)DestinationVector))));


				if (angle > 12.5f)
				{
                        DestinationVector = (ClickPosition - player.Position);
					    DestinationVector = Vector2.Normalize(DestinationVector);
					    player.Forward = AdditionalMath.MyMath.Rotate(player.Forward, SideMultiplier * 10.5f);
                        prev = angle;
                }
				else
				{
				    player.Forward = AdditionalMath.MyMath.Rotate(player.Forward, SideMultiplier* angle);
				    angle = (int)Math.Floor(MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(player.Forward, (Vector2)DestinationVector))));
                    DestinationVector = Vector2.Zero;
                }

                TESTANGLE = angle.ToString();
            }

		    _oldButtonPushed = mouse.RightButton;

		}
	}
}
