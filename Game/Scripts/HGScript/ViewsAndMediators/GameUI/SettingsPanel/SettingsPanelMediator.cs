using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;

public class SettingsPanelMediator:Mediator
{
    [Inject]
    public SettingsPanelView settingsView { get; set; }

    public override void OnRegister()
    {
        settingsView.Init();
    }
}
