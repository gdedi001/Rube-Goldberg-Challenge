using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "FloorPlane") {
            Debug.Log("Hit Floor");
            ResetBall();
        }
    }

    private void ResetBall() {
        float yNudge = 0.5314798f; // used to properly place ball at top of pedistal
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0,0,0);
        //Vector3 resetPos = GetComponentInParent<Transform>().localPosition;
        Vector3 resetPos = GameObject.Find("Pedastal").GetComponent<Transform>().position;
        gameObject.transform.position = new Vector3(resetPos.x, resetPos.y + yNudge, resetPos.z);
        
    }
}
