using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManager : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;

    // Choose Hand
    [SerializeField]
    private bool isLeftHand;

    // Force applied to thrown objects
    private float throwForce = 1.5f;

    // Menu / Swipping mechanism
    [SerializeField]
    private ObjectMenuManager objectMenu;
    private float swipeSum;
    private float touchLast;
    private float touchCurrent;
    private float distance;
    private bool hasSwipedLeft;
    private bool hasSwipedRight;

    // Teleporter
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject ball; // Used to call associated methods to prevent cheating
    [SerializeField]
    private LayerMask laserMask; // Where you can teleport to
    [SerializeField]
    private GameObject teleportAimerObject; // teleport cylinder
    private LineRenderer laser; // laser pointer
    private Vector3 teleportLocation; // teleport 3D position
    //private float yNudgeAmount = 1f; // specific to teleportAimerObject height


    // Use this for initialization
    void Start() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        device = SteamVR_Controller.Input((int)trackedObj.index);

        // Left hand functionality - teleportation
        if (isLeftHand) {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip)) {
                laser.gameObject.SetActive(true);
                teleportAimerObject.gameObject.SetActive(true);

                laser.SetPosition(0, transform.position); // start laser from hand controller

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 10, laserMask)) {

                    if (hit.transform.gameObject.tag == "PlayArea") {
                        print("Enable ball");
                    }

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

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
                laser.gameObject.SetActive(false);
                teleportAimerObject.gameObject.SetActive(false);
                player.transform.position = teleportLocation;              
            }
        }


        // Right hand functionality - Menu
        if (!isLeftHand) {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
                touchLast = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
            }

            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
                EnableMenu();
                touchCurrent = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x; // get location of thumb with respect to x-axis
                distance = touchCurrent - touchLast;
                touchLast = touchCurrent; // cache our position for the next frame
                swipeSum += distance;

                if (!hasSwipedRight) {
                    if (swipeSum > 0.5f) {
                        swipeSum = 0;
                        SwipeRight();
                        hasSwipedRight = true;
                        hasSwipedLeft = false;
                    }
                }

                if (!hasSwipedLeft) {
                    if (swipeSum < -0.5f) {
                        swipeSum = 0;
                        SwipeLeft();
                        hasSwipedLeft = true;
                        hasSwipedRight = false;
                    }
                }

                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
                    // Spawn object currently selected by menu
                    SpawnObject();
                }
            }

            // Restart variables when finger is lifted
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
                swipeSum = 0;
                touchCurrent = 0;
                touchLast = 0;
                hasSwipedLeft = false;
                hasSwipedRight = false;
                DisableMenu();
            }
        }
    }

    // Physics frames
    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Throwable")) {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
                GrabObject(col);
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
                ThrowObject(col);
            }
        }
    }

    // Physics methods for interacting with objects
    void GrabObject(Collider col) {
        col.transform.SetParent(gameObject.transform); // attach the object to our controller
        col.GetComponent<Rigidbody>().isKinematic = true; // turn off physics
        device.TriggerHapticPulse(8000); // controller vibration
        Debug.Log("You are touching down the trigger on an object.");
    }

    void ThrowObject(Collider col) {
        col.transform.SetParent(null); // detach object from controller
        Rigidbody rigidBody = col.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false; // turn on physics

        // Set velocity based on controller movement
        rigidBody.velocity = device.velocity * throwForce;
        rigidBody.angularVelocity = device.angularVelocity;
        Debug.Log("You have released the trigger");
    }

    
    // Menu interaction methods
    void SpawnObject() {
        objectMenu.SpawnCurrentObject();
    }

    void SwipeLeft() {
        objectMenu.MenuLeft();
        Debug.Log("SwipeLeft");
    }

    void SwipeRight() {
        objectMenu.MenuRight();
        Debug.Log("SwipeRight");
    }

    void EnableMenu() {
        objectMenu.gameObject.SetActive(true);
    }
    
    void DisableMenu() {
        objectMenu.gameObject.SetActive(false);
    }
}
