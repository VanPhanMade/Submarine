using UnityEngine;
using UnityEngine.Events;

public class ButtonMeterController : MonoBehaviour, IInteractable
{
    
    public UnityEvent TriggerChange;
    bool canInteract = true;
    Animator animator;

    [SerializeField] float interactionCooldown = 1f;

    Timer cooldown;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        cooldown = new Timer(interactionCooldown);
        cooldown.OnTimerStop += ResetCooldown;
    }
    public void Interact()
    {
        if (!canInteract)
        {
            return;
        }
        TriggerChange?.Invoke();
        animator?.Play("Activated", -1, 0);
        canInteract = false;
        cooldown.Start();
        
    }

    void ResetCooldown()
    {
        canInteract = true;
    }
    private void Update()
    {
        cooldown.Tick(Time.deltaTime);
    }

    public void Test()
    {
        Debug.Log("Working");
    }


}
