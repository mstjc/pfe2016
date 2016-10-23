using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    [SerializeField]
    private Button _MainMenuQuit, _MainMenuTutorial, _MainMenuStart;
    [SerializeField]
    private GameManager _GM;
    [SerializeField]
    private PlayerHUD _HUD;

    // Use this for initialization
    void Start()
    {
        _MainMenuQuit.onClick.AddListener(OnClickQuit);
        _MainMenuTutorial.onClick.AddListener(OnClickTutorial);
        _MainMenuStart.onClick.AddListener(OnClickStart);
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
}
