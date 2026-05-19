using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private ICoroutineRunner _coroutineRunner;

    public SceneLoader(ICoroutineRunner coroutineRunner) =>
        _coroutineRunner = coroutineRunner;

    public void Load(string name, LoadingCurtain curtain = null, Action onLoaded = null) =>
        _coroutineRunner.StartCoroutine(LoadScene(name, curtain, onLoaded));

    public IEnumerator LoadScene(string name, LoadingCurtain curtain, Action onLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == name)
        {
            onLoaded?.Invoke();
            yield break;
        }

        if (curtain != null)
        {
            curtain.Show();
            curtain.SetProgress(0);
        }

        AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

        while (!waitNextScene.isDone)
        {
            if (curtain != null)
            {
                float progress = Mathf.Clamp01(waitNextScene.progress / 0.9f);
                curtain.SetProgress(progress);
            }

            yield return null;
        }

        if (curtain != null)        
            curtain.SetProgress(1f);        

        onLoaded?.Invoke();
    }
}