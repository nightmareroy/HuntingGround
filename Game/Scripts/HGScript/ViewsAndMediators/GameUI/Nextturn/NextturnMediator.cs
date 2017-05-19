using System;
using System.Collections;
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

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get; set; }

    [Inject]
    public UpdateNextturnTimeSignal updateNextturnTimeSignal { get; set; }

    int nextturn_time = 0;

    public override void OnRegister()
    {
        nextturnView.nextturndelegate += OnNextturnClicked;
        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);
        updateNextturnTimeSignal.AddListener(OnUpdateNextturnTimeSignal);

        TimeSpan now_time_span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        double now_time_stamp = now_time_span.TotalSeconds;
        nextturn_time = (int)(gameInfo.nexttime / 1000 - now_time_stamp);

        //nextturnView.update_time(nextturn_time);
        StartCoroutine(UpdateTime());
    }

    public void OnNextturnClicked()
    {
        nextturnSignal.Dispatch(OnNextturnCallback);
        nextturnView.setBtnInteractable(false);
    }

    public void OnNextturnCallback(bool result)
    {
//        Debug.Log(result);
    }

    public void OnActionAnimStartSignal()
    {
        nextturnView.setBtnInteractable(false);
    }

    public void OnActionAnimFinishSignal()
    {
        if (gameInfo.allplayers_dic[sPlayerInfo.uid].direction_turn != gameInfo.current_turn)
        {
            nextturnView.setBtnInteractable(true);
        }
    }

    public void OnUpdateDirectionTurnSignal(int uid)
    {
        nextturnView.setBtnInteractable(true);
    }

    void OnUpdateNextturnTimeSignal()
    {
        TimeSpan now_time_span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        double now_time_stamp = now_time_span.TotalSeconds;
        nextturn_time = (int)(gameInfo.nexttime / 1000 - now_time_stamp);
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            if (nextturn_time > 0)
            {
                nextturn_time--;
            }
            nextturnView.update_time(nextturn_time);
            yield return new WaitForSeconds(1);
        }
    }

    public void OnDestroy()
    {
        nextturnView.nextturndelegate -= OnNextturnClicked;
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
        updateNextturnTimeSignal.RemoveListener(OnUpdateNextturnTimeSignal);
    }
}

