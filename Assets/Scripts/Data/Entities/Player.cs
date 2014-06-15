using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data.Entities {
    public class Player : Entity{

        public Player(int x, int y) {
            this.Position = new Point(x, y);
        }
    }
}
