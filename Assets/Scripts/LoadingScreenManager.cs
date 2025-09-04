using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject loadingPanelPrefab;
    public GameObject loadingPanelInstance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsyncWithLoadingPanelWithName(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutineByName(sceneName));
    }

    public void LoadSceneAsyncWithLoadingPanelWithIndex(int sceneIndex)
    {
        StartCoroutine(LoadSceneCoroutineByIndex(sceneIndex));
    }

    private IEnumerator LoadSceneCoroutineByName(string sceneName)
    {
        if (loadingPanelPrefab != null)
        {
            loadingPanelInstance = Instantiate(loadingPanelPrefab);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private IEnumerator LoadSceneCoroutineByIndex(int sceneIndex)
    {
        if (loadingPanelPrefab != null)
        {
            loadingPanelInstance = Instantiate(loadingPanelPrefab);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void RemoveLoadingPanel()
    {
        if (loadingPanelInstance != null)
        {
            Destroy(loadingPanelInstance);
            loadingPanelInstance = null;
        }
    }
}
