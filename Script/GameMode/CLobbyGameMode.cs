using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CLobbyGameMode : MonoBehaviour
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
        Button FindButton = GameObject.Find("Button_0").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButton0());
        FindButton = GameObject.Find("Button_1").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButton1());
        FindButton = GameObject.Find("Button_2").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButton2());
        FindButton = GameObject.Find("Button_3").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnButton3());

    }

    public void OnButton0()
    {
        CBattleGameMode.PlayerName = "SwordMan";
        SceneManager.LoadScene("Battle");
    }
    public void OnButton1()
    {
        CBattleGameMode.PlayerName = "Archer";
        SceneManager.LoadScene("Battle");
    }
    public void OnButton2()
    {
        CBattleGameMode.PlayerName = "Wizard";
        SceneManager.LoadScene("Battle");
    }
    public void OnButton3()
    {
        CBattleGameMode.PlayerName = "Healer";
        SceneManager.LoadScene("Battle");
    }
    
}
