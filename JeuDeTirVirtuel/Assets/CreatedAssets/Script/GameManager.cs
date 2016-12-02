using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

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
    private GameObject _LastBoss;
    [SerializeField]
    private GameObject[] _LastBossColumn;
    [SerializeField]
    private GameObject _LastBossForcefield;
    [SerializeField]
    private HUDUpdating _HUDUpdating;
    [SerializeField]
    private RectTransform _CongratulationText;
    [SerializeField]
    private DetectorEnablingScript _RightHandDetectors;
    [SerializeField]
    private DetectorEnablingScript _LeftHandDetectors;

    public AudioMixerSnapshot mainMixer;
    public AudioMixerSnapshot gameMixer;

    private int _CurrentStage = 0;
    private int _EnnemiesRemaining = 0;
    private PlayerHealth _PlayerHealth;
    private float _CongratulationTimeFloat = 5f;

    private WaitForSeconds _CongratulationTime;
    private WaitForSeconds _TimeBetweenSpawn;
    private WaitForSeconds _StartOfStageWait;

    public void BeginGame()
    {
        // Start of the game
        StartCoroutine(StageLoop(0));
        _RightHandDetectors.EnableDetectors();
        _LeftHandDetectors.EnableDetectors();
        PlayGameMusic();
    }

    public void BeginTutorial()
    {
        // Start of the tutorial
        _RightHandDetectors.EnableDetectors();
        _LeftHandDetectors.EnableDetectors();
    }

    public void AbortGame()
    {
        PlayMainMusic();
    }

    // Use this for initialization
    void Start () {
        _StartOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
        _CongratulationTime = new WaitForSeconds(_CongratulationTimeFloat);
        _PlayerHealth = _Player.GetComponent<PlayerHealth>();
        _RightHandDetectors.DisableDetectors();
        _LeftHandDetectors.DisableDetectors();
        PlayMainMusic();
    }
   
    private IEnumerator StageLoop(int stage)
    {
        _CurrentStage = stage;
        _HUDUpdating.UpdateStage(_CurrentStage + 1);
        //_HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);


        yield return StartCoroutine(StageStarting(stage));
        yield return StartCoroutine(StagePlaying(stage));
        yield return StartCoroutine(StageEnding(stage));

        if ((stage + 1) == _NumStage)
        {
            //Last Stage completed
            StartCoroutine(LastBoss());
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
        InstantiateEnnemy(0, _TypeOfEnnemisSpawning[_CurrentStage], true);
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

    private void PlayMainMusic()
    {
        if(mainMixer && gameMixer)
        {
            mainMixer.TransitionTo(0.5f);
        }
    }

    private void PlayGameMusic()
    {
        if (mainMixer && gameMixer)
        {
            gameMixer.TransitionTo(0.5f);
        }
    }

    private IEnumerator LastBoss()
    {
        //Instantiate(_LastBoss, new Vector3(0, 4.5f, 40), Quaternion.AngleAxis(180, new Vector3(0, 1, 0)));
        //Instantiate(_LastBossColumn, new Vector3(-7, 12, 30), Quaternion.identity);
        //Instantiate(_LastBossColumn, new Vector3(7, 12, 30), Quaternion.identity);
        //Instantiate(_LastBossForcefield, new Vector3(0, 12, 30), Quaternion.identity);

        
        var alienScript = _LastBoss.GetComponent(typeof(MonsterManager)) as MonsterManager;

        if (alienScript != null)
        {

            alienScript.Died += OnAlienDead;
            alienScript._Target = _Player;
        }
        _LastBoss.gameObject.SetActive(true);
        _LastBossForcefield.gameObject.SetActive(true);
        for(int i=0; i<_LastBossColumn.Length; i++)
        {
            _LastBossColumn[i].SetActive(true);
        }
        yield return StartCoroutine(LastBossPhase(5, 0));
        yield return StartCoroutine(LastBossPhase(5, 4));
        yield return StartCoroutine(LastBossFinalPhase());
        yield return StartCoroutine(LastBossDefeated());
    }

    private IEnumerator LastBossPhase(float spawnTime, int alienSpawnStartIndex)
    {
        _EnnemiesRemaining = 2;
        _TimeBetweenSpawn = new WaitForSeconds(spawnTime);
        _HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);
        if (_EnnemiesRemaining > 0)
        {
            InstantiateEnnemy(alienSpawnStartIndex, _Aliens.Length, true);
        }

        for (int curFoes = 1; curFoes < _EnnemiesRemaining; curFoes++)
        {
            yield return _TimeBetweenSpawn;
            InstantiateEnnemy(alienSpawnStartIndex, _Aliens.Length, true);
        }
        while (_EnnemiesRemaining > 0)
        {
            // waiting for all enemies to be defeated.
            yield return null;
        }
    }

    private IEnumerator LastBossFinalPhase()
    {
        // Force field off, spawning skeleton only (slower spawn) boss moves in arc
        _EnnemiesRemaining = 1;
        _LastBoss.GetComponent<LastBossMovement>().BossPhaseStarted = true;
        _LastBoss.GetComponent<LastBossShooter>().BossPhaseStarted = true;
        _LastBossForcefield.gameObject.SetActive(false);

        _HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);
        _TimeBetweenSpawn = new WaitForSeconds(5);
        while (_LastBoss != null)
        {
            // waiting for Skrull to be defeated.
            InstantiateEnnemy(4, _Aliens.Length, true);
            ++_EnnemiesRemaining;
            _HUDUpdating.UpdateEnnemiesRemaining(_EnnemiesRemaining);
            yield return _TimeBetweenSpawn;
        }
        while (_EnnemiesRemaining > 0)
        {
            // waiting for all enemies to be defeated.
            yield return null;
        }
    }

    private IEnumerator LastBossDefeated()
    {
        // Display a text congratulating the player for x seconds then reset the game;
        _CongratulationText.gameObject.SetActive(true);
        yield return _CongratulationTime;
        Reset();
    }

    private void InstantiateEnnemy(int startIndex, int endIndex, bool ennemiLifeMatters)
    {
        float horizon = 180F;
        var radAngleRange = ((_FOVAngleDeg - horizon) / 2) * Mathf.Deg2Rad;
        var radHorizon = horizon * Mathf.Deg2Rad;
        float angle = 0;
        float x = 0;
        float z = 0;
        var alienIndex = Random.Range(startIndex, endIndex);
        bool occupiedSpace = true;
		int searchCount = 0;
        while (occupiedSpace && searchCount <= 5)
        {
            angle = Random.Range(-radAngleRange, radAngleRange + radHorizon);
            x = 40 * Mathf.Cos(angle);
            z = 40 * Mathf.Sin(angle);
            Vector3 pos = new Vector3(x, 4, z);
            var hitColliders = Physics.OverlapSphere(pos, 2); // Biggest monster is 1.5x+1z and this is int only so 2
            if (hitColliders.Length <= 1)
            {
                occupiedSpace = false;
            }

			++searchCount;
        }
        var alien = Instantiate(_Aliens[alienIndex], new Vector3(x, 0, z), Quaternion.identity) as GameObject;
        var alienScript = alien.GetComponent(typeof(MonsterManager)) as MonsterManager;

        if (alienScript != null)
        {
            if(ennemiLifeMatters)
            {
                alienScript.Died += OnAlienDead;
            }
            alienScript._Target = _Player;
        }
    }
}
