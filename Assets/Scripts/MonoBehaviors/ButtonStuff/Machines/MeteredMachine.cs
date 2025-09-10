using UnityEngine;

public class MeteredMachine : Machine
{
    [SerializeField] protected float currentMeter = 0;
    [SerializeField] protected float maxMeter = 100;
    [SerializeField] protected float dangerThreshold;

    //protected bool inDanger = false;



    public void TriggerChange(float value)
    {
        CurrentMeter += value;
        if(currentMeter > maxMeter)
            currentMeter = maxMeter;
        if(currentMeter < 0)
            currentMeter = 0;
        
    }

    public virtual float CurrentMeter
    {
        get { return currentMeter; }
        set { 
            currentMeter = value;
            if(ongoingDisaster)
            {
                if(currentMeter > dangerThreshold)
                {
                    TriggerSafe();
                }
            }
            else
            {
                if(currentMeter < dangerThreshold && !ongoingDisaster)
                {
                    TriggerDisaster();
                }
            }
        }
    }






    
}
