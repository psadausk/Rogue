using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {

    private int m_sizeX;
    private int m_sizeY;
    public int m_tileSize = 5;
    //public Map Map;
    
    private Color[][] ChoppedTextures;

    public Texture2D MapTiles;
    public int TileResolution = 16;

	// Use this for initialization
	void Start () {
        //this.BuildMesh();        
	}

    Color[][] ChopUpTiles() {
        var textureWidth = this.m_sizeX * this.TileResolution;
        var textureHeight = this.m_sizeY * this.TileResolution;
        var numTilesPerRow = this.MapTiles.width/TileResolution;
        var numRows = this.MapTiles.height / TileResolution;
        var tiles = new Color[numTilesPerRow * numRows][];
        Debug.Log(numTilesPerRow);
        Debug.Log(numRows);
        for ( var y = 0; y < numRows; y++ ) {
            for ( var x = 0; x < numTilesPerRow; x++ ) {
                tiles[y * numTilesPerRow + x] = this.MapTiles.GetPixels(x * this.TileResolution, y * this.TileResolution, this.TileResolution, this.TileResolution);
            }
        }
        return tiles;
    }
	

    public void BuildMesh(Map map) {


        this.m_sizeX = map.m_sizeX;
        this.m_sizeY = map.m_sizeY;


        Debug.Log("Size x = " + this.m_sizeX);
        //this.EntityTextures = new Texture2D(this.m_sizeX * TileResolution, this.m_sizeY * TileResolution);

        var numTiles = this.m_sizeX * this.m_sizeY;
        var numTri = numTiles * 2;

        var vSizeX = this.m_sizeX + 1;
        var vSizeY = this.m_sizeY + 1;
        var numVert = vSizeX * vSizeY;
        var x = 0;
        var y = 0;        

        var vertices = new Vector3[numVert];
        var triangles = new int[numTri*3];
        var normals = new Vector3[numVert];
        var uv = new Vector2[numVert];

        for ( y = 0; y < vSizeY; y++ ) {
            for ( x = 0; x < vSizeX; x++ ) {
                vertices[y * vSizeX + x] = new Vector3(x * this.m_tileSize, 0, -y * this.m_tileSize);
                normals[y * vSizeX + x] = Vector3.up;
                uv[y * vSizeX + x] = new Vector2((float)x /m_sizeX, 1f- (float)y /m_sizeY);
            }
        }
        x = 0;
        for ( y = 0; y < this.m_sizeY; y++ ) {
            for ( x = 0; x < this.m_sizeX; x++ ) {
                var squareOffset = y * m_sizeX + x;
                var triOffset = squareOffset * 6;

                triangles[triOffset + 0] = y * vSizeX + x          + 0;
                triangles[triOffset + 2] = y * vSizeX + x + vSizeX + 0;
                triangles[triOffset + 1] = y * vSizeX + x + vSizeX + 1;
                

                triangles[triOffset + 3] = y * vSizeX + x          + 0;                                
                triangles[triOffset + 5] = y * vSizeX + x + vSizeX + 1;
                triangles[triOffset + 4] = y * vSizeX + x          + 1;
            }
        }

        var mesh = new Mesh {
            vertices = vertices,
            triangles = triangles,
            normals = normals, 
            uv = uv
        };


        var meshFilter = GetComponent<MeshFilter>();
        var meshRender = GetComponent<MeshRenderer>();
        var meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        BuildTexture(map);
    }

    void BuildTexture(Map map) {

        //this.Map = new Map(m_sizeX, m_sizeY);
        this.ChoppedTextures = this.ChopUpTiles();
        var textureWidth = this.m_sizeX * this.TileResolution;
        var textureHeight = this.m_sizeY * this.TileResolution;
        var texture = new Texture2D(textureWidth, textureHeight);
        for ( var y = 0; y < m_sizeY; y++ ) {
            for ( var x = 0; x < m_sizeX; x++ ) {
                var p = this.ChoppedTextures[(int)map.GetTileData(x,y) ];
                texture.SetPixels(x * this.TileResolution, y * this.TileResolution, this.TileResolution, this.TileResolution, p);
            }
        }
        texture.filterMode = FilterMode.Point;

        texture.Apply();

        var meshRender = GetComponent<MeshRenderer>();
        meshRender.sharedMaterials[0].mainTexture = texture;

    }

    public void UpdateEntity(Entity entity, int x, int y) {
        var oldPos = entity.Position;
        var meshRender = this.GetComponent<MeshRenderer>();
        var texture = meshRender.sharedMaterials[0].mainTexture as Texture2D;
        //texture.SetPixels(oldPos.X * this.TileResolution, oldPos.Y * this.TileResolution, this.TileResolution, this.TileResolution, this.ChoppedTextures[1]);
        texture.SetPixels(x * this.TileResolution, y * this.TileResolution, this.TileResolution, this.TileResolution, this.ChoppedTextures[1]);
        texture.Apply();
    }
}
