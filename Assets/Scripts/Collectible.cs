using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
    [SerializeField]
    private Goal goal;

    void OnTriggerEnter(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
            this.gameObject.SetActive(false);
            collectItem();
            //Debug.Log("Available Stars " + goal.getAvailableCollectibles());
        }
    }

    void collectItem() {
        goal.decrementCounter();
    }
}
