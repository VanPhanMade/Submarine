
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("WHERE IS IT");

            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
    }

    float timeRemaining;
    [SerializeField] float maxTime;
    [SerializeField] float submarineHealth;
    [SerializeField] float submarineMaxHealth;

    [Header("UI References")]
    [SerializeField] Slider submarineHealthUI;
    [SerializeField] TextMeshProUGUI timeRemainingText;
    [SerializeField] GameObject blackScreen;

    // Wwise Variables
    [SerializeField] private AK.Wwise.Event backgroundHum;

    private float hitTime;
    [SerializeField] private AK.Wwise.Event hitEvent;
    private bool subHit;

    private void Start()
    {
        timeRemaining = maxTime;

        // Audio Variables
        hitTime = UnityEngine.Random.value * maxTime;
        Debug.Log(hitTime);
        subHit = false;

        backgroundHum.Post(gameObject);
    }

    public void Update()
    {
        if (timeRemaining > 0 && submarineHealth > 0)
            timeRemaining -= Time.deltaTime;
        timeRemainingText.text = CreateTimeText();

        // Environment Audio
        if (!subHit && timeRemaining < hitTime)
        {
            hitEvent.Post(gameObject);
            subHit = true;
        }
            

        if (timeRemaining <= 0)
            Victory();
            
    }
    string CreateTimeText()
    {
        string minute = ((int) timeRemaining / 60).ToString();
        int remainder = (int)timeRemaining % 60;
        string seconds = remainder < 10 ? "0" + (remainder).ToString() : (remainder).ToString();
        return minute + ":" + seconds;
    }



    public void SubTakeDamage(float damage)
    {
        submarineHealth -= damage;
        submarineHealthUI.value = submarineHealth / submarineMaxHealth;

        if(submarineHealth <= 0)
        {
            blackScreen.SetActive(true);
            blackScreen.GetComponent<FadeUI>().GetRightText(false);
        }

    }

    void Victory()
    {
        blackScreen.SetActive(true);
        blackScreen.GetComponent<FadeUI>().GetRightText(true);
    }
}
