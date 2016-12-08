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

    int width;
    int height;
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
    public Page newGameSettingPage;
    //public Page serverSelectPage;

    //UpdateMapInfoSignal.Param updateMapInfoSignalParam;
    NetService.ServerConnector server=null;

    public override void OnRegister()
    {
        loginView.Init();

        InitPages();
        loginView.viewClick += OnViewClicked;

        //param = new StartGameSignal.Param();
        //updateMapInfoSignalParam=new UpdateMapInfoSignal.Param(
        //param.map_info.width = 20;
        //param.map_info.height = 40;
        //for (int i = 0; i < param.map_info.width * param.map_info.height; i++)
        //{
        //    param.map_info.landform_map.Add(0);
        //    param.map_info.resource_map.Add(0);
        //    param.map_info.explorestatus_map.Add(0);
        //}



        

        //loginView.playModeSignal.AddListener(OnPlayModeSignal);

        //loginView.singleMapSizeSignal.AddListener(OnSingleMapSizeSignal);

        //loginView.singleStartSignal.AddListener(OnSingleStartSignal);

        //loginView.onLoginClickedDelegate += OnLoginClick;
        //loginView.onRegisterClickedDelegate += OnRegisterClick;

        //getServerListSignal.Dispatch(OnGetServerList);

        //netService.testt();

        //return;
        netService.GetServerConnector((connector) =>
        {
            Debug.Log(connector.host + connector.port);
            netService.Connect(connector, () =>
            {
                Debug.Log("connect success!");
            });
        });
    }

    void InitPages()
    {
        //mainPage = new Page(loginView.mainMenu);
        //
        loginPage = new Page(loginView.loginMenu);
        registerPage = new Page(loginView.registerMenu);
        gamePage = new Page(loginView.gameMenu);
        newGameSettingPage = new Page(loginView.newGameSettingMenu);
        //serverSelectPage = new Page(loginView.serverSelectMenu);

        pageManager = new PageManager(loginPage);

        //mainPage.SetChild(singleSettingPage);
        //mainPage.SetChild(loginPage);
        //serverSelectPage.SetChild(loginPage);

        loginPage.SetChild(registerPage);
        loginPage.SetChild(gamePage);

        

        gamePage.SetChild(newGameSettingPage);
    }

    void OnViewClicked(string clickName)
    {
        switch (clickName)
        {
            case "Login":
                //loginSignal.Dispatch(new LoginSignal.Param(loginView.loginAccount.text, loginView.loginPwd.text));
                JsonObject user_login=new JsonObject();
                user_login["account"]=loginView.loginAccount.text;
                user_login["pwd"]=loginView.loginPwd.text;
                netService.Request(netService.loginRoute, user_login, (msg) => {
                    //Debug.Log(msg.rawString);

                    


                    if (msg.code == 200)
                    {
                        SPlayerInfo sPlayerInfo_temp=new SPlayerInfo();
                        sPlayerInfo_temp=SimpleJson.SimpleJson.DeserializeObject<SPlayerInfo>(msg.data.ToString());
                        
                        sPlayerInfo.UpdateSplayer(sPlayerInfo_temp);
                        //Debug.Log(sPlayerInfo);
                        //mainContext.injectionBinder.Unbind<SPlayerInfo>();
                        //mainContext.injectionBinder.Bind<SPlayerInfo>().ToValue(sPlayerInfo);

                        JsonObject user_isingame = new JsonObject();
                        netService.Request(netService.isingame, null, (msg2) =>
                        {
                            bool isingame = bool.Parse((msg2.data as JsonObject)["isingame"].ToString());
                            pageManager.JumpDown(gamePage);
                            loginView.SetLoadGameVisible(isingame);

                            //JsonObject user_startnew = new JsonObject();
                            ////user_startnew["gametype"] = 1;
                            ////user_startnew["mapsizeid"] = 1;
                            //netService.Request("game.gameHandler.test", null, (msg3) =>
                            //{

                            //});
                            
                        });
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
            case "ConfirmRegister":
                if (loginView.registerPwd.text != loginView.registerPwd2.text)
                {
                    Debug.Log("there two passwords are different..");
                    return;
                }
                JsonObject user_register=new JsonObject();
                user_register["account"]=loginView.registerAccount.text;
                user_register["pwd"]=loginView.registerPwd.text;
                netService.Request(netService.registerRoute, user_register, (msg) =>
                {
                    Debug.Log(msg.rawString);
                });

                //registerSignal.Dispatch(new RegisterSignal.Param(loginView.loginAccount.text, loginView.loginPwd.text));
                break;
            case "Big":
                break;
            case "Small":
                break;
            case "StartNewGame":
                break;
            case "Friend":
                break;
            case "Return":
                pageManager.JumpUp();
                break;
            case "LoadGame":
                //JsonObject user_loadgame = new JsonObject();
                //user_startnew["gametype"] = 1;
                //user_startnew["mapsizeid"] = 1;
                netService.Request(netService.loadgame, null, (msg) =>
                {
                    //Debug.Log(msg.rawString);
                    if (msg.code == 200)
                    {
                        gameInfo.InitFromJson(msg.data as JsonObject, sPlayerInfo);
                        startGameSignal.Dispatch();
                    }
                    
                });
                break;
            default:
                break;
        }
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

    public void OnDestroy()
    {


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
