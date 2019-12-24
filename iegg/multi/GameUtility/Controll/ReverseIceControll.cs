using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Engine.GameUtility.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.GameUtility.Controll
{
    class ReverseIceControll : IControll
    {
        public Vector2 ClickPosition { get; set; }
        public Vector2 DestinationVector { get; set; } = Vector2.Zero;
        public int SideMultiplier { get; set; } = 0;
        public float GroundSpeed { get; set; } = 1.6f;
        private ButtonState _oldButtonPushed;
        private float prev { get; set; }


        public void Update(GameTime time, Player player, Vector3 planePosition)
        {
            var mouse = Mouse.GetState();

            if (mouse.RightButton == ButtonState.Pressed && _oldButtonPushed == ButtonState.Released)
            {

                ClickPosition = new Vector2(planePosition.X, planePosition.Z);
                //  if (Vector2.Distance(player.Position, ClickPosition) < 15.0f)
                //  { }
                //  else
                {
                    DestinationVector = (ClickPosition - player.Position);
                    DestinationVector = Vector2.Normalize(DestinationVector);

                    var dotProduct = Vector2.Dot(DestinationVector, player.Forward);

                    if (AdditionalMath.MyMath.AngleDir(player.Forward, DestinationVector) >= 0)
                    {
                        SideMultiplier = 1;
                    }
                    else
                    {
                        SideMultiplier = -1;
                    }
                }
            }


            if (DestinationVector != Vector2.Zero)
            {
                var angle = (int) Math.Floor(
                    MathHelper.ToDegrees((float) Math.Acos(Vector2.Dot(player.Forward, (Vector2) DestinationVector))));


                if (angle > 12.5f)
                {
                    DestinationVector = (ClickPosition - player.Position);
                    DestinationVector = Vector2.Normalize(DestinationVector);
                    player.Forward = AdditionalMath.MyMath.Rotate(player.Forward, SideMultiplier * 10.5f);
                    prev = angle;
                }
                else
                {
                    player.Forward = AdditionalMath.MyMath.Rotate(player.Forward, SideMultiplier * angle);
                    angle = (int) Math.Floor(
                        MathHelper.ToDegrees(
                            (float) Math.Acos(Vector2.Dot(player.Forward, (Vector2) DestinationVector))));
                    DestinationVector = Vector2.Zero;
                }
                
            }

            _oldButtonPushed = mouse.RightButton;

        }

        public void Update(Vector3 planePosition)
        {
            throw new NotImplementedException();
        }
    }
}