using UnityEngine;
using UnityEngine.Events;

public abstract class Machine : MonoBehaviour
{
    public UnityEvent disasterHappening;
    public UnityEvent disasterSolved;
    protected bool ongoingDisaster = false;
    [SerializeField] protected int chanceToGoWrong;
    protected virtual void Start()
    {
        InvokeRepeating("TryDisaster", 5, 5);
    }
    protected virtual void Update()
    {
        if (ongoingDisaster)
            GameManager.Instance.SubTakeDamage(1 * Time.deltaTime);
    }

    protected virtual void TryDisaster()
    {
        if (ongoingDisaster)
            return;
        if (chanceToGoWrong > Random.Range(0, 100))
        {
            TriggerDisaster();
        }
    }

    protected virtual void TriggerDisaster()
    {
        ongoingDisaster = true;
        disasterHappening?.Invoke();
    }

    protected virtual void TriggerSafe()
    {
        ongoingDisaster = false;
        disasterSolved?.Invoke();
    }

}
