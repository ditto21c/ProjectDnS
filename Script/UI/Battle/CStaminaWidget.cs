using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CStaminaWidget : MonoBehaviour
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

        float Stamina = Player.GetStamina();
        float Value = Stamina / 100.0f;
        slider.value = Value;
    }

    CPlayerCharacter Player;
    Slider slider;
}
