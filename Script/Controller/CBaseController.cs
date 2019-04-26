using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CBaseController : MonoBehaviour
{
    protected CBaseCharacter Character;
    
    public void Init(CBaseCharacter InCharacter)
    {
        Character = InCharacter;
    }
}
