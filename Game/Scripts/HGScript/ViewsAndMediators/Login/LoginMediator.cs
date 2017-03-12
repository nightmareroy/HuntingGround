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
    [Inject]
    public CreateMultiGamePushSignal createMultiGamePushSignal{ get; set;}
    [Inject]
    public CancelMultiGamePushSignal cancelMultiGamePushSignal{ get; set;}
    [Inject]
    public JoinMultiGamePushSignal joinMultiGamePushSignal{ get; set;}
    [Inject]
    public LeaveMultiGamePushSignal leaveMultiGamePushSignal{ get; set;}

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
    public Page gameTypePage;
    public Page multiGameSettingPage;
	public Page multiGamesPage;
    public Page singleGamePage;
    //public Page serverSelectPage;

    //UpdateMapInfoSignal.Param updateMapInfoSignalParam;
    NetService.ServerConnector server=null;

    GameHallDic gameHallDic=new GameHallDic();

    int multi_game_type_id;

    public override void OnRegister()
    {
        loginView.Init();

        InitPages();
        loginView.viewClick += OnViewClicked;

        //game hall
        createMultiGamePushSignal.AddListener(OnCreateMultiGamePushSignal);
        cancelMultiGamePushSignal.AddListener(OnCancelMultiGamePushSignal);
        joinMultiGamePushSignal.AddListener(OnJoinMultiGamePushSignal);
        leaveMultiGamePushSignal.AddListener(OnLeaveMultiGamePushSignal);

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
        gameTypePage = new Page(loginView.gameTypeMenu);
        multiGameSettingPage = new Page(loginView.multiGameSettingMenu);
        multiGamesPage = new Page(loginView.multiGamesMenu);
        singleGamePage = new Page(loginView.singleGameMenu);

        pageManager = new PageManager(loginPage);

        //mainPage.SetChild(singleSettingPage);
        //mainPage.SetChild(loginPage);
        //serverSelectPage.SetChild(loginPage);

        loginPage.SetChild(registerPage);
        loginPage.SetChild(gamePage);

        gamePage.SetChild(singleGamePage);
        gamePage.SetChild(multiGamesPage);

        multiGamesPage.SetChild(gameTypePage);

        gameTypePage.SetChild(multiGameSettingPage);




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
                        bool isingame=bool.Parse((msg.data as JsonObject)["isingame"].ToString());

                        SPlayerInfo sPlayerInfo_temp=new SPlayerInfo();
                        sPlayerInfo_temp=SimpleJson.SimpleJson.DeserializeObject<SPlayerInfo>(splayerObj.ToString());
                        sPlayerInfo.UpdateSplayer(sPlayerInfo_temp);

                        pageManager.JumpDown(gamePage);
                        loginView.SetLoadGameVisible(isingame);
                        //Debug.Log(sPlayerInfo);
                        //mainContext.injectionBinder.Unbind<SPlayerInfo>();
                        //mainContext.injectionBinder.Bind<SPlayerInfo>().ToValue(sPlayerInfo);

//                        JsonObject user_isingame = new JsonObject();
//                        netService.Request(NetService.isingame, null, (msg2) =>
//                        {
//                            bool isingame = bool.Parse((msg2.data as JsonObject)["isingame"].ToString());
//                            pageManager.JumpDown(gamePage);
//                            loginView.SetLoadGameVisible(isingame);
//                            
//                        });
                        //
                    }
                    else if (msg.code == 500)
                    {
                        Debug.LogError("error!");
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
                    Debug.Log(msg.rawString);
                });

                //registerSignal.Dispatch(new RegisterSignal.Param(loginView.loginAccount.text, loginView.loginPwd.text));
                break;

                //game
            case "CreateSingleGame":
                pageManager.JumpDown(singleGamePage,()=>{
                    loginView.InitSingleGameBtns((progress_id)=>{
                        JsonObject form=new JsonObject();
                        form.Add("progress_id",progress_id);
                        netService.Request(NetService.create_single_game,form,(msg)=>{
                            if(msg.code==200)
                            {
                                netService.Request(NetService.LoadGame, null, (msg2) =>
                                    {
                                        if (msg2.code == 200)
                                        {
                                            gameInfo.InitFromJson(msg2.data as JsonObject);
                                            startGameSignal.Dispatch();  
                                        }
                                    });
                            }
                        });
                    });
                });
                break;
            case "MultiGames":
                netService.Request(NetService.enterGameHall,null,(msg)=>{
                    if(msg.code==200)
                    {
                        gameHallDic.InitFromJson(msg.data as JsonObject);
                        loginView.MultiGameInitList(gameHallDic);
                        pageManager.JumpDown(multiGamesPage);
                    }
                });
                break;
            case "LoadGame":
                netService.Request(NetService.LoadGame, null, (msg) =>
                    {
                        //Debug.Log(msg.rawString);
                        if (msg.code == 200)
                        {
                            gameInfo.InitFromJson(msg.data as JsonObject);
                            startGameSignal.Dispatch();                 
                        }

                    });
                break;
            case "Friend":
                Debug.Log("friend list");
                break;

                //multi game
            case "CreateMultiGame":
                pageManager.JumpTo(gameTypePage);
                break;
            case "JoinGame":
                if (loginView.selectedCreatorId != -1)
                {
                    JsonObject form = new JsonObject();
                    form.Add("creator_id",loginView.selectedCreatorId);
                    netService.Request(NetService.join_multi_game,form,(msg)=>{
                        
                    });
                }

                
                break;
