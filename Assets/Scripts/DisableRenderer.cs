using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRenderer : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		GetComponent<Renderer> ().enabled = false;
		
	}
	
	// Update is called once per frame
	private void Update () {
		
	}
}
