using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [SerializeField]
    private Goal goal;

    // Responsible for enabling/disabling Ball when player is not in playarea
    [SerializeField]
    private GameObject DisabledMarker;
    SphereCollider BallCollider;

    void Start() {
        BallCollider = GetComponent<SphereCollider>();
    }

    void OnCollisionEnter(Collision col) {
        int floorLayer = 8;

        if (col.gameObject.layer == floorLayer) {
            ResetBall();
            ResetCollectibles();
        }
    }

    void ResetBall() {
        float yNudge = 0.5314798f; // used to properly place ball at top of pedistal
        Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3(0,0,0);
        //Vector3 resetPos = GetComponentInParent<Transform>().localPosition;
        Vector3 resetPos = GameObject.Find("Pedastal").GetComponent<Transform>().position;
        gameObject.transform.position = new Vector3(resetPos.x, resetPos.y + yNudge, resetPos.z);      
    }

    public void DisableBall() {
        BallCollider.enabled = false;
        DisabledMarker.SetActive(true);
    }

    public void EnableBall() {
        BallCollider.enabled = true;
        DisabledMarker.SetActive(false);
    }

    void ResetCollectibles() {
        //reset counter for active collectibles
        goal.resetCounter();
        foreach (GameObject collectible in goal.collectibleList) {
            if (!collectible.activeSelf) {
                collectible.SetActive(true);
            }
        }
    }
}
