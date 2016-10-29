using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int _NumStage = 1;
    [SerializeField]
    private int[] _FoesPerStage = new int[] { 10 };
    [SerializeField]
    private float[] _FoesSpawnWait = new float[] { 3.0f };
    [SerializeField]
    private float _StartOfStageWaitTime = 2f;
    [SerializeField]
    private GameObject _Player;
    [SerializeField]
    private GameObject[] _Aliens;
    [SerializeField]
    private HUDUpdating _HUD;


    private WaitForSeconds _TimeBetweenSpawn;
    private WaitForSeconds _StartOfStageWait;
    private int _CurrentNumberOfEnnemis;
    private int _CurrentStage = 1;

    public void BeginGame()
    {
        // Start of the game
        StartCoroutine(StageLoop(_CurrentStage));
    }

    // Use this for initialization
    void Start() {
        _StartOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
    }

    // Update is called once per frame
    void Update() {
    }

    private IEnumerator StageLoop(int stage)
    {
        yield return StartCoroutine(StageStarting(stage));
        yield return StartCoroutine(StagePlaying(stage));
        yield return StartCoroutine(StageEnding(stage));

        if (_CurrentStage == _NumStage)
        {
            //Last Stage completed
        }
        else
        {
            StageLoop(++stage);
        }
    }


    private IEnumerator StageStarting(int stage)
    {
        _CurrentStage = stage;
        _StartOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
        _HUD.UpdateStage(_CurrentStage);

        yield return _StartOfStageWait;
    }


    private IEnumerator StagePlaying(int stage)
    {
        if (_FoesPerStage.Length == 0)
        {
            Debug.Log("No foes per stage..");
        }
        else
        {
            int foesForStage = _FoesPerStage[stage];

            if (foesForStage > 0)
            {
                InstantiateEnnemy();
            }

            for (int curFoes = 1; curFoes < foesForStage; curFoes++)
            {
                _TimeBetweenSpawn = new WaitForSeconds(_FoesSpawnWait[stage]);
                yield return _TimeBetweenSpawn;
                InstantiateEnnemy();
            }
        }
    }


    private IEnumerator StageEnding(int stage)
    {
        yield return null;
    }

    private void InstantiateEnnemy()
    {
        var radAngleRange = 30.0f * Mathf.Deg2Rad;
        var radHorizon = 180.0f * Mathf.Deg2Rad;
        var angle = Random.Range(-radAngleRange, radAngleRange + radHorizon);
        var x = 40 * Mathf.Cos(angle);
        var z = 40 * Mathf.Sin(angle);

        var alienIndex = Random.Range(0, _Aliens.Length);
        var alien = Instantiate(_Aliens[alienIndex], new Vector3(x, 0, z), Quaternion.identity) as GameObject;
        var alienScript = alien.GetComponent(typeof(MonsterManager)) as MonsterManager;

        if (alienScript != null)
        {
            alienScript._Target = _Player;
        }

        ++_CurrentNumberOfEnnemis;
        _HUD.UpdateEnnemis(_CurrentNumberOfEnnemis);
    }

    public void EnnemiDied()
    {
        --_CurrentNumberOfEnnemis;
        _HUD.UpdateEnnemis(_CurrentNumberOfEnnemis);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
