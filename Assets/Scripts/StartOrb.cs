using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOrb : MonoBehaviour {
    [SerializeField]
    private GameObject instructions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Ball") {
            removeInstructions();
            playSound();
        }
    }

    void removeInstructions() {
        gameObject.SetActive(false);
            instructions.SetActive(false);
    }

    void playSound() {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
