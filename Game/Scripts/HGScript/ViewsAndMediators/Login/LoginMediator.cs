using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine.UI;
using SimpleJson;

public class LoginMediator : Mediator {
    public class testc
    {
        public Dictionary<int, int> testd = new Dictionary<int, int>();
    }
    

    [Inject]
    public LoginView loginView{get;set;}



    [Inject]
    public StartGameSignal startGameSignal { get; set; }


    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    //[Inject]
    //public UpdateMapInfoSignal updateMapInfoSignal { get; set; }


    [Inject]
    public RegisterSignal registerSignal { get; set; }

    //[Inject]
    //public GetServerListSignal getServerListSignal { get; set; }

    //[Inject]
    //public ConnectSignal connectSignal { get; set; }


    [Inject]
    public GameInfo gameInfo { get; set; }

    //game hall
    //[Inject]
    //public CreateMultiGamePushSignal createMultiGamePushSignal{ get; set;}
    //[Inject]
    //public CancelMultiGamePushSignal cancelMultiGamePushSignal{ get; set;}
    //[Inject]
    //public JoinMultiGamePushSignal joinMultiGamePushSignal{ get; set;}
    //[Inject]
    //public LeaveMultiGamePushSignal leaveMultiGamePushSignal{ get; set;}

    //game
    [Inject]
    public MultiGameStartPushSignal multiGameStartPushSignal{ get; set;}

    int width;
    int height;
//    int gametypeid;
    //GamePlayerInfo user;
    //Dictionary<int,GamePlayerInfo> opponents;
    //List<int> landform_map;
    //List<int> resource_map;
    //List<int> explorestatus_map;
    //StartGameSignal.Param param;

    //public Page mainPage;
    //
    public PageManager pageManager;

    public Page loginPage;
    public Page registerPage;
    public Page gamePage;
    //public Page gameTypePage;
    //public Page oneOnOneHallGameSettingPage;
    //public Page oneOnOneHallGamesPage;
    
    public Page singleGamePage;
    public Page friendPage;
    public Page friendListPage;
    public Page receiveApplyPage;
    public Page sendApplyPage;
    //public Page serverSelectPage;

    //UpdateMapInfoSignal.Param updateMapInfoSignalParam;
    NetService.ServerConnector server=null;

    GameHallDic gameHallDic=new GameHallDic();

    int multi_game_type_id;

    int colorIndexToSelect = 0;

    public override void OnRegister()
    {
        loginView.Init();

        InitPages();
        loginView.viewClick += OnViewClicked;

        //game hall
        //createMultiGamePushSignal.AddListener(OnCreateMultiGamePushSignal);
        //cancelMultiGamePushSignal.AddListener(OnCancelMultiGamePushSignal);
        //joinMultiGamePushSignal.AddListener(OnJoinMultiGamePushSignal);
        //leaveMultiGamePushSignal.AddListener(OnLeaveMultiGamePushSignal);

        //game
        multiGameStartPushSignal.AddListener(OnMultiGameStartPushSignal);


        if (netService.GetConnectStatus() == Pomelo.DotNetClient.NetWorkState.DISCONNECTED)
        {
            netService.GetServerConnector((connector) =>
                {
                    Debug.Log(connector.host + connector.port);
                    netService.Connect(connector, () =>
                        {
                            Debug.Log("connect success!");
                        });
                });
        }
    }

    void InitPages()
    {
        //mainPage = new Page(loginView.mainMenu);
        //
        loginPage = new Page(loginView.loginMenu);
        registerPage = new Page(loginView.registerMenu);
        gamePage = new Page(loginView.gameMenu);
        //gameTypePage = new Page(loginView.gameTypeMenu);
        //oneOnOneHallGameSettingPage = new Page(loginView.oneOnOneHallGameSettingMenu);
        //oneOnOneHallGamesPage = new Page(loginView.oneOnOneHallGamesMenu);
        singleGamePage = new Page(loginView.singleGameMenu);
        friendPage = new Page(loginView.friendMenu);
        friendListPage = new Page(loginView.friendListMenu);
        receiveApplyPage = new Page(loginView.receiveApplyMenu);
        sendApplyPage = new Page(loginView.sendApplyMenu);

        pageManager = new PageManager(loginPage);

        //mainPage.SetChild(singleSettingPage);
        //mainPage.SetChild(loginPage);
        //serverSelectPage.SetChild(loginPage);

        loginPage.SetChild(registerPage);
        loginPage.SetChild(gamePage);

        gamePage.SetChild(singleGamePage);
        gamePage.SetChild(friendPage);

        friendPage.SetChild(friendListPage);
        friendPage.SetChild(receiveApplyPage);
        friendPage.SetChild(sendApplyPage);

        //oneOnOneHallGamesPage.SetChild(gameTypePage);

        //oneOnOneHallGamesPage.SetChild(oneOnOneHallGameSettingPage);




    }

