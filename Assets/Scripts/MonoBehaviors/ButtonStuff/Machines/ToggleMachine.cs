using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleMachine : Machine
{

    bool isSet = true;
    public void TriggerChange(bool value)
    {
        IsSet = value;
    }

    public bool IsSet
    {
        get { return isSet; }
        set
        {
            bool temp = isSet;
            isSet = value;
            if (!temp)
                TriggerSafe();
        }
    }

}
