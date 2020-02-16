using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryForTime : MonoBehaviour {
    public float time = 2;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
