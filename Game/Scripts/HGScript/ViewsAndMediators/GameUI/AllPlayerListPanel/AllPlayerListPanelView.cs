using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.mediation.impl;

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

}
