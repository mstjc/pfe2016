using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MainMenuActions : MonoBehaviour {

    [SerializeField]
    private Button _MainMenuQuit, _MainMenuTutorial, _MainMenuStart, _LeapBugButton, _ExitButton;
    [SerializeField]
    private GameManager _GM;
    [SerializeField]
    private PlayerHUD _HUD;

    void Awake()
    {
        _MainMenuQuit.onClick.AddListener(OnClickQuit);
        _MainMenuTutorial.onClick.AddListener(OnClickTutorial);
        _MainMenuStart.onClick.AddListener(OnClickStart);
        _LeapBugButton.onClick.AddListener(OnClickLeapBug);
        _ExitButton.onClick.AddListener(OnClickExitGame);
        _ExitButton.gameObject.SetActive(false);
        SetMainMenuVisibility(false);
    }

    private void OnClickExitGame()
    {
        GameManager.Reset();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickQuit()
    {
        Application.Quit();
    }

    void OnClickTutorial()
    {
        gameObject.SetActive(false);
        _HUD.gameObject.SetActive(true);
        _ExitButton.gameObject.SetActive(true);
    }

    void OnClickStart()
    {
        gameObject.SetActive(false);
        _HUD.gameObject.SetActive(true);
        _ExitButton.gameObject.SetActive(true);
        _GM.BeginGame();
    }

    void OnClickLeapBug()
    {
        SetMainMenuVisibility(true);
    }

    void SetMainMenuVisibility(bool boolean)
    {
        _MainMenuQuit.gameObject.SetActive(boolean);
        _MainMenuStart.gameObject.SetActive(boolean);
        _MainMenuTutorial.gameObject.SetActive(boolean);
        _LeapBugButton.gameObject.SetActive(!boolean);
    }
}
