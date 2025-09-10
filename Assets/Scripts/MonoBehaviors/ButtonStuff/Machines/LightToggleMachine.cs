using UnityEngine;

public class LightToggleMachine : ToggleMachine
{
    Renderer meshRenderer;
    protected override void Start()
    {
        base.Start();
        meshRenderer = GetComponent<Renderer>();
    }
    protected override void TriggerDisaster()
    {
        base.TriggerDisaster();

    }
}
