using UnityEngine;

public class DebrisButton : MonoBehaviour
{
    // Wwise Variables
    [SerializeField] private AK.Wwise.Event ClearEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.SubTakeDamage(0.3f * Time.deltaTime);
    }

    public void Clicked()
    {
        ClearEvent.Post(gameObject);
        Destroy(gameObject);
    }
}
