using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

public class GameoverView:View
{
    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public AsyncSceneService asyncSceneService { get; set; }

    [Inject]
    public UpdateWhenReturnToLogin updateWhenReturnToLogin { get; set; }

    public GameObject rootPanel;

    public Transform playerItemTplT;

    public Transform winnerListT;

    public Transform loserListT;

    public void SetData(JsonObject data)
    {
        rootPanel.SetActive(true);

        JsonObject resultJO = data["result"] as JsonObject;
        foreach (string key in resultJO.Keys)
        {
            Transform rootListT=null;
            switch (key)
            {
                case "winners":
                    rootListT = winnerListT;
                    break;
                case "losers":
                    rootListT = loserListT;
                    break;
            }

            JsonArray listAR = resultJO[key] as JsonArray;
            foreach(string uidStr in listAR)
            {
                int uid=int.Parse(uidStr);

                GameObject playerItem = GameObject.Instantiate<GameObject>(playerItemTplT.gameObject);
                playerItem.transform.SetParent(rootListT);
                playerItem.SetActive(true);
                playerItem.transform.localPosition = Vector3.zero;
                playerItem.transform.localScale = Vector3.one;
                playerItem.transform.localRotation = Quaternion.identity;

                Text name = playerItem.transform.FindChild("Name").GetComponent<Text>();
                Text weight = playerItem.transform.FindChild("Weight").GetComponent<Text>();

                name.text = gameInfo.allplayers_dic[uid].name;
                weight.text = (data["all_weight"] as JsonObject)[uid.ToString()].ToString();

            }
        }
    }

    public void OnReturnClick()
    {

        asyncSceneService.LoadScene("Login", () => {
            //Debug.Log("test2");
            //updateWhenReturnToLogin.Dispatch();
        });
    }
}
