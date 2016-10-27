using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    [SerializeField]
    private Button _MainMenuQuit, _MainMenuTutorial, _MainMenuStart, _LeapBugButton;
    [SerializeField]
    private Text _LeapBugText;
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
        SetMainMenuVisibility(false);
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
    }

    void OnClickStart()
    {
        gameObject.SetActive(false);
        _HUD.gameObject.SetActive(true);
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
        _LeapBugText.gameObject.SetActive(!boolean);
    }
}
