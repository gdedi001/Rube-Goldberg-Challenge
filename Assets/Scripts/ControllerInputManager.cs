using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    // Teleporter
    private LineRenderer laser; // laser pointer
    public GameObject teleportAimerObject; // teleport cylinder
    public Vector3 teleportLocation; // teleport 3D position
    public GameObject player;
    public LayerMask laserMask; // Where you can teleport to
    //private float yNudgeAmount = 1f; // specific to teleportAimerObject height

    public bool isLeft;

    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        if (isLeft) {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                laser.gameObject.SetActive(true);
                teleportAimerObject.gameObject.SetActive(true);

                laser.SetPosition(0, transform.position); // start laser from hand controller

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 10, laserMask)) {

                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);
                    // aimer position
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, teleportLocation.z);

                }
                else {
                    // teleportLocation = new Vector3(transform.forward.x * 15 + transform.position.x, transform.forward.y * 15 + transform.position.y, transform.forward.z * 15 + transform.position.z);
                    //teleportLocation = transform.position + (transform.forward * 15);

                    RaycastHit groundRay;
                    if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 10, laserMask)) {
                        teleportLocation = new Vector3(transform.forward.x * 10 + transform.position.x, groundRay.point.y, transform.forward.z * 10 + transform.position.z);
                    }

                    laser.SetPosition(1, transform.forward * 10 + transform.position);
                    // aimer position
                    teleportAimerObject.transform.position = teleportLocation + new Vector3(0, 0, 0);
                }
            }

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
                laser.gameObject.SetActive(false);
                teleportAimerObject.gameObject.SetActive(false);
                player.transform.position = teleportLocation;
            }
        }   
    }

    private void OnTriggerStay(Collider col) {
        
    }
}
