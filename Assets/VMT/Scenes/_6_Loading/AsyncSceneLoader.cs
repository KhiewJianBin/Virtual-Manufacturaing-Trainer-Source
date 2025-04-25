using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    public static string NextSceneToLoad = "";

    public GameObject LoadingText;
    public Image LoadingProgressImg;
    public Image LoadingSceneBG;

    void Start()
    {
        LoadingProgressImg.fillAmount = 0;

        StartCoroutine(LoadScene(NextSceneToLoad));
    }

    IEnumerator LoadScene(string sceneNameToLoad)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);

        //endless loop untl asyncOperation / loading of next scene is done loading
        while (!asyncOperation.isDone)
        {
            //change the loadingprogress img to reflect the progress
            LoadingProgressImg.fillAmount = Mathf.Lerp(LoadingProgressImg.fillAmount, asyncOperation.progress * 1.111f, Time.deltaTime * 15);
            yield return null;
        }
        StartCoroutine(FadeOutBG());
    }
    IEnumerator FadeOutBG()
    {
        //Destory objects
        DestroyImmediate(LoadingText);
        DestroyImmediate(LoadingProgressImg.gameObject);
        
        //change the blackground color transparency / alpha
        while (LoadingSceneBG.color.a >= 0)
        {
            LoadingSceneBG.color = new Color(0, 0, 0, LoadingSceneBG.color.a - 2 * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
