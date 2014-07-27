using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using System.Collections.Generic;
using Assets.Scripts.Data.Entities;
using Assets.Scripts.Graphics;

public class GameController : MonoBehaviour {
    public int SizeX = 30;
    public int SizeY = 30;
    public int TileSize = 5;
    public int TileResolution = 16;

    private TileMap m_tileMap;
    private EntityMap m_entityMap;
    private LightingMap m_lightingMap;
    private Player m_player;
    private IList<Entity> m_entities;
    private Map m_map;

	// Use this for initialization
	void Start () {
        this.m_tileMap = this.GetComponentInChildren<TileMap>();
        this.m_entityMap = this.GetComponentInChildren<EntityMap>();
        this.m_lightingMap = this.GetComponentInChildren<LightingMap>();
        this.ResetGame();
	}

    //Global functions to reset the state of the game
    public void ResetGame() {

        //For debugging purposes. If the game hasn't been started I still want to render the map for testing
        if ( this.m_tileMap == null ) {
            this.m_tileMap = this.GetComponentInChildren<TileMap>();
        }
        if ( this.m_entityMap == null ) {
            this.m_entityMap = this.GetComponentInChildren<EntityMap>();
        }
        if ( this.m_lightingMap == null ) {
            this.m_lightingMap = this.GetComponentInChildren<LightingMap>();
        }

        this.ResetMap();
    }

    public void ResetMap() {
        Debug.Log("GEnerating map");
        //Reset the game
        this.m_map = new Map(this.SizeX, this.SizeY);

        this.m_tileMap.BuildMesh(this.m_map, this.TileSize, this.TileResolution);
        this.m_lightingMap.BuildMesh(this.m_map, this.TileSize, this.TileResolution);
            
        //Resest the entities
        this.ResetEntities();
    }

    public void ResetEntities() {
        this.m_entityMap.BuildMesh(this.m_map, this.TileSize, this.TileResolution);
        this.m_player = this.m_map.Entities[0] as Player;

    }

    public void UpdatePlayerPosition(Direction d) {
        //Get the current player position
        var playerPos = m_player.Position;
        var newPos = DirectionUtility.GetPoint(d, playerPos);
        Debug.Log(newPos);
        if ( this.m_map.GetTileData(newPos.X, newPos.Y) == TileType.Floor || this.m_map.GetTileData(newPos.X, newPos.Y) == TileType.Stone ) {
            this.m_map.UpdateEntity(this.m_player, newPos.X, newPos.Y);
            this.m_map.UpdateLighting(playerPos, newPos, m_player.LineOfSight);
            this.m_entityMap.UpdateEntity(this.m_player, newPos.X, newPos.Y);
            //TODO This call should just take an IEnumerable to set the lighting
            this.m_lightingMap.UpdateLighting(Lighting.NotVisible, this.m_map.GetNeighboringPoints(playerPos, this.m_player.LineOfSight));
            this.m_lightingMap.UpdateLighting(Lighting.Visible, this.m_map.GetNeighboringPoints(newPos, this.m_player.LineOfSight));
            this.m_player.Position = newPos;
            Camera.main.transform.position = new Vector3(newPos.X * TileSize, 10, ( SizeY - newPos.Y ) * -TileSize);
        }
    }
}
