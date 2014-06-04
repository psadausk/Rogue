using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
        this.DrawDefaultInspector();
        if ( GUILayout.Button("Regenerate" )) {
            var tileMap = (TileMap)this.target;
            tileMap.BuildMesh();
        }
    }
}
