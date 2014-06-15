using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data {
    public abstract class Entity {
        public bool Alive { get; set; }
        public EntityType EntityType { get; set; }
        public Point Position { get; set; }
    }
}
