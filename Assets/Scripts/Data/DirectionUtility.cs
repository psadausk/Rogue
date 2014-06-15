using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data {
    public static class DirectionUtility {
        public static Point GetPoint(Direction d, Point p) {
            switch ( d ) {
                case Direction.North:
                    return new Point(p.X, p.Y +1);                     
                case Direction.West:
                    return new Point(p.X -1, p.Y);
                case Direction.South:
                    return new Point(p.X, p.Y - 1);
                case Direction.East:
                    return new Point(p.X+1, p.Y);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