    void OnViewClicked(string clickName)
    {
        Debug.Log(clickName);
        switch (clickName)
        {
                //login
            case "Login":
                //loginSignal.Dispatch(new LoginSignal.Param(loginView.loginAccount.text, loginView.loginPwd.text));
                JsonObject user_login=new JsonObject();
                user_login["account"]=loginView.loginAccount.text;
                user_login["pwd"]=loginView.loginPwd.text;
                netService.Request(NetService.loginRoute, user_login, (msg) => {
                    //Debug.Log(msg.rawString);

                    


                    if (msg.code == 200)
                    {
                        JsonObject splayerObj=(msg.data as JsonObject)["splayer"] as JsonObject;
                        //bool isingame=bool.Parse((msg.data as JsonObject)["isingame"].ToString());

                        SPlayerInfo sPlayerInfo_temp=new SPlayerInfo();
                        sPlayerInfo_temp=SimpleJson.SimpleJson.DeserializeObject<SPlayerInfo>(splayerObj.ToString());
                        sPlayerInfo.UpdateSplayer(sPlayerInfo_temp);

                        pageManager.JumpDown(gamePage);
                        //loginView.SetLoadGameVisible(isingame);

                    }
                    else if (msg.code == 500)
                    {
                        loginView.ShowMessage(msg.data.ToString());
                    }
                    
                });
                break;
            case "Register":
                pageManager.JumpDown(registerPage);
                break;

                //register
            case "ConfirmRegister":
                if (loginView.registerPwd.text != loginView.registerPwd2.text)
                {
                    Debug.Log("there two passwords are different..");
                    return;
                }
                JsonObject user_register=new JsonObject();
                user_register["account"]=loginView.registerAccount.text;
                user_register["pwd"]=loginView.registerPwd.text;
                netService.Request(NetService.registerRoute, user_register, (msg) =>
                {
                    //Debug.Log(msg.rawString);
                    loginView.ShowMessage(msg.data.ToString());
                });

                //registerSignal.Dispatch(new RegisterSignal.Param(loginView.loginAccount.text, loginView.loginPwd.text));
                break;

                //game
            case "CreateSingleGame":
                pageManager.JumpDown(singleGamePage,()=>{
                    loginView.InitSingleGameBtns((progress_id)=>{
                        JsonObject form=new JsonObject();
                        form.Add("progress_id",progress_id);
                        netService.Request(NetService.SingleGameStart,form,(msg)=>{
                            if (msg.code == 200)
                            {
                                netService.Request(NetService.LoadGame, null, (msg2) =>
                                    {
                                        if (msg2.code == 200)
                                        {
                                            gameInfo.InitFromJson(msg2.data as JsonObject);
                                            gameInfo.allplayers_dic[sPlayerInfo.uid].color_index = 0;
                                            startGameSignal.Dispatch();
                                        }
                                    });
                            }
                            else
                            {
                                loginView.ShowMessage(msg.data.ToString());
                            }
                        });
                    });
                });
                break;
            case "Ladder":
                //netService.Request(NetService.enterOneOnOneGameHall,null,(msg)=>{
                //    if(msg.code==200)
                //    {
                //        gameHallDic.InitFromJson(msg.data as JsonObject);
                //        loginView.MultiGameInitList(gameHallDic);
                //        pageManager.JumpDown(oneOnOneHallGamesPage);
                //    }
                //});
                break;
            case "Friend":
                pageManager.JumpDown(friendPage);
                break;

                //friend

            case "FriendList":
                JsonObject form_get_friends = new JsonObject();
                netService.Request(NetService.GetFriends, form_get_friends, (msg) =>
                {

                    if (msg.code == 200)
                    {
                        List<Friend> friendList = new List<Friend>();
                        foreach (object key in msg.data as JsonArray)
                        {
                            Friend friend = SimpleJson.SimpleJson.DeserializeObject<Friend>(key.ToString());
                            friendList.Add(friend);
                        }

                        friendList.Sort(SortFriend);
                        loginView.InitFriendList(friendList);
                        pageManager.JumpDown(friendListPage);
                    }
                });
                break;
            case "ConfirmApplyFriend":
                if (loginView.applyFriendName.text.Length == 0)
                {
                    loginView.ShowMessage("用户昵称不能为空");
                    break;
                }
                JsonObject form_confirm_apply_friends = new JsonObject();
                form_confirm_apply_friends.Add("tar_name",loginView.applyFriendName.text);
                netService.Request(NetService.ApplyFriend, form_confirm_apply_friends, (msg) =>
                {
                    //Debug.Log(msg_confirm_apply_friends.rawString);
                    if (msg.code == 200)
                    {
                        //loginView.InitFriendList(msg_confirm_apply_friends.data as JsonObject);
                        //pageManager.JumpDown(friendListPage);
                        loginView.SetApplyPanelVisible(false);
                    }
                    //else if (msg_confirm_apply_friends.code == 500)
                    //{
                        
                    //}
                    loginView.ShowMessage(msg.data.ToString());
                });
                break;
            case "ReceiveApply":
                JsonObject form_receive_apply = new JsonObject();
                //form_receive_apply.Add("tar_name", loginView.applyFriendName.text);
                netService.Request(NetService.GetApplications, form_receive_apply, (msg) =>
                {
                    //Debug.Log(msg_receive_apply.rawString);
                    if (msg.code == 200)
                    {
                        loginView.InitReceiveApplyList(msg.data as JsonObject);
                        pageManager.JumpDown(receiveApplyPage);
                    }

                    
                });
                break;

            case "SendApply":
                JsonObject form_send_apply = new JsonObject();
                //form_send_apply.Add("tar_name", loginView.applyFriendName.text);
                netService.Request(NetService.GetSends, form_send_apply, (msg) =>
                {
                    //Debug.Log(msg_receive_apply.rawString);
                    if (msg.code == 200)
                    {
                        loginView.InitSendApplyList(msg.data as JsonObject);
                        pageManager.JumpDown(sendApplyPage);
                    }


                });
                break;



                //friend list
            case "FightFriend":
                //JsonObject form_fight_friend = new JsonObject();
                //netService.Request(NetService.GetFriends, form_get_friends, (msg_get_friends) =>
                //{

                //    if (msg_get_friends.code == 200)
                //    {
                //        loginView.InitFriendList(msg_get_friends.data as JsonObject);
                //    }
                //});
                break;
            case "DeleteFriend":
                JsonObject form_delete_friend = new JsonObject();
                form_delete_friend.Add("tar_uid",loginView.selectedFriendUid);
                netService.Request(NetService.DeleteFriend, form_delete_friend, (msg) =>
                {

                    if (msg.code == 200)
                    {
                        int friend_uid = int.Parse(msg.data.ToString());
                        loginView.DeleteFriend(friend_uid);
                        loginView.SetDeleteFriendPanelVisible(false);
                    }
                });
                break;
            case "CancelFight":
                break;
            

                //receive apply
            case "AgreeApply":
                JsonObject form_agree_apply = new JsonObject();
                form_agree_apply.Add("src_uid", loginView.selectedReceiveApplyUid);
                netService.Request(NetService.AgreeApplication, form_agree_apply, (msg) =>
                {

                    if (msg.code == 200)
                    {
                        int friend_uid = int.Parse(msg.data.ToString());
                        loginView.DeleteApply(friend_uid);
                    }
                });
                break;
            case "RefuseApply":
                JsonObject form_refuse_apply = new JsonObject();
                form_refuse_apply.Add("src_uid", loginView.selectedReceiveApplyUid);
                netService.Request(NetService.RefuseApplication, form_refuse_apply, (msg) =>
                {

                    if (msg.code == 200)
                    {
                        int friend_uid = int.Parse(msg.data.ToString());
                        loginView.DeleteApply(friend_uid);
                    }
                });
                break;

                //send apply
            case "CancelApply":
                JsonObject form_cancel_apply = new JsonObject();
                form_cancel_apply.Add("tar_uid", loginView.selectedCancelApplyUid);
                netService.Request(NetService.CancelApplication, form_cancel_apply, (msg) =>
                {

                    if (msg.code == 200)
                    {
                        int friend_uid = int.Parse(msg.data.ToString());
                        loginView.DeleteSend(friend_uid);
                    }
                });
                break;

            
                //shared
            case "Return":
                pageManager.JumpUp();
                break;
            case "ReturnToGamePage":
                pageManager.JumpTo(gamePage);
                break;
            
            
            
            default:
                break;
        }
    }

