using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        GameInstance = GameObject.FindGameObjectWithTag("GameInstance").GetComponent<CGameInstance>();
    }

    // Update is called once per frame
    void Update()
    {
        int Gold = GameInstance.Inventory.GetGold();
        text.text = Gold.ToString();
    }

    Text text;
    CGameInstance GameInstance;
}
