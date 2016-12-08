using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncSceneService
{
    [Inject]
    public BootstrapView bootstrapView { get; set; }

    public void LoadScene(string sceneName,Action callback)
    {
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        bootstrapView.StartCoroutine(ienumerator(asyncOperation,callback));
    }

    IEnumerator ienumerator(YieldInstruction yieldInstruction,Action callback)
    {
        yield return yieldInstruction;
        callback();
    }
}

