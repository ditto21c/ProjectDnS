using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrapTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "PlayerCharacter" || tag == "EnemyCharacter")
        {
            CTrap Trap = gameObject.GetComponentInParent<CTrap>();
            Trap.ExcuteTrigger();
        }
    }
   
}
