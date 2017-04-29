using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using SimpleJson;
using UnityEngine;

public class GameoverMediator:Mediator
{
    [Inject]
    public GameoverView gameoverView { get; set; }

    [Inject]
    public GameoverSignal gameoverSignal { get; set; }

    public override void OnRegister()
    {
        gameoverSignal.AddListener(OnGameoverSignal);
    }

    void OnGameoverSignal(JsonObject data)
    {
        gameoverView.SetData(data);
    }

    void OnDestroy()
    {

        gameoverSignal.RemoveListener(OnGameoverSignal);
    }
}
