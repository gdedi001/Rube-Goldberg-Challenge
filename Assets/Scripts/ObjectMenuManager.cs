using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenuManager : MonoBehaviour {
    [SerializeField]
    private List<GameObject> objectList; // automatically handled at start
    public List<GameObject> objectPrefabList; // set manually in inspector and MUST match order of scene menu objects.
    private int currentObject = 0;

    // Use this for initialization
    void Start () {
		foreach (Transform child in transform) {
            objectList.Add(child.gameObject);
        }
	}
	
    public void MenuLeft() {
        objectList[currentObject].SetActive(false);
        currentObject--;

        if (currentObject < 0) {
            currentObject = objectList.Count - 1;
        }

        objectList[currentObject].SetActive(true);
    }

    public void MenuRight() {
        objectList[currentObject].SetActive(false);
        currentObject++;

        if (currentObject > objectList.Count-1) {
            currentObject = 0;
        }

        objectList[currentObject].SetActive(true);
    }

    public void SpawnCurrentObject() {
        GameObject goldbergObject = objectPrefabList[currentObject];
        Vector3 controllerPosition = gameObject.transform.parent.gameObject.transform.position; // targets the position of the actual controller (touch/wand) in 3D space
        float spawnDistance = .8f; // to prevent the prefab from spawning right in front of our face
        Instantiate(goldbergObject, new Vector3(controllerPosition.x, controllerPosition.y, controllerPosition.z + spawnDistance), goldbergObject.transform.rotation);
    }
}