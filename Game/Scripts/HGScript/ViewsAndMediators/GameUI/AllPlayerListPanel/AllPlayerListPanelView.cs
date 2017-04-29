using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.mediation.impl;
using SimpleJson;

public class AllPlayerListPanelView : View
{
    public Transform listT;
    public Transform playerItemTpl;

    [Inject]
    public GameInfo gameInfo{ get; set;}

//    [Inject]
//    public 

    public void Init()
    {
        
    }

    public void UpdatePlayers()
    {
        foreach (int player_id in gameInfo.allplayers_dic.Keys)
        {
            PlayerInfo playerInfo = gameInfo.allplayers_dic[player_id];
            GameObject playerItemObj = GameObject.Instantiate(playerItemTpl.gameObject as Object) as GameObject;

            playerItemObj.name = playerInfo.uid.ToString();
            playerItemObj.transform.FindChild("Name").GetComponent<Text>().text=playerInfo.name;

            playerItemObj.transform.SetParent(listT);
            playerItemObj.transform.localScale = Vector3.one;
            playerItemObj.transform.localPosition = Vector3.zero;
            playerItemObj.transform.localRotation = Quaternion.identity;

            playerItemObj.SetActive(true);



        }


    }

    public void SetPlayerReady(int uid, bool ready)
    {
        Transform playerItemT = listT.FindChild(uid.ToString());
        Transform readyT = playerItemT.FindChild("Ready");
        readyT.gameObject.SetActive(ready);
    }

    public void ResetAllPlayerReady()
    {
        for (int i = 0; i < listT.childCount; i++)
        {
            Transform playerItemT = listT.GetChild(i);
            Transform readyT = playerItemT.FindChild("Ready");
            readyT.gameObject.SetActive(false);
        }
    }

    public void RemovePlayer(int uid)
    {
        Transform playerItemT = listT.FindChild(uid.ToString());
        Tools.Destroy(playerItemT);
    }

    public void UpdateWeights(JsonObject weight_dic)
    {
        foreach (string key in weight_dic.Keys)
        {
            Transform itemT = listT.FindChild(key);
            Text weightText = itemT.FindChild("Weight").GetComponent<Text>();
            weightText.text = weight_dic[key].ToString();

        }
    }

}
