using UnityEngine;

public class TextButton : MonoBehaviour, IInteractable
{
    [SerializeField] private AK.Wwise.Event ButtonPress;
    
    KeypadHolder keypadHolder;
    string key;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        keypadHolder = GetComponentInParent<KeypadHolder>();
    }
    public void Interact()
    {
        keypadHolder.AddCharacter(key);
        animator.Play("Activated", -1, 0);
        if (ButtonPress !=  null) ButtonPress.Post(gameObject);
    }

    public void SetKeyNum(string key)
    {
        this.key = key;
    }
}
