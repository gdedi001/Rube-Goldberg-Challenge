using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRControllerInputManager : MonoBehaviour {
    private OVRInput.Controller thisController;

    // Choose Hand
    [SerializeField]
    private bool rightHand;

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
    private bool menuIsSwipable;
    private float menuStickX;


    // Teleporter & Player Rotation
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Ball ball; // Used to call associated methods to prevent cheating
    [SerializeField]
    private LayerMask laserMask; // Where you can teleport to
    [SerializeField]
    private LayerMask invalidLaserMask; // Where you cannot teleport to
    [SerializeField]
    private GameObject teleportAimerObject; // teleport cylinder
    private LineRenderer laser; // laser pointer
    private Vector3 teleportLocation; // teleport 3D position
    private string playArea = "PlayArea";
    private float yNudgeAmount = 1.5f; // specific to teleportAimerObject height
    private int maxDistance = 7; // max distance that a player can teleport
    private RaycastHit hit;


    // Use this for initialization
    void Start() {
        laser = GetComponentInChildren<LineRenderer>();

        if (rightHand) {
            thisController = OVRInput.Controller.RTouch;
        }
        else {
            thisController = OVRInput.Controller.LTouch;
        }
    }

    // Update is called once per frame
    void Update() {

        // Left hand functionality - teleportation
        if (!rightHand) {
            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, thisController)) {
                laser.gameObject.SetActive(true);
                teleportAimerObject.gameObject.SetActive(true);

                laser.SetPosition(0, transform.position); // start laser from hand controller

                if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, laserMask)) {
                    teleportLocation = hit.point;
                    laser.SetPosition(1, teleportLocation);
                    // aimer position
                    teleportAimerObject.transform.position = new Vector3(teleportLocation.x, teleportLocation.y + yNudgeAmount, teleportLocation.z);
                }
                /*else if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, invalidLaserMask)) {
                    laser.gameObject.SetActive(false);
                    teleportAimerObject.gameObject.SetActive(false);
                    teleportLocation = player.transform.position;
                }*/
                else {
                    Vector3 tempTeleportLocation = transform.position + (transform.forward * maxDistance);
                    if (Physics.Raycast(tempTeleportLocation, Vector3.down, out hit, maxDistance, laserMask)) {
                        teleportLocation = new Vector3(transform.forward.x * maxDistance + transform.position.x, hit.point.y, transform.forward.z * maxDistance + transform.position.z);
                    }

                    laser.SetPosition(1, transform.forward * maxDistance + transform.position);
                    // aimer position
                    teleportAimerObject.transform.position = teleportLocation + new Vector3(0, 0, 0);
                }
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, thisController)) {
                laser.gameObject.SetActive(false);
                teleportAimerObject.gameObject.SetActive(false);
                player.transform.position = teleportLocation;

                // Prevent cheating - disabled the ball when player is not on platform, enable otherwise
                if (hit.transform.gameObject && hit.transform.gameObject.tag != playArea) {
                    ball.DisableBall();
                }
                else {
                    ball.EnableBall();
                }
            }
        }
      
        // Right hand functionality - Menu
        if (rightHand) {
            menuStickX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, thisController).x;
            
            // enables menu with respective functionality
            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, thisController)) {
                EnableMenu();
                if (menuStickX < 0.45f && menuStickX > -0.45f) {
                    menuIsSwipable = true;
                }
                if (menuIsSwipable) {
                    if (menuStickX >= 0.45f) {
                        // fire function that looks at menuList,
                        // disables current item, and enables next item
                        objectMenu.MenuRight();
                        menuIsSwipable = false;
                    }
                    else if (menuStickX <= -0.45f) {
                        objectMenu.MenuLeft();
                        menuIsSwipable = false;
                    }
                }

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController)) {
                    objectMenu.SpawnCurrentObject();
                }
            }

            // Disables menu when thumb is lifted
            if (OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, thisController)) {
                DisableMenu();
            }
        }
    }
    

    // Physics frames
    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Throwable")) {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController)) {
                GrabObject(col);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, thisController)) {
                ThrowObject(col);
            }
        }

        if (col.gameObject.CompareTag("Structure")) {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController)) {
                GrabObject(col);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, thisController)) {
                PlaceObject(col);
            }
        }
    }

    // Physics methods for interacting with objects
    void GrabObject(Collider col) {
        col.transform.SetParent(gameObject.transform); // attach the object to our controller
        col.GetComponent<Rigidbody>().isKinematic = true; // turn off physics
        //device.TriggerHapticPulse(8000); // controller vibration
    }

    void PlaceObject(Collider col) {
        col.transform.parent = null;
    }

    void ThrowObject(Collider col) {
        col.transform.SetParent(null); // detach object from controller
        Rigidbody rigidBody = col.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false; // turn on physics

        // Set velocity based on controller movement
        rigidBody.velocity = OVRInput.GetLocalControllerVelocity(thisController) * throwForce;
        rigidBody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(thisController);
    }

    
    // Menu interaction methods
    void SpawnObject() {
        objectMenu.SpawnCurrentObject();
    }

    void SwipeLeft() {
        objectMenu.MenuLeft();
    }

    void SwipeRight() {
        objectMenu.MenuRight();
    }

    void EnableMenu() {
        objectMenu.gameObject.SetActive(true);
    }
    
    void DisableMenu() {
        objectMenu.gameObject.SetActive(false);
    }
}
