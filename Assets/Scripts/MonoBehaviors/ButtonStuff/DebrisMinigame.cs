using UnityEngine;

public class DebrisMinigame : Machine
{
    [SerializeField] GameObject debrisPrefab;
    RectTransform rectTransform;
    
    protected override void Start()
    {
        base.Start();
        rectTransform = GetComponent<RectTransform>();
        

    }

    protected override void Update()
    {

    }
    protected override void TriggerDisaster()
    {
        base.TriggerDisaster();
        //These are magic numbers I know
        Vector3 randomLoc = new Vector3(Random.Range(-340,340), Random.Range(-245, 10), 0);
        GameObject instantiatedDebris = Instantiate(debrisPrefab, transform, false);
        instantiatedDebris.GetComponent<RectTransform>().localPosition = randomLoc;
    }

    protected override void TryDisaster()
    {
        if (chanceToGoWrong > Random.Range(0, 100))
        {
            TriggerDisaster();
        }
    }
}
