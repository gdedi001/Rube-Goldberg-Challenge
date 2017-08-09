using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Hand") {
            Debug.Log("Hand Interaction on button");
            // disable instructions when user touches button
            DisableMenu();
        }
    }

    void DisableMenu() {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