    int SortFriend(Friend friend_a, Friend friend_b)
    {
        return friend_a.state - friend_b.state;
    }

    public void OnSingleGameNameClick(int progress_id)
    {
        
    }

    //void OnPlayModeSignal(LoginView.PlayMode mode)
    //{
    //    switch (mode)
    //    {
    //        case LoginView.PlayMode.single:

    //            //param.allplayer.Clear();
    //            //GamePlayerInfo gamePlayerInfo=new GamePlayerInfo();
    //            //gamePlayerInfo.id=-1;
    //            //param.allplayer.Add(gamePlayerInfo.id, gamePlayerInfo);
    //            //param.uplayer = gamePlayerInfo;
                    
    //            break;
    //        case LoginView.PlayMode.multi:
    //            break;
    //    }
    //}

    void OnSingleMapSizeSignal(LoginView.MapSize mapSize)
    {
        switch (mapSize)
        {
            case LoginView.MapSize.small:
                //param.map_info.width = 20;
                //param.map_info.height = 40;
                width = 20;
                height = 40;
                break;
            case LoginView.MapSize.big:
                //param.map_info.width = 30;
                //param.map_info.height = 60;
                width = 30;
                height = 60;
                break;
        }
        //param.map_info.landform_map.Clear();
        //param.map_info.resource_map.Clear();
        //param.map_info.explorestatus_map.Clear();

        //for (int i = 0; i < param.map_info.width * param.map_info.height; i++)
        //{
        //    param.map_info.landform_map.Add(0);
        //    param.map_info.resource_map.Add(0);
        //    param.map_info.explorestatus_map.Add(0);
        //}
    }

