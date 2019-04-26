using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSpecialPointWidget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<CPlayerCharacter>();

        }

        if (Player == null)
            return;

        slider.value = Player.GetSpecialPoint();
    }

    CPlayerCharacter Player;
    Slider slider;
}
