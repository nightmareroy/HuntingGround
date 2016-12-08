using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

public class LoadingMediator:Mediator
{
    [Inject]
    public LoadingView loadingView {get;set;}

    [Inject]
    public LoadingSignal loadingSignal { get; set; }

    Canvas canvas;

    public override void OnRegister()
    {
        //base.OnRegister();
        //Debug.Log("register");
        loadingSignal.AddListener(OnLoadingSignal);
        canvas = GetComponent<Canvas>();
    }

    void OnLoadingSignal(bool enable)
    {
        //gameObject.SetActive(enable);
        canvas.enabled = enable;
    }

    //public override void OnRemove()
    //{
    //    //base.OnRemove();
    //    loadingSignal.RemoveListener(OnLoadingSignal);
    //}

    public void OnDestroy()
    {
        loadingSignal.RemoveListener(OnLoadingSignal);
    }

}

