using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNotifyTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string OtherTag = other.gameObject.tag;
        if (OtherTag == "PlayerCharacter" || OtherTag == "EnemyCharacter")
        {
            if (Owner.tag != OtherTag)
            {
                Owner.GetComponent<CRealTreasureChest>().OnNotifyTrigger();
            }
        }
    }

    public GameObject Owner;
    
}
