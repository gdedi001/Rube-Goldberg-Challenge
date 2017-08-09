using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlow : MonoBehaviour {

    void OnTriggerStay(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
            Blow(obj);
        }
    }

    void Blow(GameObject item) {
        int fanStrength = 45;
        item.GetComponent<Rigidbody>().AddForce(-transform.forward * fanStrength, ForceMode.Acceleration);
    }
}
