﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using System.Collections.Generic;
using Assets.Scripts.Data.Entities;
using Assets.Scripts.Graphics;

//[RequireComponent(typeof(TileMap))]
public class GameController : MonoBehaviour {
    public int SizeX;
    public int SizeY;


    private TileMap m_tileMap;
    private EntityMap m_entityMap;
    private Entity m_player;
    private IList<Entity> m_entities;
    private Map m_map;


	// Use this for initialization
	void Start () {
        this.m_tileMap = this.GetComponentInChildren<TileMap>();
        this.m_entityMap = this.GetComponentInChildren<EntityMap>();
        this.ResetGame();
	}


    public void UpdatePlayerPosition(Direction d) {
        //Get the current player position
        var playerPos = m_player.Position;
        var newPos = DirectionUtility.GetPoint(d, playerPos);

        if ( this.m_map.GetTileData(newPos.X, newPos.Y) == TileType.Floor || this.m_map.GetTileData(newPos.X, newPos.Y) == TileType.Stone ) {
            this.m_map.UpdateEntity(this.m_player, newPos.X, newPos.Y);
            this.m_entityMap.UpdateEntity(this.m_player, newPos.X, newPos.Y);
            this.m_player.Position = newPos;
        }
        
        //this.m_tileMap.UpdateEntity(this.m_player)
    }


    //Global functions to reset the state of the game
    public void ResetGame() {

        //For debugging purposes. If the game hasn't been started I still want to render the map for testing
        if(this.m_tileMap == null){
            this.m_tileMap = this.GetComponentInChildren<TileMap>();
        }
        if(this.m_entityMap == null){
            this.m_entityMap = this.GetComponentInChildren<EntityMap>();
        }

        this.ResetMap();
    }

    public void ResetMap() {
        //Reset the game
        this.m_map = new Map(this.SizeX, this.SizeY);
        this.m_tileMap.BuildMesh(this.m_map);
        
        

        //Resest the entities
        this.ResetEntities();
    }

    public void ResetEntities() {
        this.m_entityMap.BuildMesh(this.m_map);
        this.m_player = this.m_map.Entities[0];

    }
}
