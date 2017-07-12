using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenuManager : MonoBehaviour {
    public List<GameObject> objectList; // automatically handled at start
    public List<GameObject> objectPrefabList; // set manually in inspector and MUST match order of scene menu objects.

    public int currentObject = 0;

    // Use this for initialization
    void Start () {
		foreach (Transform child in transform) {
            objectList.Add(child.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
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
        GameObject item = objectPrefabList[currentObject].transform.GetChild(0).gameObject;
        // targets the actual object in our menu
        Instantiate(item, item.transform.position, item.transform.rotation);
    }
}