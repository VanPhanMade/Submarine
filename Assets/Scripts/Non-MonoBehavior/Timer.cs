using System;
using UnityEngine;

public class Timer
{
    public Action OnTimerStart = delegate { };
    public Action OnTimerStop = delegate { };
    float initialTime = 0;
    float time = 0;
    bool isRunning;
    public Timer(float initialTime)
    {
        this.initialTime = initialTime;
        isRunning =false;
    }
    public void Tick(float deltaTime)
    {

        if (isRunning && time > 0)
            time -= deltaTime;
        
        if(isRunning && time <= 0)
            Stop();
    }

    public float Progress => time/initialTime;
    public void Pause() => isRunning = false;
    public void Resume () => isRunning = true;
    public void Start()
    {
        time = initialTime;
        if(!isRunning)
        {
            isRunning = true;
            OnTimerStart?.Invoke();
        }
    }
    
    public void Stop()
    {
        isRunning = false;
        OnTimerStop?.Invoke();
    }
    public void Reset() => time = initialTime;
    public bool IsFinished => time <= 0;

}
