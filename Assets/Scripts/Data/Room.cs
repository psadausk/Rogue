using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assets.Scripts.Data {
    public class Room {
        public int Bottom = 0;
        public int Left = 0;
        public int Height = 0;
        public int Width = 0;

        public bool IsConnected = false;

        public int Right {
            get { return Left + Width- 1; }
        }

        public int Top {
            get { return this.Bottom + Height - 1; }
        }
   


        public Room(int bottom, int left, int height, int width) {
            this.Bottom = bottom;
            this.Left = left;
            this.Height = height;
            this.Width = width;
        }

        public Point GetCenterPoint() {
            return new Point(( this.Bottom + this.Height ) / 2, ( this.Left + this.Width ) / 2);
        }

        //public bool CollidesWith(Room other, TileMap mapData) {
        //    ////Check if one is inside the other
        //    //if ( this.Top <= other.Top &&
        //    //    this.Bottom >= other.Bottom &&
        //    //    this.Left >= other.Left &&
        //    //    this.Right <= other.Right ) 
        //    //    return true;

        //    ////Check vice versa
        //    //if ( this.Top >= other.Top &&
        //    //    this.Bottom <= other.Bottom &&
        //    //    this.Left <= other.Left &&
        //    //    this.Right >= other.Right )
        //    //    return true;




        //    //if ( this.Left > other.Right )
        //    //    return false;
        //    //if ( this.Right < other.Left)
        //    //    return false;
        //    //if ( this.Top > other.Bottom )
        //    //    return false;
        //    //if ( this.Bottom < other.Top ) {
        //    //    return false;
        //    //}
        //    //Console.WriteLine("collided");
        //    //return true;


        //    for ( var y = 0; y < this.Width; y++ ) {
        //        for ( var x = 0; x < this.Width; x++ ) {
        //            mapData.
        //    }
        //}


        public override string ToString() {
            return String.Format("Bottom: {0}, Left: {1}, Height: {2}, Width{3}", this.Bottom, this.Left, this.Height, this.Width);
        }
    }
}
