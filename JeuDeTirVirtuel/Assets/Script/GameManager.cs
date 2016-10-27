using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int _numStage = 1;
    [SerializeField]
    private int[] _foesPerStage = new int[] { 10 };
    [SerializeField]
    private float[] _foesSpawnWait = new float[] { 3.0f };
    [SerializeField]
    private float _StartOfStageWaitTime = 2f;
    [SerializeField]
    private GameObject _Player;
    [SerializeField]
    private GameObject[] _Aliens;

    private WaitForSeconds _timeBetweenSpawn;
    private WaitForSeconds _startOfStageWait;

    public void BeginGame()
    {
        // Start of the game
        StartCoroutine(StageLoop(0));
    }

    public void AbortGame()
    {

    }

    // Use this for initialization
    void Start () {
        _startOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
    }
	
	// Update is called once per frame
	void Update () {
	}

    private IEnumerator StageLoop(int stage)
    {
        yield return StartCoroutine(StageStarting(stage));
        yield return StartCoroutine(StagePlaying(stage));
        yield return StartCoroutine(StageEnding(stage));

        if ((stage + 1) == _numStage)
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
        _startOfStageWait = new WaitForSeconds(_StartOfStageWaitTime);
        yield return _startOfStageWait;
    }


    private IEnumerator StagePlaying(int stage)
    {
        if (_foesPerStage.Length == 0)
        {
            Debug.Log("No foes per stage..");
        }
        else
        {
            int foesForStage = _foesPerStage[stage];

            if (foesForStage > 0)
            {
                InstantiateEnnemy();
            }

            for (int curFoes = 1; curFoes < foesForStage; curFoes++)
            {
                _timeBetweenSpawn = new WaitForSeconds(_foesSpawnWait[stage]);
                yield return _timeBetweenSpawn;
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

        if(alienScript != null)
        {
            alienScript._Target = _Player;
        }
    }

    public static void Reset()
    {

    }
}
