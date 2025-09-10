using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RadialMeterMachine : MeteredMachine
{
    float rotX;
    float desiredThreshold;
    int[] possibleNumbers = { 0, 50, 100 };
    [SerializeField] TextMeshProUGUI desiredThresholdText;

    protected override void Start()
    {
        base.Start();
        UpdateUI();
    }
    protected override void TriggerDisaster()
    {
        base.TriggerDisaster();
        int randomNum = Random.Range(0, possibleNumbers.Length);
        desiredThreshold = possibleNumbers[randomNum];
        UpdateUI();
        CheckIfSafe();
    }

    protected override void TriggerSafe()
    {
        base.TriggerSafe();
    }

    protected override void Update()
    {
        base.Update();
        rotX = Mathf.Clamp(Unity.Mathematics.math.remap(0, 100, 0, 66, currentMeter),0, 66);
        transform.localRotation =  Quaternion.Euler(0f, 0f, rotX);
        
        
        
    }
    public override float CurrentMeter 
    {
        get {  return currentMeter; }
        set
        {
            currentMeter = value;
            CheckIfSafe();
        }
    }
    void CheckIfSafe()
    {
        if (!Approximate(currentMeter, desiredThreshold, 3))
        {
            ongoingDisaster = true;
            disasterHappening?.Invoke();
        }
        else
            TriggerSafe();
    }

    //Returns if true if the number is close
    bool Approximate(float number, float comparingNumber, float threshold)
    {
        if (Mathf.Abs(Mathf.Abs(number) - Mathf.Abs(comparingNumber)) < threshold)
            return true;
        return false;
    }

    protected override void TryDisaster()
    {
        base.TryDisaster();

    }

    void UpdateUI()
    {
        switch(desiredThreshold)
        {
            case 0:
                desiredThresholdText.text = "LOW";
                break;
            case 50:
                desiredThresholdText.text = "MED";
                break;
            case 100:
                desiredThresholdText.text = "HIGH";
                break;
            default:
                desiredThresholdText.text = "MED";
                break;
        }
    }
}