//            case "ReturnGameSetting":
//                JsonObject form_leave_c = new JsonObject();
//                netService.Request(NetService.return_gamesetting,form_leave_c,(msg_return_c)=>{
//                    if(msg_return_c.code==200)
//                        pageManager.JumpUp();
//                });
//                break;

                //gametype
            case "1v1":

                JsonObject form_1v1 = new JsonObject();
                form_1v1.Add("gametype_id",2);
                netService.Request(NetService.create_multi_game,form_1v1,(msg_1v1)=>{
//                    if(msg_1v1.code==200)
//                        pageManager.JumpTo(multiGameSettingPage);
                });

                break;
            case "2v2":
                JsonObject form_2v2 = new JsonObject();
                form_2v2.Add("gametype_id",3);
                netService.Request(NetService.create_multi_game,form_2v2,(msg_2v2)=>{
//                    if(msg_2v2.code==200)
//                        pageManager.JumpTo(multiGameSettingPage);
                });
                break;

                //multi game setting
            case "StartMultiGame":
                netService.Request(NetService.multi_game_start,null,(msg)=>{
                    
                });
                break;
            case "CancelOrLeaveMultiGame":
                netService.Request(NetService.cancel_or_leave_multi_game,null,(msg)=>{
                    
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
        GameHallGame gameHallGame = new GameHallGame();
        gameHallGame.InitFromJson(jo);
        gameHallDic.gameHallDic.Add(gameHallGame.creator_id,gameHallGame);

        if (gameHallGame.creator_id==sPlayerInfo.uid)
        {
//            int groupCount = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameHallGame.gametype_id].playercount_in_group.Count;
//            loginView.MultiGameSettingSetGroupCount(groupCount);
//            loginView.MultiGameSettingAddPlayer(gameHallGame.players_info[gameHallGame.creator_id]);
            loginView.MultiGameSettingInit(gameHallGame);
            pageManager.JumpTo(multiGameSettingPage);
        }

        if (pageManager.activePage == multiGamesPage)
        {
            loginView.MultiGameAddGame(gameHallGame);
        }

//        switch (pageManager.activePage)
//        {
//            case multiGamesPage:
//                loginView.MultiGameAddGame(gameHallGame);
//                break;
//            case multiGameSettingPage:
//                break;
//        }

    }

    void OnCancelMultiGamePushSignal(JsonObject jo)
    {
        int creator_id = int.Parse(jo["creator_id"].ToString());
        bool is_start = bool.Parse(jo["is_start"].ToString());

        GameHallGame gameHallGame = gameHallDic.gameHallDic[creator_id];
        gameHallDic.gameHallDic.Remove(creator_id);

        if (pageManager.activePage == multiGamesPage)
        {
            loginView.MultiGameRemoveGame(creator_id);
        }
        else if (pageManager.activePage == multiGameSettingPage)
        {
            if (is_start == false)
            {
                if (gameHallGame.players_info.ContainsKey(sPlayerInfo.uid))
                {
                    loginView.MultiGameInitList(gameHallDic);
                    pageManager.JumpTo(multiGamesPage);
                }
            }
        }
//        switch (pageManager.activePage)
//        {
//            case multiGamesPage:
//                loginView.MultiGameRemoveGame(creator_id);
//                break;
//            case multiGameSettingPage:
//                if (gameHallDic.gameHallDic[creator_id].players_info.ContainsKey(sPlayerInfo.uid))
//                {
//                    loginView.MultiGameInitList(gameHallDic);
//                    pageManager.JumpUp(multiGamesPage);
//                }
//                break;
//        }

    }

    void OnJoinMultiGamePushSignal(JsonObject jo)
    {
        int creator_id = int.Parse(jo["creator_id"].ToString());
        int uid = int.Parse(jo["uid"].ToString());

        GameHallPlayer gameHallPlayer = new GameHallPlayer();
        gameHallPlayer.uid = uid;
        gameHallPlayer.group_id = int.Parse(jo["group_id"].ToString());
        gameHallPlayer.name = jo["name"].ToString();


        gameHallDic.gameHallDic[creator_id].players_info.Add(uid,gameHallPlayer);

        if (pageManager.activePage == multiGamesPage)
        {
            if (uid == sPlayerInfo.uid)
            {
                loginView.MultiGameSettingInit(gameHallDic.gameHallDic[creator_id]);
//                loginView.MultiGameSettingAddPlayer(gameHallPlayer);
                pageManager.JumpTo(multiGameSettingPage);
            }
        }
        else if (pageManager.activePage == multiGameSettingPage)
        {
            loginView.MultiGameSettingAddPlayer(gameHallPlayer);
        }

//        switch (pageManager.activePage)
//        {
//            case multiGamesPage:
//                if (uid == sPlayerInfo.uid)
//                {
//                    loginView.MultiGameSettingAddPlayer(gameHallPlayer);
//                    pageManager.JumpDown(multiGameSettingPage);
//                }
//                break;
//            case multiGameSettingPage:
//                loginView.MultiGameSettingAddPlayer(gameHallPlayer);
//                break;
//        }
    }

    void OnLeaveMultiGamePushSignal(JsonObject jo)
    {
        int creator_id = int.Parse(jo["creator_id"].ToString());
        int uid = int.Parse(jo["uid"].ToString());

        gameHallDic.gameHallDic[creator_id].players_info.Remove(uid);

        if (pageManager.activePage == multiGamesPage)
        {
            
        }
        else if (pageManager.activePage == multiGameSettingPage)
        {
            if (uid == sPlayerInfo.uid)
            {
                loginView.MultiGameInitList(gameHallDic);
                pageManager.JumpTo(multiGamesPage);
            }
            loginView.MultiGameSettingRemovePlayer(uid);
        }

//        switch (pageManager.activePage)
//        {
//            case multiGamesPage:
//                break;
//            case multiGameSettingPage:
//                if (uid == sPlayerInfo.uid)
//                {
//                    loginView.MultiGameInitList(gameHallDic);
//                    pageManager.JumpUp(multiGamesPage);
//                }
//                loginView.MultiGameSettingRemovePlayer(uid);
//                break;
//        }
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
        createMultiGamePushSignal.RemoveListener(OnCreateMultiGamePushSignal);
        cancelMultiGamePushSignal.RemoveListener(OnCancelMultiGamePushSignal);
        joinMultiGamePushSignal.RemoveListener(OnJoinMultiGamePushSignal);
        leaveMultiGamePushSignal.RemoveListener(OnLeaveMultiGamePushSignal);

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
