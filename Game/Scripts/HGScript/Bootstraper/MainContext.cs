using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using System.IO;
using System.Threading;
using Vectrosity;

public class MainContext : MVCSContext {

    public MapRootMediator mapRootMediator;
    public GameObject uiCanvas;
    public Camera gameCamera;
    public Camera uiCamera;
    public Camera overviewCamera;


	public MainContext (MonoBehaviour view) : base(view)
	{
        //父类Context类的构造函数会跳过本类构造函数，所以这里的代码不会执行，我也不知为啥要这样写
        //所以要获取view，必须从父类MVCSContext里获取，contextView已被MVCSContext类转换为GameObject类型。。
	}

    public MainContext(MonoBehaviour view, ContextStartupFlags flags)
        : base(view, flags)
	{
	}

    // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
    }

    protected override void mapBindings()
    {
        //MainContext
        injectionBinder.Bind<MainContext>().ToValue(this);

        //注册全局view
        injectionBinder.Bind<BootstrapView>().ToValue((contextView as GameObject).GetComponent<BootstrapView>());
        

        //绑定services
        bindServices();

        //绑定models
        bindModels();

        //绑定signals和commands
        bindSignalsAndCommands();
        
        //绑定mediators
        bindMediators();

        //绑定视图signal
        bindViewSignal();




        
    }


    void bindServices()
    {

        //资源
        injectionBinder.Bind<ResourceService>().ToSingleton();

        //场景
        injectionBinder.Bind<AsyncSceneService>().ToSingleton();

        //txt读写
        injectionBinder.Bind<FileIOService>().ToSingleton();

        //游戏动态数据
        injectionBinder.Bind<ActiveGameDataService>().ToSingleton();

        //网络
        injectionBinder.Bind<NetService>().ToSingleton();

        //单机存档
        injectionBinder.Bind<SaveService>().ToSingleton();

        //网络数据更新
        injectionBinder.Bind<NetDataUpdateService>().ToSingleton();

        //网络推送触发signal服务
        injectionBinder.Bind<NetPushSignalSerivice>().ToSingleton();

        //icon sprites
        injectionBinder.Bind<IconSpritesService>().ToSingleton();

        //color
        injectionBinder.Bind<ColorService>().ToSingleton();



    }


    void bindModels()
    {
        //静态数据集合
        injectionBinder.Bind<DGameDataCollection>().ToSingleton();


        //动态数据
        //splayerinfo
        injectionBinder.Bind<SPlayerInfo>().ToSingleton();
        //游戏数据
        GameInfo gameInfo = new GameInfo();
        injectionBinder.Bind<GameInfo>().ToValue(gameInfo);

        //游戏中途退出的玩家队列
        injectionBinder.Bind<GameStateChangeQueue>().ToSingleton();

        


    }

    void bindSignalsAndCommands()
    {

        //app
        commandBinder.Bind<StartAppSignal>().To<StartAppCommand>().Once();

        //loading
        injectionBinder.Bind<LoadingSignal>().ToSingleton();


        //register
        commandBinder.Bind<RegisterSignal>().To<RegisterCommand>();
        injectionBinder.Bind<RegisterCallbackCommand>().ToSingleton();

        //default game datas
        commandBinder.Bind<DefaultGameDataUpdateSignal>().To<DefaultGameDataUpdateCommand>().Once();

        //game
        commandBinder.Bind<StartGameSignal>().To<StartGameCommand>();
        injectionBinder.Bind<CreateNewGameCallbackSignal>().ToSingleton();
        commandBinder.Bind<EndGameSignal>().To<EndGameCommand>();

        //players
        commandBinder.Bind<AddPlayerSignal>().To<AddPlayerCommand>();
        injectionBinder.Bind<AddPlayerCallbackSignal>().ToSingleton();
        commandBinder.Bind<AddCountySkillSignal>().To<AddCountySkillCommand>();
        injectionBinder.Bind<AddCountySkillCallbackSignal>().ToSingleton();

        //roles
        //commandBinder.Bind<AddRoleSignal>().To<AddRoleCommand>();
        //injectionBinder.Bind<AddRoleCallbackSignal>().ToSingleton();
        //commandBinder.Bind<AddRoleSkillSignal>().To<AddRoleSkillCommand>();
        //injectionBinder.Bind<AddRoleSkillCallbackSignal>().ToSingleton();
        //commandBinder.Bind<SetRolePosSignal>().To<SetRolePosCommand>();
        //injectionBinder.Bind<SetRolePosCallbackSignal>().ToSingleton();

        //direction
//        commandBinder.Bind<DirectionClickSignal>().To<DirectionClickCommand>();
//        injectionBinder.Bind<DirectionClickCallbackSignal>().ToSingleton();
        injectionBinder.Bind<UpdateRoleDirectionSignal>().ToSingleton();

        //turn
        commandBinder.Bind<NextturnSignal>().To<NextturnCommand>();
        commandBinder.Bind<BroadcastActionSignal>().To<BroadcastActionCommand>();
        commandBinder.Bind<BroadcastSubActionSignal>().To<BroadcastSubActionCommand>();
        injectionBinder.Bind<DoRoleActionAnimSignal>().ToSingleton();
        injectionBinder.Bind<DoBuildingActionAnimSignal>().ToSingleton();
        injectionBinder.Bind<PlayerFailPushSignal>().ToSingleton();
        injectionBinder.Bind<DoMapUpdateSignal>().ToSingleton();
        injectionBinder.Bind<ActionAnimStartSignal>().ToSingleton();
        injectionBinder.Bind<ActionAnimFinishSignal>().ToSingleton();
        injectionBinder.Bind<DoMoneyUpdateSignal>().ToSingleton();
        injectionBinder.Bind<DoSightzoonUpdateSignal>().ToSingleton();
        commandBinder.Bind<FlowUpTipSignal>().To<FlowUpTipCommand>();
        injectionBinder.Bind<DoGroupGeneUpdateSignal>().ToSingleton();
        injectionBinder.Bind<UpdateWeightsSignal>().ToSingleton();
        injectionBinder.Bind<UpdateCurrentTurnSignal>().ToSingleton();
        injectionBinder.Bind<GameoverSignal>().ToSingleton();
        injectionBinder.Bind<UpdateWhenReturnToLogin>().ToSingleton();
        injectionBinder.Bind<UpdateNextturnTimeSignal>().ToSingleton();
        injectionBinder.Bind<UpdateRoleFaceSignal>().ToSingleton();
        injectionBinder.Bind<FindFreeRoleSignal>().ToSingleton();
        commandBinder.Bind<MsgBoxSignal>().To<MsgBoxCommand>();

        //gamehall push
        //injectionBinder.Bind<CreateMultiGamePushSignal>().ToSingleton();
        //injectionBinder.Bind<CancelMultiGamePushSignal>().ToSingleton();
        //injectionBinder.Bind<JoinMultiGamePushSignal>().ToSingleton();
        //injectionBinder.Bind<LeaveMultiGamePushSignal>().ToSingleton();

        //game push
        injectionBinder.Bind<MultiGameStartPushSignal>().ToSingleton();
        injectionBinder.Bind<NextTurnPushSignal>().ToSingleton();
        injectionBinder.Bind<UpdateDirectionTurnSignal>().ToSingleton();
        commandBinder.Bind<CheckGameStateQueueSignal>().To<CheckGameStateQueueCommand>();
        injectionBinder.Bind<UserStateChangeSignal>().ToSingleton();

        //friend push
        injectionBinder.Bind<InviteFightPushSignal>().ToSingleton();
        injectionBinder.Bind<CancelInviteFightPushSignal>().ToSingleton();
        injectionBinder.Bind<FriendGameStartPushSignal>().ToSingleton();
        injectionBinder.Bind<RefuseInviteFightPushSignal>().ToSingleton();

        //food
        injectionBinder.Bind<OpenFoodPanelSignal>().ToSingleton();

    }

    void bindViewSignal()
    {
        //injectionBinder.Bind<DirectionClickSignal>().ToSingleton();
        injectionBinder.Bind<MapNodeSelectSignal>().ToSingleton();
        injectionBinder.Bind<FindNodeSignal>().ToSingleton();
        //injectionBinder.Bind<UpdateDirectionPathSignal>().ToSingleton();
        //injectionBinder.Bind<RoleSelectSignal>().ToSingleton();
        //injectionBinder.Bind<PathSetFinishedSignal>().ToSingleton();
    }

    void bindMediators()
    {
        mediationBinder.Bind<TestBtnView>().To<TestBtnMediator>();
        mediationBinder.Bind<LoginView>().To<LoginMediator>();
        mediationBinder.Bind<MapRootView>().To<MapRootMediator>();
        mediationBinder.Bind<RoleView>().To<RoleMediator>();
        mediationBinder.Bind<BuildingView>().To<BuildingMediator>();
        mediationBinder.Bind<LoadingView>().To<LoadingMediator>();
        //mediationBinder.Bind<RoleUIView>().To<RoleUIMediator>();
        mediationBinder.Bind<NextturnView>().To<NextturnMediator>();
        mediationBinder.Bind<GameUIView>().To<GameUIMediator>();

        mediationBinder.Bind<PropertyPanelView>().To<PropertyPanelMediator>();
        mediationBinder.Bind<TopPanelView>().To<TopPanelMediator>();

        mediationBinder.Bind<MyRoleListPanelView>().To<MyRoleListPanelMediator>();
        mediationBinder.Bind<AllPlayerListPanelView>().To<AllPlayerListPanelMediator>();

        mediationBinder.Bind<FoodPanelView>().To<FoodPanelMediator>();

        mediationBinder.Bind<GameoverView>().To<GameoverMediator>();

        mediationBinder.Bind<TalkView>().To<TalkMediator>();
    }

    public override IContext Start()
    {
        base.Start();

        test();

        IconSpritesService iconSpritesService = injectionBinder.GetInstance<IconSpritesService>();
        VectorLine.SetEndCap("Arrow", EndCap.Front, iconSpritesService.GetView().lineMaterial, iconSpritesService.GetView().arrowStart, iconSpritesService.GetView().arrowEnd);

        //读取静态数据到内存
        injectionBinder.GetInstance<DefaultGameDataUpdateSignal>().Dispatch(() => 
        {
            StartAppSignal startSignal = injectionBinder.GetInstance<StartAppSignal>();
            startSignal.Dispatch();
            test2();
        });

        
        return this;
    }

    void test()
    {
        
        //injectionBinder.GetInstance<StaticFileIOService>().WriteAllTxt("12344444", "/123.txt", "/1/2/3");
        //string str = injectionBinder.GetInstance<StaticFileIOService>().ReadAllText("/123.txt", "/1/2/3");
        //Debug.Log(str);

        //injectionBinder.GetInstance<DefaultDataIOService>().SaveExampleData();
        //Debug.Log("streaming:" + Application.streamingAssetsPath);
        //Debug.Log("persistant:" + Application.persistentDataPath);
        //injectionBinder.GetInstance<DefaultDataIOService>().CopyFromStreamingToPersistant();
        

        //injectionBinder.GetInstance<StaticFileIOService>().WriteAllTxt("123","123.txt");
        //Debug.Log(injectionBinder.GetInstance<StaticFileIOService>().ReadAllText("123.txt"));
        //Debug.Log(Application.streamingAssetsPath);

//        injectionBinder.GetInstance<BootstrapView>().StartCoroutine(dl());
    }

    IEnumerator dl()
    {
        WWW www=new WWW("127.0.0.1:10001/turtle.png");
        yield return www;
        Texture2D tt = www.texture;
        byte[] bytes = tt.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath+"/test",bytes);
    }

    void test2()
    {
        //injectionBinder.GetInstance<NetService>().testt();
        //injectionBinder.GetInstance<NetService>().GetServerList((list) =>
        //{
        //    Debug.Log("objjjj" + list.Count);

        //    if (list.Count > 0)
        //    {
        //        string host = list[0].host;
        //        int port = list[0].port;
        //        Debug.Log("host:" + host + "port:" + port);
        //        injectionBinder.GetInstance<NetService>().Connect(host, port, (msg) =>
        //        {
        //            injectionBinder.GetInstance<NetService>().Request(injectionBinder.GetInstance<NetService>().registerRoute, null, (msg2) =>
        //            {
        //                Debug.Log(msg2.rawString);
        //            });
        //        });
        //    }
        //});
    }

    protected override void postBindings()
    {
        base.postBindings();
        injectionBinder.GetInstance<NetService>().Init();
    }
}
