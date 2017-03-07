using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using SimpleJson;

public class LoginView : View {

    //Button singleBtn;
    //Button multiBtn;
    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}


    //total
    public Action onRegisterClickedDelegate;

    public Action onLoginClickedDelegate;

    public InputField loginAccount;
    public InputField loginPwd;
    public InputField loginYzm;

    public InputField registerAccount;
    public InputField registerPwd;
    public InputField registerPwd2;

    //public Button loadGameBtn;

    //public GameObject mainMenu;
    //
    public GameObject loginMenu;
    public GameObject registerMenu;
    public GameObject gameMenu;
    public GameObject gameTypeMenu;
    public GameObject multiGameSettingMenu;
    public GameObject multiGamesMenu;
    public GameObject singleGameMenu;


    public GameObject multiGameItemTemplate;
    public GameObject multiGameScrollviewList;

    public GameObject multiGameSettingPlayerTemplate;
    public GameObject multiGameSettingGroupTemplate;
    public GameObject multiGameSettingPanel;

    public GameObject gameNamesRoot;
    public GameObject gameNameTpl;

    [HideInInspector]
    public int selectedCreatorId=-1;

    //public GameObject serverItemTpl;

    public enum PlayMode
    {
        single,
        multi
    }
    //public Signal<PlayMode> playModeSignal;

    //single
    public enum MapSize
    {
        small,
        big
    }
    //public Signal<MapSize> singleMapSizeSignal;
    //public Signal singleStartSignal;

    public Action<string> viewClick;
    

    //multi

    //public Button loginBtn;
    //public Button registerBtn;



	// Use this for initialization
    public void Init()
	{

	}


    //login
    public void OnLoginClicked()
    {
        if (viewClick != null)
        {
            viewClick("Login");
        }
    }

    public void OnRegisterClicked()
    {
        if (viewClick != null)
        {
            viewClick("Register");
        }
    }

    //register
    public void OnConfirmRegisterClicked()
    {
        if (viewClick != null)
        {
            viewClick("ConfirmRegister");
        }
    }

    //game
    public void OnCreateSingleGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("CreateSingleGame");
        }
    }

    public void OnMultiGamesClicked()
    {
        if (viewClick != null)
        {
            viewClick("MultiGames");
        }
    }

    public void OnLoadGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("LoadGame");
        }
    }

    public void OnFriendClicked()
    {
        if (viewClick != null)
        {
            viewClick("Friend");
        }
    }

    //multi game
    public void OnCreateMultiGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("CreateMultiGame");
        }
    }

    public void OnJoinGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("JoinGame");
        }
    }

    //game type
    public void On1v1Clicked()
    {
        
        if (viewClick != null)
        {
            viewClick("1v1");
        }
    }

    public void On2v2Clicked()
    {
        
        if (viewClick != null)
        {
            viewClick("2v2");
        }
    }

    //multi game setting
    public void OnStartMultiGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("StartMultiGame");
        }
    }

    public void OnCancelOrLeaveMultiGameClicked()
    {
        if (viewClick != null)
        {
            viewClick("CancelOrLeaveMultiGame");
        }
    }

    //shared
    public void OnReturnClicked()
    {
        if (viewClick != null)
        {
            viewClick("Return");
        }
    }

    public void OnReturnToGamePageClicked()
    {
        if (viewClick != null)
        {
            viewClick("ReturnToGamePage");
        }
    }

//    public void OnReturnGameSetting()
//    {
//        if (viewClick != null)
//        {
//            viewClick("ReturnGameSetting");
//        }
//    }

    //public void SetLoadGameEnable(bool enable)
    //{
    //    loadGameBtn.enabled = enable;
    //}

    //public void enterGameMenu()
    //{
    //    loginMenu.SetActive(false);
    //    gameMenu.SetActive(true);
    //}
    public void InitSingleGameBtns(Action<int> action)
    {
        Tools.ClearChildren(gameNamesRoot.transform);
        int progress_id = 1;
        while (progress_id <= sPlayerInfo.single_game_progress)
        {
            GameObject gameNameObj = GameObject.Instantiate(gameNameTpl as UnityEngine.Object) as GameObject;
            gameNameObj.transform.SetParent(gameNamesRoot.transform);
            gameNameObj.name = progress_id.ToString();

            gameNameObj.transform.FindChild("Text").GetComponent<Text>().text = dGameDataCollection.dSingleGameInfoCollection.dSingleGameInfoDic[progress_id].name;
            int progress_id_temp=progress_id;
            gameNameObj.GetComponent<Button>().onClick.AddListener(()=>{
                action(progress_id_temp);
            });
            gameNameObj.transform.localScale = Vector3.one;

            gameNameObj.SetActive(true);
            progress_id++;
        }
    }



    public void SetLoadGameVisible(bool visible)
    {
        gameMenu.transform.FindChild("LoadBtn").GetComponent<Button>().interactable = visible;
    }



    public void MultiGameAddGame(GameHallGame gameHallGame)
    {
        string game_name = gameHallGame.game_name;
        int creator_id = gameHallGame.creator_id;
        string creator_name = gameHallGame.players_info[creator_id].player_name;
        int gametype_id = gameHallGame.gametype_id;
        int player_count = gameHallGame.players_info.Count;

        GameObject itemObj = GameObject.Instantiate(multiGameItemTemplate as UnityEngine.Object) as GameObject;

        Text creatorText = itemObj.transform.Find("ItemDetail/Creator").GetComponent<Text>();
        Text typeText = itemObj.transform.Find("ItemDetail/Type").GetComponent<Text>();
        Text playerCountText = itemObj.transform.Find("ItemDetail/PlayerCount").GetComponent<Text>();

        creatorText.text = creator_name;
        typeText.text = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].desc;
        int maxPlayerCount = 0;
        for (int i = 0; i < dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].playercount_in_group.Count; i++)
        {
            maxPlayerCount += dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].playercount_in_group[i];
        }
        playerCountText.text = player_count + "/" + maxPlayerCount;

        itemObj.GetComponent<Toggle>().onValueChanged.AddListener((newValue) =>
        {
            if (newValue == true)
            {
                selectedCreatorId = int.Parse(itemObj.name);
            }
        });

        itemObj.name = creator_id.ToString();
        itemObj.transform.SetParent(multiGameScrollviewList.transform);
        itemObj.transform.localScale = Vector3.one;
        itemObj.transform.localPosition = Vector3.zero;
        itemObj.SetActive(true);
    }

    public void MultiGameRemoveGame(int gameid)
    {
        GameObject itemObj = multiGameScrollviewList.transform.Find(gameid.ToString()).gameObject;
        Destroy(itemObj);
        if (selectedCreatorId == gameid)
        {
            selectedCreatorId = -1;
            Debug.Log(selectedCreatorId);
        }
    }

    public void MultiGameInitList(GameHallDic gameHallDic)
    {
        Tools.ClearChildren(multiGameScrollviewList.transform);
        foreach (int gameid in gameHallDic.gameHallDic.Keys)
        {
            GameHallGame gameHallGame=gameHallDic.gameHallDic[gameid];
            MultiGameAddGame(gameHallGame);
        }
    }

