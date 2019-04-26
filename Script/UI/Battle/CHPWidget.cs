using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHPWidget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        Sprite[] FindSprites = CResourceMgr.LoadSprite("IT");
        Sprites = new Sprite[4];
        for(int i=0; i<4; ++i)
            Sprites[i] = FindSprites[87+i];
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public enum EHPWidgetType
    {
        Full = 0,
        Half,
        Zero,
        NotFull,
    }
    public void SetImage(EHPWidgetType Type)
    {
        image.sprite = Sprites[(int)Type];
    }

    Image image;
    Sprite[] Sprites;
}
