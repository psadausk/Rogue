using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using System.Linq;
using System.Drawing;
using System;
using Random = UnityEngine.Random;
using Assets.Scripts.Data.Entities;
public class Map {
    public int m_sizeX;
    public int m_sizeY;
    public Tile[,] MapData;
    public List<Entity> Entities;
    List<Room> m_rooms;

    public const int MaxNumOfRooms = 40;
    //public const int MinNumOfRooms = 7;
    public const int MaxNumOfRetries = 100;
    public const int MinRoomSize = 6;
    public const int MaxRoomSize = 8;
    public const int MinCorridorLength = 5;
    public const int MaxCorridorLength = 10;
    private int retryCount = 0;

    public Map() : this(20, 20) { }

    public Map(int sizeX, int sizeY) {
        this.m_sizeX = sizeX;
        this.m_sizeY = sizeY;
        this.m_rooms = new List<Room>();
        this.Entities = new List<Entity>();

        this.MapData = new Tile[this.m_sizeX, this.m_sizeY];
        for ( var x = 0; x < this.m_sizeX; x++ ) {
            for ( var y = 0; y < this.m_sizeY; y++ ) {
                this.MapData[x, y] = new Tile();
            }
        }

        this.BuildDungeonLayout();
        this.PlacePlayer();

    }

    #region Dungeon building
    private void BuildDungeonLayout() {
        //Build a room at a randomlocation
        var size = Random.Range(MinRoomSize, MaxRoomSize);
        var top = Random.Range(0, m_sizeX - size);
        var left = Random.Range(0, m_sizeY - size);
        var room = new Room(top, left, size, size);
        this.m_rooms.Add(room);
        this.TileRoom(room);
        while ( this.m_rooms.Count < MaxNumOfRooms && retryCount < MaxNumOfRetries ) {
            //Pick a random room
            var randRoom = this.m_rooms[Random.Range(0, this.m_rooms.Count - 1)];

            //Get a random wall from that room
            var randomWall = this.FindWallInRoom(randRoom);

            //Ensure that it is actually a wall
            if ( this.MapData[randomWall.First.X, randomWall.First.Y].Type != TileType.Wall || !this.IsRandomWallValid(randomWall.First) ) {
                retryCount++;
                continue;
            }



            //Weight the probability of a wall slightly higher
            var rand = Random.Range(0, 9);
            //Build a room
            if ( rand <= 5 ) {
                //First center the new room on new tile
                room = this.BuildRoom(randomWall.First, Random.Range(MinRoomSize, MaxRoomSize), randomWall.Second);
            } else {
                room = this.BuildCorridor(randomWall.First, Random.Range(MinCorridorLength, MaxCorridorLength), randomWall.Second);
            }
            if ( this.IsRoomValid(room) ) {
                //Debug.Log(room.ToString());

                this.MapData[randomWall.First.X, randomWall.First.Y].Type = TileType.Stone;
                this.TileRoom(room);
                this.m_rooms.Add(room);
            } else {
                retryCount++;
            }
        }
        //Debug.Log(retryCount);
    }

    private Tuple<Point, Direction> FindWallInRoom(Room room) {
        var direction = this.GetRandomDirection();
        var x = -1;
        var y = -1;
        switch ( direction ) {
            case Direction.North:
                y = room.Bottom + room.Height - 1;
                x = Random.Range(room.Left + 1, room.Left + room.Width - 1);
                break;
            case Direction.South:
                y = room.Bottom;
                x = Random.Range(room.Left + 1, room.Left + room.Width - 1);
                break;
            case Direction.East:
                y = Random.Range(room.Bottom + 1, room.Bottom + room.Height - 1);
                x = room.Left + room.Width - 1;
                break;
            case Direction.West:
                y = Random.Range(room.Bottom + 1, room.Bottom + room.Height - 1);
                x = room.Left;
                break;
        }
        return new Tuple<Point, Direction>(new Point(x, y), direction);
    }

