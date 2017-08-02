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

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    public Text helpText;
    public Transform indexT;
    public Transform subTitleT;

    public void Init()
    {
        helpText.text = "";
        foreach (int id in dGameDataCollection.dHelpCollection.dHelpDic.Keys)
        {
            DHelp dHelp = dGameDataCollection.dHelpCollection.dHelpDic[id];

            GameObject subTileItemObj = GameObject.Instantiate<GameObject>(subTitleT.gameObject);
            subTileItemObj.transform.FindChild("Text").GetComponent<Text>().text = dHelp.title;
            subTileItemObj.transform.SetParent(indexT);
            subTileItemObj.transform.localPosition = Vector3.zero;
            subTileItemObj.transform.localScale = Vector3.one;
            subTileItemObj.transform.localRotation = Quaternion.identity;
            subTileItemObj.SetActive(true);

            subTileItemObj.GetComponent<Toggle>().onValueChanged.AddListener((ison) => {
                if (ison)
                {
                    helpText.text = dHelp.content;
                }
            });
        }
    }

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
