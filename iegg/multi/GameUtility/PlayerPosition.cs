using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.GameUtility.Control;
using Microsoft.Xna.Framework;
using Engine.GameUtility.Physic;
using Point = Microsoft.Xna.Framework.Point;
using NETGame;
using Microsoft.Xna.Framework.Input;

namespace Engine.GameUtility
{
	public class Player
	{
		public IControll _controll = new NormalControll();
		private float Speed=0.75f;

		public Vector2 Position = Vector2.Zero;
		public Vector2 Forward = new Vector2(1,0);
        public Vector2 OldForward = new Vector2(1, 0);
        public  bool AliveBoiiii = false;
        public ICollider CollisionObject { get; set; }
        public PlayerClass PlayerNetInfo { get; set; }

        public Player()
		{
			
		}

		public Player(Vector2 startPosition)
		{
			Position = startPosition;
		    CollisionObject = new Circle(Position, 15.0f);
		}

		public void SetControllType(IControll controll)
		{
			_controll = controll;
		}


       


        /// <summary>
        /// Zostawiam to ale wymaga ogólnie modyfikacji myśle ze kamera mogła by być singletonem ten jeden raz(mam nadzieje)
        /// Czyli wyjebac ten parametr clickPos
        /// </summary>
        /// <param name="gameTime"></param>
		public void Update(GameTime gameTime,Vector3 clickPos)
		{
            _controll.Update(gameTime,this,clickPos);


            if (AliveBoiiii == true)
            {
                this.Position += this.Forward * Speed * _controll.GroundSpeed;
                ((Engine.GameUtility.Physic.Point) CollisionObject).Position = this.Position;
            }
		}
	}
}
