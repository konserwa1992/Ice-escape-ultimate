using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using multi.GameUtility.Control;
using multi.GameUtility.Physic;


namespace multi.GameUtility.Map.Elements.FloorType
{
    class RegularIce: IFloor
    {
        public Polygon FloorPolygon { get; set; } = new Polygon("RegularIce");
        public bool Hidden { get; set; } = false;
        public IControll ControllType { get ; set; } = new NormalControll();
        public string Key { get; set; }

        public RegularIce(string name)
        {
            this.Key = name;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect ef)
        {
            if (Hidden == false)
            {
                FloorPolygon.Draw(graphicsDevice);
            }
        }
    }
}
