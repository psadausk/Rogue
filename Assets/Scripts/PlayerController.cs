using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;

[RequireComponent(typeof(GameController))]
public class PlayerController : MonoBehaviour {
    private GameController m_controller;


	// Use this for initialization
	void Start () {
        this.m_controller = this.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        Direction? direction = null;
        if ( Input.GetKey(KeyCode.W) ) {
            direction = Direction.North;
        } else if ( Input.GetKey(KeyCode.A) ) {
            direction = Direction.West;
        } else if ( Input.GetKey(KeyCode.S) ) {
            direction = Direction.South;
        } else if ( Input.GetKey(KeyCode.D) ) {
            direction = Direction.East;
        }

        if ( direction.HasValue ) {

        }

	}
}
