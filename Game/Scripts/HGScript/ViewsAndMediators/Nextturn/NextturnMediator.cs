using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using SimpleJson;

public class NextturnMediator:Mediator
{
    [Inject]
    public NextturnView nextturnView { get; set; }

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public NextturnSignal nextturnSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    public override void OnRegister()
    {
        nextturnView.nextturndelegate += OnNextturnClicked;
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
    }

    public void OnNextturnClicked()
    {
        nextturnSignal.Dispatch(OnNextturnCallback);
    }

    public void OnNextturnCallback(bool result)
    {
        Debug.Log(result);
    }

    public void OnActionAnimStartSignal()
    {
        nextturnView.setBtnInteractable(false);
    }

    public void OnActionAnimFinishSignal()
    {
        nextturnView.setBtnInteractable(true);
    }

    public void OnDestroy()
    {
        nextturnView.nextturndelegate -= OnNextturnClicked;
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
    }
}