    //void OnSingleStartSignal()
    //{
    //    Debug.Log("start a single game");
    //    startGameSignal.Dispatch(param);
    //}


    void OnLoginCallbackSignal()
    {
        //pageManager.JumpDown(gamePage);
    }






    void OnCreateMultiGamePushSignal(JsonObject jo)
    {
        //GameHallGame gameHallGame = new GameHallGame();
        //gameHallGame.InitFromJson(jo);
        //gameHallDic.gameHallDic.Add(gameHallGame.creator_id,gameHallGame);

        //if (gameHallGame.creator_id==sPlayerInfo.uid)
        //{

        //    loginView.MultiGameSettingInit(gameHallGame);
        //    pageManager.JumpTo(oneOnOneHallGameSettingPage);
        //}

        //if (pageManager.activePage == oneOnOneHallGamesPage)
        //{
        //    loginView.MultiGameAddGame(gameHallGame);
        //}


    }

    //void OnCancelMultiGamePushSignal(JsonObject jo)
    //{
    //    int creator_id = int.Parse(jo["creator_id"].ToString());
    //    bool is_start = bool.Parse(jo["is_start"].ToString());

    //    GameHallGame gameHallGame = gameHallDic.gameHallDic[creator_id];
    //    gameHallDic.gameHallDic.Remove(creator_id);

    //    if (pageManager.activePage == oneOnOneHallGamesPage)
    //    {
    //        loginView.MultiGameRemoveGame(creator_id);
    //    }
    //    else if (pageManager.activePage == oneOnOneHallGameSettingPage)
    //    {
    //        if (is_start == false)
    //        {
    //            if (gameHallGame.players_info.ContainsKey(sPlayerInfo.uid))
    //            {
    //                loginView.MultiGameInitList(gameHallDic);
    //                pageManager.JumpTo(oneOnOneHallGamesPage);
    //            }
    //        }
    //    }


