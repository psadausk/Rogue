using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using Point = System.Drawing.Point;
using System.Collections.Generic;

namespace Assets.Scripts.Graphics {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class LightingMap : MonoBehaviour {

        private int m_sizeX;
        private int m_sizeY;
        private int m_tileSize;
        private int m_tileResolution;
        private Color[][] ChoppedTextures;

        public Texture2D EntityTiles;


        // Use this for initialization
        void Start() { }

        Color[][] ChopUpTiles() {
            var textureWidth = this.m_sizeX * this.m_tileResolution;
            var textureHeight = this.m_sizeY * this.m_tileResolution;
            var numTilesPerRow = this.EntityTiles.width / m_tileResolution;
            var numRows = this.EntityTiles.height / m_tileResolution;
            var tiles = new Color[numTilesPerRow * numRows][];
            Debug.Log(numTilesPerRow);
            Debug.Log(numRows);
            for ( var y = 0; y < numRows; y++ ) {
                for ( var x = 0; x < numTilesPerRow; x++ ) {
                    tiles[y * numTilesPerRow + x] = this.EntityTiles.GetPixels(x * this.m_tileResolution, y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution);
                }
            }
            return tiles;
        }


        public void BuildMesh(Map map, int tileSize, int tileResolution) {
            this.m_sizeX = map.m_sizeX;
            this.m_sizeY = map.m_sizeY;
            this.m_tileSize = tileSize;
            this.m_tileResolution = tileResolution;


            var numTiles = this.m_sizeX * this.m_sizeY;
            var numTri = numTiles * 2;

            var vSizeX = this.m_sizeX + 1;
            var vSizeY = this.m_sizeY + 1;
            var numVert = vSizeX * vSizeY;
            var x = 0;
            var y = 0;

            var vertices = new Vector3[numVert];
            var triangles = new int[numTri * 3];
            var normals = new Vector3[numVert];
            var uv = new Vector2[numVert];

            for ( y = 0; y < vSizeY; y++ ) {
                for ( x = 0; x < vSizeX; x++ ) {
                    vertices[y * vSizeX + x] = new Vector3(x * this.m_tileSize, 0, -y * this.m_tileSize);
                    normals[y * vSizeX + x] = Vector3.up;
                    uv[y * vSizeX + x] = new Vector2((float)x / m_sizeX, 1f - (float)y / m_sizeY);
                }
            }
            x = 0;
            for ( y = 0; y < this.m_sizeY; y++ ) {
                for ( x = 0; x < this.m_sizeX; x++ ) {
                    var squareOffset = y * m_sizeX + x;
                    var triOffset = squareOffset * 6;

                    triangles[triOffset + 0] = y * vSizeX + x + 0;
                    triangles[triOffset + 2] = y * vSizeX + x + vSizeX + 0;
                    triangles[triOffset + 1] = y * vSizeX + x + vSizeX + 1;


                    triangles[triOffset + 3] = y * vSizeX + x + 0;
                    triangles[triOffset + 5] = y * vSizeX + x + vSizeX + 1;
                    triangles[triOffset + 4] = y * vSizeX + x + 1;
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
            this.ChoppedTextures = this.ChopUpTiles();
            var textureWidth = this.m_sizeX * this.m_tileResolution;
            var textureHeight = this.m_sizeY * this.m_tileResolution;
            var texture = new Texture2D(textureWidth, textureHeight);
            for ( var y = 0; y < m_sizeY; y++ ) {
                for ( var x = 0; x < m_sizeX; x++ ) {
                    var p = this.ChoppedTextures[(int)map.MapData[x,y].Lighting];
                    Debug.Log(map.MapData[x,y].Lighting);
                    texture.SetPixels(x * this.m_tileResolution, y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution, p);
                }
            }
            texture.filterMode = FilterMode.Point;

            texture.Apply();

            var meshRender = GetComponent<MeshRenderer>();
            meshRender.sharedMaterials[0].mainTexture = texture;
        }

        public void UpdateLighting(Lighting lighting, IEnumerable<Point> points) {
            var meshRender = this.GetComponent<MeshRenderer>();
            var texture = meshRender.sharedMaterials[0].mainTexture as Texture2D;
            foreach ( var p in points ) {
                texture.SetPixels(p.X * this.m_tileResolution, p.Y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution, this.ChoppedTextures[(int)lighting]);
            }
            texture.Apply();
        }    
    }
}
