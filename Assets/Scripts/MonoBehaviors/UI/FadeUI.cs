using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    Image fadeImage;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject defeatText;
    bool won;
    [SerializeField] string sceneToReload;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
        fadeImage = GetComponent<Image>();
        StartCoroutine(FadeToBlack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetRightText(bool _won)
    {
        won = _won;
    }

    IEnumerator FadeToBlack()
    {
        float a = 0;
        for(int i = 0; i < 60 ; i++)
        {
            a += (1f / 60f);
            Debug.Log(a);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return new WaitForSeconds(0.05f);
        }
        if(won)
            winText.SetActive(true);
        else
            defeatText.SetActive(true);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneToReload);
    }

    
}
