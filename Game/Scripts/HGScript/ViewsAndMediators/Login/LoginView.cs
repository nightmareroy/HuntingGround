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

    [Inject]
    public IconSpritesService iconSpritesService { get;set; }

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
    //public GameObject gameTypeMenu;
    //public GameObject oneOnOneHallGameSettingMenu;
    //public GameObject oneOnOneHallGamesMenu;
    public GameObject singleGameMenu;
    public GameObject friendMenu;
    public GameObject friendListMenu;
    public GameObject receiveApplyMenu;
    public GameObject sendApplyMenu;

    public Transform friendItemTplT;
    public Transform friendListRootT;
    public Transform deleteFriendPanelT;
    public Transform waitInvitePanelT;

    public Transform receiveApplyItemTplT;
    public Transform receiveApplyRootT;

    public Transform sendApplyItemTplT;
    public Transform sendApplyRootT;

    public Transform inviteFightPanelT;
    public Text inviterName;
    public Button inviteAcceptBtn;
    public Button inviteRefuseBtn;

    //public GameObject multiGameItemTemplate;
    //public GameObject multiGameScrollviewList;

    //public GameObject multiGameSettingPlayerTemplate;
    //public GameObject multiGameSettingGroupTemplate;
    //public GameObject oneOnOneGameSettingPanel;

    public GameObject gameNamesRoot;
    public GameObject gameNameTpl;

    

    public GameObject messagePanel;
    public Text messageText;

    public Text applyFriendName;

    public Transform applyPanelT;

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

    [HideInInspector]
    public int selectedFriendUid=-1;

    [HideInInspector]
    public int selectedReceiveApplyUid = -1;

    [HideInInspector]
    public int selectedCancelApplyUid = -1;

    [HideInInspector]
    public int selectedInviterUid = -1;


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

    public void OnLadderClicked()
    {
        if (viewClick != null)
        {
            viewClick("Ladder");
        }
    }

    public void OnFriendClicked()
    {
        if (viewClick != null)
        {
            viewClick("Friend");
        }
    }

    //friend
    public void OnFriendListClicked()
    {
        if (viewClick != null)
        {
            viewClick("FriendList");
        }
    }

    public void OnConfirmApplyFriendClicked()
    {
        if (viewClick != null)
        {
            viewClick("ConfirmApplyFriend");
        }
    }

    public void OnReceiveApplyClicked()
    {
        if (viewClick != null)
        {
            viewClick("ReceiveApply");
        }
    }

    public void OnSendApplyClicked()
    {
        if (viewClick != null)
        {
            viewClick("SendApply");
        }
    }

    //friend list
    //public void OnFightFriendClicked()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("FightFriend");
    //    }
    //}

    public void OnDeleteFriendClicked()
    {
        if (viewClick != null)
        {
            viewClick("DeleteFriend");
        }
    }

    public void OnCancelFightClicked()
    {
        if (viewClick != null)
        {
            viewClick("CancelFight");
        }
    }

    //invite fight
    public void OnAcceptInviteFightClicked()
    {
        if (viewClick != null)
        {
            viewClick("AcceptInviteFight");
        }
    }

    public void OnRefuseInviteFightClicked()
    {
        if (viewClick != null)
        {
            viewClick("RefuseInviteFight");
        }
    }
    
    

    //1v1 game
    //public void OnCreateOneOnOneGameClicked()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("CreateOneOnOneGame");
    //    }
    //}

    //public void OnJoinOneOnOneGameClicked()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("JoinOneOnOneGame");
    //    }
    //}

    //game type
    //public void On1v1Clicked()
    //{
        
    //    if (viewClick != null)
    //    {
    //        viewClick("1v1");
    //    }
    //}

    //public void On2v2Clicked()
    //{
        
    //    if (viewClick != null)
    //    {
    //        viewClick("2v2");
    //    }
    //}

    //1v1 game setting
    //public void OnStartOneOnOneGameClicked()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("StartOneOnOneGame");
    //    }
    //}

    //public void OnCancelOrLeaveOneOnOneGameClicked()
    //{
    //    if (viewClick != null)
    //    {
    //        viewClick("CancelOrLeaveOneOnOneGame");
    //    }
    //}

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

    //single game
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

    //friend

    public void SetApplyPanelVisible(bool visible)
    {
        applyPanelT.gameObject.SetActive(visible);
    }
    

    

    //friend list
    public void InitFriendList(List<Friend> friendList)
    {
        Tools.ClearChildren(friendListRootT);
        //JsonObject friends_idle = jo["friends_idle"] as JsonObject;
        //JsonObject friends_fighting = jo["friends_fighting"] as JsonObject;
        //JsonObject friends_offline = jo["friends_offline"] as JsonObject;
        

        foreach (Friend friend in friendList)
        {
            //JsonObject itemJS = key as JsonObject;
            //int uid = int.Parse( itemJS["uid"].ToString());
            //string name = itemJS["name"].ToString();
            //int state = int.Parse(itemJS["state"].ToString());

            GameObject item = GameObject.Instantiate<GameObject>(friendItemTplT.gameObject);
            item.transform.SetParent(friendListRootT);
            item.SetActive(true);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.name = friend.uid.ToString();

            Text nameText = item.transform.FindChild("Name").GetComponent<Text>();
            Image stateImg = item.transform.FindChild("State").GetComponent<Image>();
            Button fightBtn = item.transform.FindChild("FightBtn").GetComponent<Button>();
            Button deleteBtn = item.transform.FindChild("DeleteBtn").GetComponent<Button>();

            nameText.text = friend.name;
            fightBtn.onClick.AddListener(() =>
            {
                selectedFriendUid = friend.uid;
                if (viewClick != null)
                {
                    viewClick("FightFriend");
                }
            });
            deleteBtn.onClick.AddListener(() =>
            {
                selectedFriendUid = friend.uid;

            });

            switch (friend.state)
            {
                    //offline
                case 0:
                    nameText.color = Color.black;
                    stateImg.sprite = iconSpritesService.GetView().ball1;
                    fightBtn.interactable = false;
                    break;
                    //fighting
                case 1:
                    nameText.color = Color.white;
                    stateImg.sprite = iconSpritesService.GetView().ball2;
                    fightBtn.interactable = false;
                    break;
                    //idle
                case 2:
                    nameText.color = Color.white;
                    stateImg.sprite = iconSpritesService.GetView().ball3;
                    fightBtn.interactable = true;
                    break;
            }
        }
    }

    

    public void DeleteFriend(int friend_uid)
    {
        Tools.Destroy(friendListRootT.FindChild(friend_uid.ToString()));
        
    }

    public void SetWaitInvitePanelVisible(bool visible)
    {
        waitInvitePanelT.gameObject.SetActive(visible);
    }

    public void SetDeleteFriendPanelVisible(bool visible)
    {
        deleteFriendPanelT.gameObject.SetActive(visible);
    }

    //receive apply
    public void InitReceiveApplyList(JsonObject jo)
    {
        Tools.ClearChildren(receiveApplyRootT);
        foreach (string key in jo.Keys)
        {
            int uid = int.Parse(key);
            string name = (jo[key] as JsonObject)["name"].ToString();

            GameObject item = GameObject.Instantiate<GameObject>(receiveApplyItemTplT.gameObject);
            item.transform.SetParent(receiveApplyRootT);
            item.SetActive(true);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.name = uid.ToString();

            Text nameText = item.transform.FindChild("Name").GetComponent<Text>();
            Button agreeBtn = item.transform.FindChild("AgreeBtn").GetComponent<Button>();
            Button refuseBtn = item.transform.FindChild("RefuseBtn").GetComponent<Button>();

            nameText.text = name;
            agreeBtn.onClick.AddListener(() =>
            {
                selectedReceiveApplyUid = uid;
                if (viewClick != null)
                {
                    viewClick("AgreeApply");
                }
            });
            refuseBtn.onClick.AddListener(() =>
            {
                selectedReceiveApplyUid = uid;
                if (viewClick != null)
                {
                    viewClick("RefuseApply");
                }
            });
        }
    }

    

    public void DeleteApply(int friend_uid)
    {
        Tools.Destroy(receiveApplyRootT.FindChild(friend_uid.ToString()));
    }

    //send apply
    public void InitSendApplyList(JsonObject jo)
    {
        Tools.ClearChildren(sendApplyRootT);
        foreach (string key in jo.Keys)
        {
            int uid = int.Parse(key);
            string name = (jo[key] as JsonObject)["name"].ToString();

            GameObject item = GameObject.Instantiate<GameObject>(sendApplyItemTplT.gameObject);
            item.transform.SetParent(sendApplyRootT);
            item.SetActive(true);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            item.name = uid.ToString();

            Text nameText = item.transform.FindChild("Name").GetComponent<Text>();
            Button cancelBtn = item.transform.FindChild("CancelBtn").GetComponent<Button>();

            nameText.text = name;
            cancelBtn.onClick.AddListener(() =>
            {
                selectedCancelApplyUid = uid;
                if (viewClick != null)
                {
                    viewClick("CancelApply");
                }
            });

        }
    }


    public void DeleteSend(int friend_uid)
    {
        Tools.Destroy(sendApplyRootT.FindChild(friend_uid.ToString()));
    }


    //invite fight
    public void ShowInvitePanel(int src_uid, string name)
    {
        inviteFightPanelT.gameObject.SetActive(true);
        inviterName.text = name;
        selectedInviterUid = src_uid;
    }

    public void HideInvitePanel()
    {
        inviteFightPanelT.gameObject.SetActive(false);
        //inviterName.text = name;
        selectedInviterUid = -1;
    }



    //shared
    public void ShowMessage(string content)
    {
        messagePanel.SetActive(true);
        messageText.text = content;

    }


    //public void SetLoadGameVisible(bool visible)
    //{
    //    gameMenu.transform.FindChild("LoadBtn").GetComponent<Button>().interactable = visible;
    //}



    //public void MultiGameAddGame(GameHallGame gameHallGame)
    //{
    //    string game_name = gameHallGame.game_name;
    //    int creator_id = gameHallGame.creator_id;
    //    string creator_name = gameHallGame.players_info[creator_id].name;
    //    int gametype_id = gameHallGame.gametype_id;
    //    int player_count = gameHallGame.players_info.Count;

    //    GameObject itemObj = GameObject.Instantiate(multiGameItemTemplate as UnityEngine.Object) as GameObject;

    //    Text creatorText = itemObj.transform.Find("ItemDetail/Creator").GetComponent<Text>();
    //    Text typeText = itemObj.transform.Find("ItemDetail/Type").GetComponent<Text>();
    //    Text playerCountText = itemObj.transform.Find("ItemDetail/PlayerCount").GetComponent<Text>();

    //    creatorText.text = creator_name;
    //    typeText.text = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].desc;
    //    int maxPlayerCount = 0;
    //    for (int i = 0; i < dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].playercount_in_group.Count; i++)
    //    {
    //        maxPlayerCount += dGameDataCollection.dGameTypeCollection.dGameTypeDic[gametype_id].playercount_in_group[i];
    //    }
    //    playerCountText.text = player_count + "/" + maxPlayerCount;

    //    itemObj.GetComponent<Toggle>().onValueChanged.AddListener((newValue) =>
    //    {
    //        if (newValue == true)
    //        {
    //            selectedCreatorId = int.Parse(itemObj.name);
    //        }
    //    });

    //    itemObj.name = creator_id.ToString();
    //    itemObj.transform.SetParent(multiGameScrollviewList.transform);
    //    itemObj.transform.localScale = Vector3.one;
    //    itemObj.transform.localPosition = Vector3.zero;
    //    itemObj.SetActive(true);
    //}

    //public void MultiGameRemoveGame(int gameid)
    //{
    //    GameObject itemObj = multiGameScrollviewList.transform.Find(gameid.ToString()).gameObject;
    //    Destroy(itemObj);
    //    if (selectedCreatorId == gameid)
    //    {
    //        selectedCreatorId = -1;
    //        Debug.Log(selectedCreatorId);
    //    }
    //}

    //public void MultiGameInitList(GameHallDic gameHallDic)
    //{
    //    Tools.ClearChildren(multiGameScrollviewList.transform);
    //    foreach (int gameid in gameHallDic.gameHallDic.Keys)
    //    {
    //        GameHallGame gameHallGame=gameHallDic.gameHallDic[gameid];
    //        MultiGameAddGame(gameHallGame);
    //    }
    //}

