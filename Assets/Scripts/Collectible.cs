using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
            this.gameObject.SetActive(false);
        }
    }
}
