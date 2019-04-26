using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CLoginGameMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitUIKeyEvent();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitUIKeyEvent()
    {
        Button FindButton = GameObject.Find("Button").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButton());
    }

    public void OnButton()
    {
        SceneManager.LoadScene("Lobby");
    }
}