//    public void OnMultiGameItemValueChanged(bool value)
//    {
//        Debug.Log(value);
//        if (value)
//        {
//            Debug.Log(multiGameScrollviewList.GetComponent<ToggleGroup>().ActiveToggles());
//        }
//    }





    //public void MultiGameSettingSetGroupCount(int groupCount)
    //{
    //    Tools.ClearChildren(multiGameSettingPanel.transform);
    //    for (int i = 0; i < groupCount; i++)
    //    {
    //        GameObject groupItem = GameObject.Instantiate(multiGameSettingGroupTemplate as UnityEngine.Object) as GameObject;

    //        groupItem.name = (i + 1).ToString();
    //        groupItem.transform.FindChild("Team").GetComponent<Text>().text="队伍"+(i+1);
    //        groupItem.transform.SetParent(multiGameSettingPanel.transform);
    //        groupItem.transform.localPosition = Vector3.zero;
    //        groupItem.transform.localScale = Vector3.one;
    //        groupItem.SetActive(true);
    //    }
    //}

    //public void MultiGameSettingAddPlayer(GameHallPlayer gameHallPlayer,int colorIndex)
    //{
    //    GameObject groupItem = multiGameSettingPanel.transform.FindChild(gameHallPlayer.group_id.ToString()).gameObject;

    //    GameObject playerItem = GameObject.Instantiate(multiGameSettingPlayerTemplate as UnityEngine.Object) as GameObject;

    //    playerItem.name = gameHallPlayer.uid.ToString();
    //    playerItem.GetComponent<Text>().text = gameHallPlayer.name;
    //    playerItem.transform.SetParent(groupItem.transform.FindChild("PlayerList"));
    //    playerItem.transform.localPosition = Vector3.zero;
    //    playerItem.transform.localScale = Vector3.one;
    //    playerItem.SetActive(true);

    //}

    //public void MultiGameSettingRemovePlayer(int player_id)
    //{
    //    GameObject item;
    //    for (int i = 0; i < multiGameSettingPanel.transform.childCount; i++)
    //    {
    //        GameObject groupItem = multiGameSettingPanel.transform.GetChild(i).gameObject;
    //        Transform playerListT = groupItem.transform.FindChild("PlayerList");
    //        for (int j = 0; j < playerListT.childCount; j++)
    //        {
    //            GameObject playerItem = playerListT.GetChild(j).gameObject;

    //            if (playerItem.name == player_id.ToString())
    //            {
    //                item = playerItem;
    //                goto togo;
    //            }

    //        }
    //    }
    //    Debug.LogError("can not find that player!");
    //    return;

    //    togo:
    //    Destroy(item);
    //}

    //public void MultiGameSettingClearAllPlayers()
    //{
    //    for (int i = 0; i < multiGameSettingPanel.transform.childCount; i++)
    //    {
    //        GameObject groupItem = multiGameSettingPanel.transform.GetChild(i).gameObject;
    //        Transform playerListT = groupItem.transform.FindChild("PlayerList");
    //        for (int j = 0; j < playerListT.childCount; j++)
    //        {
    //            GameObject playerItem = playerListT.GetChild(j).gameObject;
    //            Destroy(playerItem);

    //        }
    //    }
    //}

    //public void MultiGameSettingInit(GameHallGame gameHallGame)
    //{
    //    int groupCount = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameHallGame.gametype_id].playercount_in_group.Count;
    //    MultiGameSettingSetGroupCount(groupCount);
    //    foreach (int player_id in gameHallGame.players_info.Keys)
    //    {
    //        MultiGameSettingAddPlayer(gameHallGame.players_info[player_id]);
    //    }
    //}

    //public void OneOnOneGameAddPlayer(GameHallPlayer gameHallPlayer)
    //{
    //    Transform groupItemT = oneOnOneGameSettingPanel.transform.FindChild(gameHallPlayer.group_id.ToString());

    //    //GameObject playerItem = GameObject.Instantiate(multiGameSettingPlayerTemplate as UnityEngine.Object) as GameObject;
    //    Transform playerItemT = groupItemT.FindChild("PlayerList/Player");

    //    //Image colorImg = playerItemT.FindChild("Color").GetComponent<Image>();
    //    Text name = playerItemT.FindChild("Name").GetComponent<Text>();

    //    playerItemT.name = gameHallPlayer.uid.ToString();
    //    name.text = gameHallPlayer.name;

    //}

    //public void OneOnOneGameRemovePlayer(int player_id)
    //{

    //}

}