    private bool IsRandomWallValid(Point p) {
        //Check east or west
        if ( this.MapData[p.X + 1, p.Y].Type == TileType.Wall &&
            this.MapData[p.X - 1, p.Y].Type == TileType.Wall )
            return true;
        //Check North or South
        if ( this.MapData[p.X, p.Y + 1].Type == TileType.Wall &&
            this.MapData[p.X, p.Y - 1].Type == TileType.Wall )
            return true;
        return false;
    }

    private bool DoesRoomCollideWithExistingRoom(Room newRoom) {

        for ( var y = newRoom.Bottom; y < newRoom.Top; y++ ) {
            for ( var x = newRoom.Left; x < newRoom.Right; x++ ) {
                if ( this.MapData[x, y].Type != TileType.Unknown && this.MapData[x, y].Type != TileType.Wall ) {
                    //Debug.Log("Failed at (" + x + "," + y + "), Tile type was " + MapData[x, y]);
                    return true;
                }
            }
        }
        return false;
    }

    private Direction GetRandomDirection() {
        return (Direction)Random.Range(0, 4);
    }

    private Room BuildCorridor(Point start, int length, Direction startDirection) {
        //Get the largest direction to start from
        switch ( startDirection ) {
            case Direction.North:
                return new Room(start.Y, start.X - 1, length, 3);
            case Direction.South:
                return new Room(start.Y - length + 1, start.X - 1, length, 3);
            case Direction.East:
                return new Room(start.Y - 1, start.X, 3, length);
            case Direction.West:
                return new Room(start.Y - 1, start.X - length + 1, 3, length);
        }
        return null;

    }

    private bool IsRoomValid(Room room) {
        return room.Bottom >= 0 &&
            room.Bottom + room.Height < this.m_sizeY &&
            room.Left >= 0 &&
            room.Left + room.Width < this.m_sizeX && !this.DoesRoomCollideWithExistingRoom(room);
    }

    private Room BuildRoom(Point start, int size, Direction startDirection) {
        //Start is the midpoint of the direction the room expands outwards
        var bottom = start.Y;// - size / 2;
        var left = start.X;// - size / 2;

        switch ( startDirection ) {
            case Direction.North:
                return new Room(bottom, left - size / 2, size, size);
            case Direction.South:
                return new Room(bottom - size + 1, left - size / 2, size, size);
            case Direction.East:
                return new Room(bottom - size / 2, left, size, size);
            case Direction.West:
                return new Room(bottom - size / 2, left - size + 1, size, size);
        }
        throw new Exception();
    }

    private void TileRoom(Room room) {
        for ( var y = 0; y < room.Height; y++ ) {
            for ( var x = 0; x < room.Width; x++ ) {
                if ( //m_mapData[x + room.Left, y + room.Bottom] == TileType.Floor ||
                    MapData[x + room.Left, y + room.Bottom].Type == TileType.Stone ) {
                    continue;
                } else if ( x == 0 || x == room.Width - 1 || y == 0 || y == room.Height - 1 ) {
                    this.MapData[x + room.Left, y + room.Bottom].Type = TileType.Wall;
                } else {
                    this.MapData[x + room.Left, y + room.Bottom].Type = TileType.Floor;
                }
            }
        }
    }

    public Point FindRandomPosition() {
        var room = this.m_rooms[Random.Range(0, this.m_rooms.Count)];
        var randX = Random.Range(room.Left + 1, room.Right - 1);
        var randY = Random.Range(room.Bottom + 1, room.Top - 1);

        return new Point(randX, randY);
    }
    #endregion

    #region Entity Placement
    private void PlacePlayer() {
        var p = this.FindRandomPosition();
        var player = new Player(p.X, p.Y);
        this.MapData[p.X, p.Y].Entity = player;
        this.Entities.Add(player);
    }
    #endregion


    public TileType GetTileData(int x, int y) {
        return this.MapData[x, y].Type;
    }

    public EntityType GetEntityData(int x, int y) {
        return this.MapData[x, y].Entity.EntityType;
    }

    public void UpdateEntity(Entity entity, int x, int y) {
        var oldPos = entity.Position;
        this.MapData[oldPos.X, oldPos.Y].Entity = null;
        this.MapData[x, y].Entity = entity;
    }
}
