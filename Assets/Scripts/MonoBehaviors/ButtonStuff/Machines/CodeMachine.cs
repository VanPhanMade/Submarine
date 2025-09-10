using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CodeMachine : Machine
{
    // Wwise Fields
    [SerializeField] private AK.Wwise.Event SuccessEvent;
    [SerializeField] private AK.Wwise.Event FailEvent;

    string passcode;
    string statusText;
    [SerializeField] TextMeshProUGUI statusTextMesh;
    [SerializeField] TextMeshProUGUI passcodeTextMesh;
    protected override void Start()
    {
        base.Start();
        statusText = "System OK";
        statusTextMesh.gameObject.GetComponent<TextUpdater>().Enabled += UpdateText;
    }
    public void AttemptSolve(string passcode)
    {
        Debug.Log(passcode);
        if (passcode != this.passcode)
        {
            FailEvent.Post(gameObject);
            return;
        }
        else
            TriggerSafe();
    }
    protected override void TriggerSafe()
    {
        base.TriggerSafe();
        SuccessEvent.Post(gameObject);
        statusText = "System OK";
        passcode = string.Empty;
        UpdateText();

    }

    protected override void TriggerDisaster()
    {
        base.TriggerDisaster();
        statusText = "ERROR DETECTED";
        passcode = GenerateRandomCode();
        UpdateText();


    }

    string GenerateRandomCode()
    {
        int code = 0;
        for (int i = 0; i < 4; i++)
        {
            code += (int)Mathf.Pow(10,i) * Random.Range(0, 10);
        }
        return code.ToString();
    }

    void UpdateText()
    {
        statusTextMesh.text = statusText;
        passcodeTextMesh.text = passcode;
    }
}
