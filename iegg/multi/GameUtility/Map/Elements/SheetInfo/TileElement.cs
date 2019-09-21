using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi.GameUtility.Map.Elements.SheetInfo
{
    public class TilePosition
    {
        public int X;
        public int Y;
    }

    public class ElementsGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TilePosition> Positions { get; set; }
        public int[] Pattern { get; set; }
    }

    public class TileElement
    {
        public List<ElementsGroup> Groups { get; set; }
    }
}
