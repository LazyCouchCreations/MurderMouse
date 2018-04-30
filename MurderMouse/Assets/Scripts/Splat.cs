using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour {

    public float splatDuration = 3f;

	// Use this for initialization
	void Start () {
        StartCoroutine(RemoveSplat());
	}

    private IEnumerator RemoveSplat()
    {
        yield return new WaitForSeconds(splatDuration);
        Destroy(gameObject);
    }
}
