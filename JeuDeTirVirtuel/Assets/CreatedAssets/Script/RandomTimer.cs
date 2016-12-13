using UnityEngine;
using System.Collections;

public class RandomTimer {

    public float MinTime = 2.0f;
    public float MaxTime = 4.0f;

    public delegate void EventHandler();
    public event EventHandler OnTimerTick;

    private float _TimeLeft;
    public bool Started = false;

    public RandomTimer(float minTime, float maxTime)
    {
        MinTime = minTime;
        MaxTime = maxTime;
    }

	

	public void Update (float delta) {
        if(Started)
        {
            _TimeLeft -= delta;

            if (_TimeLeft <= 0.0f)
            {
                if(OnTimerTick != null)
                {
                    OnTimerTick();
                    Started = false;
                }
            }
        }
	}

    public void StartTimer()
    {
        _TimeLeft = Random.Range(MinTime, MaxTime);
        Started = true;
    }

    public void StopTimer()
    {
        Started = false;
    }
}
