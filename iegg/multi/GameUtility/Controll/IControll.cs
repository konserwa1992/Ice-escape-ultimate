using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.GameUtility.Control
{
    public interface IControll
	{
		Vector2 ClickPosition { get; set; }
		Vector2 DestinationVector { get; set; }
		float GroundSpeed { get; set; }
		int SideMultiplier { set; get; }
        void Update(GameTime time, Player player, Vector3 planePosition);
	}
}
