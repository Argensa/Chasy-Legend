using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An entry in the pool
// Contains a smaller pool, and a prefab to know which kind of object this pool contains
public class PoolEntry {
	public GameObject prefab;
	public List<GameObject> list = new List<GameObject>();
	
	public PoolEntry (GameObject pfb) {
		prefab = pfb;
	}
}

// The pool singleton
public class SCR_Pool : MonoBehaviour {
	// This is the pool
	private static List<PoolEntry> entries = new List<PoolEntry>();	
	
	// Get the actual object from the small pool in the entry.
	// If not existed, create the object and put into the small pool
	private static GameObject GetFreeObjectFromEntry (PoolEntry entry, GameObject prefab) {
		GameObject result = null;
		foreach (GameObject gameObject in entry.list) {
			
                if (gameObject != null && !gameObject.activeSelf)
                {
                    result = gameObject;
                    break;
                }
           
           
		}
		
		if (result == null) {
			result = Instantiate (prefab);
			entry.list.Add (result);
		}
		
		result.SetActive (true);
		
		return result;
	}
	
	// Query the big pool to get the entry which contain the requested prefab
	// If such entry not existed, create it, and put into the big pool.
	public static GameObject GetFreeObject (GameObject prefab) {
		PoolEntry result = null;
		foreach (PoolEntry entry in entries) {
			if (entry.prefab == prefab) {
				result = entry;
				break;
			}
		}
		
		if (result == null) {
			result = new PoolEntry(prefab);
			entries.Add(result);
		}
		
		return GetFreeObjectFromEntry (result, prefab);
	}
	
	// Get the list object from the pool for a certain prefab
	public static List<GameObject> GetObjectList (GameObject prefab) {
		PoolEntry result = null;
		foreach (PoolEntry entry in entries) {
			if (entry.prefab == prefab) {
				result = entry;
				break;
			}
		}
		
		if (result == null) {
			result = new PoolEntry(prefab);
			entries.Add(result);
		}
		
		return result.list;
	}
	
	// Deactivate all object in the pool
	public static void DeactivateAllObject () {
		foreach (PoolEntry entry in entries) {
			foreach (GameObject gameObject in entry.list) {
				gameObject.transform.SetParent (null);
				gameObject.SetActive (false);
			}
		}
	}
	
	// Remove all object in the pool
	public static void Flush () {
		entries.Clear ();
	}
}
