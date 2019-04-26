using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRunAwayAIController : CAIController {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        EState curState = Character.curState;
        switch (curState)
        {
            case EState.Idle:
                if (!IsInvoking())
                    Invoke("RandomMove", Random.Range(1.0f, 5.0f));
                break;
            case EState.Move:
                if (Character.paths.Count == 0)
                    Character.Idle();
                break;
        }
    }

    protected void RandomMove()
    {
        Character.RandomMove();
    }
}
