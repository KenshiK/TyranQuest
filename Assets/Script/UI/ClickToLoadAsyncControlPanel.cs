using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsyncControlPanel : MonoBehaviour
{

    public Slider loadingBar;
    public GameObject loadingImage;
    public GameObject toDestroy;

    private AsyncOperation async;


    public void ClickAsync(int level)
    {
        Destroy(toDestroy);
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }


    IEnumerator LoadLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}