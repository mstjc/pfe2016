using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int _numStage = 5;
    [SerializeField]
    private int[] _foesPerStage;
    [SerializeField]
    private float[] _foesSpawnWait;
    [SerializeField]
    private float _StartOfStageWaitTime = 2f;

    private int _currentStage;
    private WaitForSeconds _timeBetweenSpawn;
    private WaitForSeconds _startOfStageWait;

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

        if( (stage + 1) == _numStage)
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
        _timeBetweenSpawn = new WaitForSeconds(_foesSpawnWait[stage]);
        yield return _startOfStageWait;
    }


    private IEnumerator StagePlaying(int stage)
    {
        yield return null;
    }


    private IEnumerator StageEnding(int stage)
    {
        yield return null;
    }
}