    //}

    void OnJoinMultiGamePushSignal(JsonObject jo)
    {
//        int creator_id = int.Parse(jo["creator_id"].ToString());
//        int uid = int.Parse(jo["uid"].ToString());

//        GameHallPlayer gameHallPlayer = new GameHallPlayer();
//        gameHallPlayer.uid = uid;
//        gameHallPlayer.group_id = int.Parse(jo["group_id"].ToString());
//        gameHallPlayer.name = jo["name"].ToString();


//        gameHallDic.gameHallDic[creator_id].players_info.Add(uid,gameHallPlayer);

//        if (pageManager.activePage == oneOnOneHallGamesPage)
//        {
//            if (uid == sPlayerInfo.uid)
//            {
//                loginView.MultiGameSettingInit(gameHallDic.gameHallDic[creator_id]);
////                loginView.MultiGameSettingAddPlayer(gameHallPlayer);
//                pageManager.JumpTo(oneOnOneHallGameSettingPage);
//            }
//        }
//        else if (pageManager.activePage == oneOnOneHallGameSettingPage)
//        {
//            loginView.MultiGameSettingAddPlayer(gameHallPlayer);
//        }


    }

    void OnLeaveMultiGamePushSignal(JsonObject jo)
    {
        //int creator_id = int.Parse(jo["creator_id"].ToString());
        //int uid = int.Parse(jo["uid"].ToString());

        //gameHallDic.gameHallDic[creator_id].players_info.Remove(uid);

        //if (pageManager.activePage == oneOnOneHallGamesPage)
        //{
            
        //}
        //else if (pageManager.activePage == oneOnOneHallGameSettingPage)
        //{
        //    if (uid == sPlayerInfo.uid)
        //    {
        //        loginView.MultiGameInitList(gameHallDic);
        //        pageManager.JumpTo(oneOnOneHallGamesPage);
        //    }
        //    loginView.MultiGameSettingRemovePlayer(uid);
        //}

    }


    void OnMultiGameStartPushSignal(JsonObject jo)
    {
        netService.Request(NetService.LoadGame, null, (msg) =>
            {
                if (msg.code == 200)
                {
                    gameInfo.InitFromJson(msg.data as JsonObject);
                    startGameSignal.Dispatch();  
                }
            });
    }


    public void OnDestroy()
    {
        //game hall
        //createMultiGamePushSignal.RemoveListener(OnCreateMultiGamePushSignal);
        //cancelMultiGamePushSignal.RemoveListener(OnCancelMultiGamePushSignal);
        //joinMultiGamePushSignal.RemoveListener(OnJoinMultiGamePushSignal);
        //leaveMultiGamePushSignal.RemoveListener(OnLeaveMultiGamePushSignal);

        //game
        //game
        multiGameStartPushSignal.RemoveListener(OnMultiGameStartPushSignal);

        //loginView.playModeSignal.RemoveListener(OnPlayModeSignal);

        //loginView.singleMapSizeSignal.RemoveListener(OnSingleMapSizeSignal);

        //loginView.singleStartSignal.RemoveListener(OnSingleStartSignal);
    }

    //void OnGetServerList(List<NetService.ServerConnector> serverList)
    //{
    //    loginView.serverItemTpl.transform.SetParent(null);
    //    Tools.ClearChildren(loginView.serverSelectMenu.transform);
        

    //    foreach(NetService.ServerConnector server in serverList)
    //    {
    //        GameObject serverItem = GameObject.Instantiate(loginView.serverItemTpl as Object) as GameObject;
    //        serverItem.transform.SetParent(loginView.serverSelectMenu.transform);
    //        serverItem.transform.localPosition = Vector3.zero;
    //        serverItem.SetActive(true);

    //        Text serverName = serverItem.transform.FindChild("Text").GetComponent<Text>();
    //        serverName.text = server.name;

    //        serverItem.GetComponent<Button>().onClick.AddListener(() => {
    //            Debug.Log(server.name);
    //            this.server = server;

    //        });
    //    }
    //}
}
