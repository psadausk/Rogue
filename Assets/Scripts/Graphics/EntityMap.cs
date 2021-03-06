﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

namespace Assets.Scripts.Graphics {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class EntityMap : MonoBehaviour {

        private int m_sizeX;
        private int m_sizeY;
        private int m_tileSize = 5;
        private int m_tileResolution = 16;

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
                    if ( map.MapData[x, y].Entity != null ) {
                        Debug.Log("Placing entity");
                        //var p = this.ChoppedTextures[(int)map.GetEntityData(x, y)];
                        var p = this.ChoppedTextures[1];
                        texture.SetPixels(x * this.m_tileResolution, y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution, p);
                    }
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
            texture.SetPixels(oldPos.X * this.m_tileResolution, oldPos.Y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution, this.ChoppedTextures[(int)EntityType.None]);
            texture.SetPixels(x * this.m_tileResolution, y * this.m_tileResolution, this.m_tileResolution, this.m_tileResolution, this.ChoppedTextures[(int)EntityType.Player]);
            texture.Apply();
        }        
    }
}
