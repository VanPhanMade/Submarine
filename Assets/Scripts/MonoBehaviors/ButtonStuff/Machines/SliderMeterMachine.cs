using UnityEngine;
using UnityEngine.UI;

public class SliderMeterMachine : MeteredMachine
{
    [SerializeField] Slider visualMeter;
    protected override void Start()
    {
        base.Start();
        
        
    }

    protected override void Update()
    {
        base.Update();
        CurrentMeter -= Time.deltaTime;
        visualMeter.value = currentMeter/maxMeter;
    }


}
