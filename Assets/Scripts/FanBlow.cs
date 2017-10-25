using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlow : MonoBehaviour {
    [Range(10, 60)]
    public int fanStrength = 0;

    void OnTriggerStay(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
            Blow(obj);
        }
    }

    void Blow(GameObject item) {
        item.GetComponent<Rigidbody>().AddForce(-transform.forward * fanStrength, ForceMode.Acceleration);
    }
}
