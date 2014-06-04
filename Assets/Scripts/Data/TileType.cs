using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Data {
    public enum TileType {
        Unknown,
        Wall , //Wall section, surronds all open areas
        Floor, //Walkable
        Stone, //Unaccessable
        
    }
}
