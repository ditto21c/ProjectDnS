using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCoroutinesMgr : UnityEngine.Object {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static IEnumerator Run()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
