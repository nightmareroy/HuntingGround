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

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

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
                case "winner_groups":
                    rootListT = winnerListT;
                    break;
                case "loser_groups":
                    rootListT = loserListT;
                    break;
            }

            JsonArray listJA = resultJO[key] as JsonArray;
            foreach(string groupidStr in listJA)
            {
                int group_id=int.Parse(groupidStr);


                foreach (int uid in gameInfo.allplayers_dic.Keys)
                {
                    PlayerInfo playerInfo=gameInfo.allplayers_dic[uid];

                    if (playerInfo.group_id == group_id)
                    {
                        GameObject playerItem = GameObject.Instantiate<GameObject>(playerItemTplT.gameObject);
                        playerItem.transform.SetParent(rootListT);
                        playerItem.SetActive(true);
                        playerItem.transform.localPosition = Vector3.zero;
                        playerItem.transform.localScale = Vector3.one;
                        playerItem.transform.localRotation = Quaternion.identity;

                        Text name = playerItem.transform.FindChild("Name").GetComponent<Text>();
                        Text weight = playerItem.transform.FindChild("Weight").GetComponent<Text>();

                        name.text = playerInfo.name;
                        weight.text = (data["all_weight"] as JsonObject)[playerInfo.uid.ToString()].ToString();
                    }
                }

                

            }
        }
    }

    public void OnReturnClick()
    {
        netService.Request(NetService.getUser, null, (msg) => {
            SPlayerInfo sPlayerInfo_temp = new SPlayerInfo();
            sPlayerInfo_temp = SimpleJson.SimpleJson.DeserializeObject<SPlayerInfo>(msg.data.ToString());
            sPlayerInfo.UpdateSplayer(sPlayerInfo_temp);

            asyncSceneService.LoadScene("Login", () =>
            {

            });
        });
        
    }
}
