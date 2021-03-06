﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    [SerializeField]
    private SteamVR_LoadLevel loadLevel;
    [SerializeField]
    public List<GameObject> collectibleList; // holds all collectible prefabs in scene
    private bool levelCompleted = false; // flag to determine if the player has collected all collectibles

    private static int availableCollectibes;
    public int AvailableCollectibles {
        get {
            return availableCollectibes;
        }
        set {
            availableCollectibes = value;
        }
    }

    // Use this for initialization
    void Start () {
        resetCounter();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public int getAvailableCollectibles() {
        return availableCollectibes;
    }

    public void resetCounter() {
        AvailableCollectibles = collectibleList.Count;
    }

    public void decrementCounter() {
        AvailableCollectibles--;
    }

    private void sceneLoad(string nextScene) {
        if (levelCompleted) {
            SteamVR_LoadLevel.Begin(nextScene);
        }
    }

    private void checkStatus() {
        if (getAvailableCollectibles() == 0) {
            levelCompleted = true;
            loadLevel.Trigger();
        }
        else {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter(Collider col) {
        GameObject obj = col.gameObject;
        if (obj.name == "Ball") {
            checkStatus();
        }
    }
}
