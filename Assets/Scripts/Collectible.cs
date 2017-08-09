using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    [SerializeField]
    private Goal goal;


    void OnTriggerEnter(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
           
            collectItem();
            this.gameObject.SetActive(false);
        }
    }

    void collectItem() {
        playCollectSound();
        goal.decrementCounter();
    }

    void playCollectSound() {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
