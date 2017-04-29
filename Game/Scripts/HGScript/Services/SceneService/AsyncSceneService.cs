using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncSceneService
{
    [Inject]
    public BootstrapView bootstrapView { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    public void LoadScene(string sceneName,Action callback)
    {
        resourceService.ClearAllPool();

        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        bootstrapView.StartCoroutine(ienumerator(asyncOperation,callback));
    }

    IEnumerator ienumerator(YieldInstruction yieldInstruction,Action callback)
    {
        yield return yieldInstruction;
        callback();
    }
}

