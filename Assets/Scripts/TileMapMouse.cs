using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {

    TileMap m_tileMap;
    public Transform Selector;


	// Use this for initialization
	void Start () {
        this.m_tileMap = this.GetComponent<TileMap>();
        this.Selector.localScale = new Vector3(this.m_tileMap.m_tileSize, 1f, this.m_tileMap.m_tileSize);
	}
	
	// Update is called once per frame
	void Update () {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if ( collider.Raycast(ray, out hitInfo, Mathf.Infinity) ) {
            var x = Mathf.FloorToInt(hitInfo.point.x / this.m_tileMap.m_tileSize) * this.m_tileMap.m_tileSize + (this.m_tileMap.m_tileSize /2);
            var y = Mathf.FloorToInt(hitInfo.point.z / this.m_tileMap.m_tileSize) * this.m_tileMap.m_tileSize + ( this.m_tileMap.m_tileSize / 2 );

            var newVec = new Vector3(x, 0, y);
            this.Selector.position = newVec;
            
        } else {
            //renderer.material.color = Color.green;
        }
	}
}
