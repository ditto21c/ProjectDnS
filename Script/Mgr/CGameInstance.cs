﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameInstance : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Inventory = new CInventory();
    }
    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public CInventory Inventory { get; set; }
}
