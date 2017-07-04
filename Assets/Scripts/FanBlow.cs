using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col) {
        GameObject ball = col.gameObject;
        if (col.gameObject.name == "Ball") {
            Debug.Log("hit fan");
            Blow(ball);
        }
    }

    void Blow(GameObject ball) {

        Debug.Log("Blowing...");
        int fanStrength = 35;
        ball.GetComponent<Rigidbody>().AddForce(-transform.forward * fanStrength, ForceMode.Acceleration);
    }
    
}
