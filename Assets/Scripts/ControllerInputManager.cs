using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {
    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    // teleporter
    public GameObject player;
    private LineRenderer laser; // laser pointer
    public GameObject teleportAimerObject; // cylinder used to show teleport location
    public Vector3 teleportLocation; // teleport 3D position
    public LayerMask laserMask; // Where you can teleport to
    private float yNudgeAmount = 1f; // specific to teleportAimerObject height

    public bool isLeft;

    // Use this for initialization
    void Start () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // We only want the left controller to be used for teleporting
        if (isLeft) {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                // enable laser and cylinder
                laser.gameObject.SetActive(true);
                teleportAimerObject.gameObject.SetActive(true);

                laser.SetPosition(0, gameObject.transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, laserMask)) {
                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);

                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);

                }
            }
        }
	}
}
