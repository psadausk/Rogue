using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameController))]
public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
        this.DrawDefaultInspector();
        if ( GUILayout.Button("Regenerate") ) {
            //var tileMap = (TileMap)this.target;
            var gameController = (GameController)this.target;
            gameController.ResetGame();

            //tileMap.BuildMesh();

        }
    }
}
