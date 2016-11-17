using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FrontPanelActions : MonoBehaviour {

    [SerializeField]
    private Button _MainMenuQuit, _MainMenuTutorial, _MainMenuStart, _LeapBugButton, _ExitButton;
    [SerializeField]
    private RectTransform _FrontRightPanel, _FrontLeftPanel, _GameTitle, _TutorialBriefing;
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
        SetTutorialVisibility(false);
        _LeapBugButton.gameObject.SetActive(true);
    }

    private void OnClickExitGame()
    {
        GameManager.Reset();
    }

    void OnClickQuit()
    {
        Application.Quit();
    }

    void OnClickTutorial()
    {
        SetMainMenuVisibility(false);
        SetTutorialVisibility(true);
        _HUD.gameObject.SetActive(true);
        _ExitButton.gameObject.SetActive(true);
        _GM.BeginTutorial();
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
        _LeapBugButton.gameObject.SetActive(false);
    }

    void SetMainMenuVisibility(bool boolean)
    {
        _MainMenuQuit.gameObject.SetActive(boolean);
        _MainMenuStart.gameObject.SetActive(boolean);
        _MainMenuTutorial.gameObject.SetActive(boolean);
        _GameTitle.gameObject.SetActive(boolean);
    }

    void SetTutorialVisibility(bool boolean)
    {
        _TutorialBriefing.gameObject.SetActive(boolean);
        _FrontLeftPanel.gameObject.SetActive(boolean);
        _FrontRightPanel.gameObject.SetActive(boolean);
    }
}
