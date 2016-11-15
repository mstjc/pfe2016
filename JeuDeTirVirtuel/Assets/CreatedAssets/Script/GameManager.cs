using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int _NumStage = 1;
    [SerializeField]
    private int[] _FoesPerStage = new int[] { 10 };
    [SerializeField]
    private float[] _FoesSpawnWait = new float[] { 3.0f };
    [SerializeField]
    private int[] _TypeOfEnnemisSpawning;
    [SerializeField]
    private float _FOVAngleDeg = 180F;
    [SerializeField]
    private float _StartOfStageWaitTime = 2f;
    [SerializeField]
    private GameObject _Player;
    [SerializeField]
    private GameObject[] _Aliens;
    [SerializeField]
    private HUDUpdating _HUDUpdating;
    [SerializeField]
    private DetectorEnablingScript _RightHandDetectors;
    [SerializeField]
    private DetectorEnablingScript _LeftHandDetectors;

    private int _CurrentStage = 0;
    private int _EnnemiesRemaining = 0;
    private PlayerHealth _PlayerHealth;

    private WaitForSeconds _TimeBetweenSpawn;
    private WaitForSeconds _StartOfStageWait;

    public void BeginGame()
    {
        // Start of the game
        StartCoroutine(StageLoop(0));
        _RightHandDetectors.EnableDetectors();
        _LeftHandDetectors.EnableDetectors();
    }

    public void BeginTutorial()
    {
        // Start of the tutorial
        _RightHandDetectors.EnableDetectors();
        _LeftHandDetectors.EnableDetectors();
    }

    public void AbortGame()
    {

    }

    // Use this for initialization
    void Start () {
        _StartOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
        _PlayerHealth = _Player.GetComponent<PlayerHealth>();
        _RightHandDetectors.DisableDetectors();
        _LeftHandDetectors.DisableDetectors();
    }

    private IEnumerator StageLoop(int stage)
    {
        _CurrentStage = stage + 1;
        _HUDUpdating.UpdateStage(_CurrentStage);
        //_HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);


        yield return StartCoroutine(StageStarting(stage));
        yield return StartCoroutine(StagePlaying(stage));
        yield return StartCoroutine(StageEnding(stage));

        if ((stage + 1) == _NumStage)
        {
            //Last Stage completed
        }
        else
        {
            StartCoroutine(StageLoop(++stage));
        }
    }


    private IEnumerator StageStarting(int stage)
    {
        _StartOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
        _TimeBetweenSpawn = new WaitForSeconds(_FoesSpawnWait[stage]);
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

            _EnnemiesRemaining = foesForStage;
            _HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);

            if (foesForStage > 0)
            {
                InstantiateEnnemy();
            }

            for (int curFoes = 1; curFoes < foesForStage; curFoes++)
            {
                yield return _TimeBetweenSpawn;
                InstantiateEnnemy();
            }
        }
    }


    private IEnumerator StageEnding(int stage)
    {
        while(_EnnemiesRemaining > 0)
        {
            // waiting for all enemies to be defeated.
            yield return null;
        }
        _PlayerHealth.RefillHealth();
    }

    private void InstantiateEnnemy()
    {
        float horizon = 180F;
        var radAngleRange = ((_FOVAngleDeg - horizon) /2) * Mathf.Deg2Rad;
        var radHorizon = horizon * Mathf.Deg2Rad;
        var angle = Random.Range(-radAngleRange, radAngleRange + radHorizon);
        var x = 40 * Mathf.Cos(angle);
        var z = 40 * Mathf.Sin(angle);

        var alienIndex = Random.Range(0, _TypeOfEnnemisSpawning[_CurrentStage]);
        var alien = Instantiate(_Aliens[alienIndex], new Vector3(x, 0, z), Quaternion.identity) as GameObject;
        var alienScript = alien.GetComponent(typeof(MonsterManager)) as MonsterManager;

        if(alienScript != null)
        {
            alienScript.Died += OnAlienDead;
            alienScript._Target = _Player;
        }
    }

    private void OnAlienDead(object sender, EventArgs e)
    {
        MonsterManager script = sender as MonsterManager;
        if(script != null)
        {
            script.Died -= OnAlienDead;
            --_EnnemiesRemaining;
            _HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);
        }
    }

    public static void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
