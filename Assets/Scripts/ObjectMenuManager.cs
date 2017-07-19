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
        //GameObject goldbergObject = objectPrefabList[currentObject].transform.GetChild(2).gameObject;
        GameObject goldbergObject = objectPrefabList[currentObject];
        Debug.Log(goldbergObject.name);
        Vector3 test = gameObject.transform.parent.gameObject.transform.position;
        // targets the actual object in our menu
        Instantiate(goldbergObject, new Vector3(test.x, test.y, test.z + .8f), goldbergObject.transform.rotation);
    }
}