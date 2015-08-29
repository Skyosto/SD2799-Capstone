using UnityEngine;
using System.Collections;
using System;

public class Inventroy : MonoBehaviour {

	public GameObject[] inventroySlots;
	int[] stackedItemsInSlot;
	bool inventroyIsFull;

	// Use this for initialization
	void Start () {
		inventroySlots = new GameObject[10];
		stackedItemsInSlot = new int[inventroySlots.Length];
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Let's pull out a shovel: "+GetItem("Shovel").name);
	}

	public GameObject GetItem(string name) {
		for (int i = 0; i < inventroySlots.Length; i++) {
			if(inventroySlots[i] != null) {
				if(inventroySlots[i].name == name){
					return inventroySlots[i];
				}
			}
		}
		return null;
	}

	public void AddItemToInventory(GameObject obj) {
		//Check if item is already in inventory
		for (int i = 0; i < inventroySlots.Length; i++) {
			if(inventroySlots[i] == obj){
				stackedItemsInSlot[i] += 1;
				return;
			}
		}
		Debug.Log("Item wasn't in inventory.");

		// Check if there are any available slots left
		for (int i = 0; i < inventroySlots.Length; i++) {
			if(inventroySlots[i] == null) {
				inventroyIsFull = false;
				break;
			}
			if(i == inventroySlots.Length - 1 && inventroySlots[i] != null) {
				inventroyIsFull = true;
			}
		}
		Debug.Log ("Inventory is full: "+inventroyIsFull);

		//Add item to the first available slot if not already in there.
		if (inventroyIsFull == false) {
			for (int i = 0; i < inventroySlots.Length; i++) {
				if (inventroySlots [i] == null) {
					inventroySlots [i] = obj;
					inventroySlots[i].name = inventroySlots[i].name.Replace("(Clone)","");
					return;
				}
			}
		}
	}

	public bool HasItemInInventory(string name) {
		for (int i = 0; i < inventroySlots.Length; i++) {
			if (inventroySlots [i] != null) {
				if (inventroySlots [i].name == name) {
					return true;
				}
			}
		}
		return false;
	}
}