//    public void OnMultiGameItemValueChanged(bool value)
//    {
//        Debug.Log(value);
//        if (value)
//        {
//            Debug.Log(multiGameScrollviewList.GetComponent<ToggleGroup>().ActiveToggles());
//        }
//    }

    public void MultiGameSettingSetGroupCount(int groupCount)
    {
        Tools.ClearChildren(multiGameSettingPanel.transform);
        for (int i = 0; i < groupCount; i++)
        {
            GameObject groupItem = GameObject.Instantiate(multiGameSettingGroupTemplate as UnityEngine.Object) as GameObject;

            groupItem.name = (i + 1).ToString();
            groupItem.transform.FindChild("Team").GetComponent<Text>().text="队伍"+(i+1);
            groupItem.transform.SetParent(multiGameSettingPanel.transform);
            groupItem.transform.localPosition = Vector3.zero;
            groupItem.transform.localScale = Vector3.one;
            groupItem.SetActive(true);
        }
    }

    public void MultiGameSettingAddPlayer(GameHallPlayer gameHallPlayer)
    {
        GameObject groupItem = multiGameSettingPanel.transform.FindChild(gameHallPlayer.group_id.ToString()).gameObject;

        GameObject playerItem = GameObject.Instantiate(multiGameSettingPlayerTemplate as UnityEngine.Object) as GameObject;

        playerItem.name = gameHallPlayer.player_id.ToString();
        playerItem.GetComponent<Text>().text = gameHallPlayer.player_name;
        playerItem.transform.SetParent(groupItem.transform.FindChild("PlayerList"));
        playerItem.transform.localPosition = Vector3.zero;
        playerItem.transform.localScale = Vector3.one;
        playerItem.SetActive(true);

    }

    public void MultiGameSettingRemovePlayer(int player_id)
    {
        GameObject item;
        for (int i = 0; i < multiGameSettingPanel.transform.childCount; i++)
        {
            GameObject groupItem = multiGameSettingPanel.transform.GetChild(i).gameObject;
            Transform playerListT = groupItem.transform.FindChild("PlayerList");
            for (int j = 0; j < playerListT.childCount; j++)
            {
                GameObject playerItem = playerListT.GetChild(j).gameObject;
//                Debug.Log(playerItem.name+",,,"+player_id.ToString());
                if (playerItem.name == player_id.ToString())
                {
                    item = playerItem;
                    goto togo;
                }

            }
        }
        Debug.LogError("can not find that player!");
        return;

        togo:
        Destroy(item);
    }

    public void MultiGameSettingClearAllPlayers()
    {
        for (int i = 0; i < multiGameSettingPanel.transform.childCount; i++)
        {
            GameObject groupItem = multiGameSettingPanel.transform.GetChild(i).gameObject;
            Transform playerListT = groupItem.transform.FindChild("PlayerList");
            for (int j = 0; j < playerListT.childCount; j++)
            {
                GameObject playerItem = playerListT.GetChild(j).gameObject;
                Destroy(playerItem);

            }
        }
    }

    public void MultiGameSettingInit(GameHallGame gameHallGame)
    {
        int groupCount = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameHallGame.gametype_id].playercount_in_group.Count;
        MultiGameSettingSetGroupCount(groupCount);
        foreach (int player_id in gameHallGame.players_info.Keys)
        {
            MultiGameSettingAddPlayer(gameHallGame.players_info[player_id]);
        }
    }

}
