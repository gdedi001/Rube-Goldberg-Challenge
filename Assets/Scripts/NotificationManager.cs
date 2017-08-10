using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayAudio();
	}
	
    void PlayAudio() {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
