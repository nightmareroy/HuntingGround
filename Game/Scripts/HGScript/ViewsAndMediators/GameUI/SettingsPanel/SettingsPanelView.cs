using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView:View
{
    //public Button returnBtn;
    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public MsgBoxSignal msgBoxSignal { get; set; }

    public void OnReturnLoginClick()
    {
        netService.Request(NetService.LeaveGame, null, (msg) => {
            if (msg.code == 200)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
            }
            else if (msg.code == 500)
            {
                Debug.LogError("exit game err!");
            }
        });
    }
}
