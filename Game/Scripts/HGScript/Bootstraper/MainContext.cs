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
        //静态文件读写
        injectionBinder.Bind<StaticFileIOService>().ToSingleton();

        //资源
        injectionBinder.Bind<ResourceService>().ToSingleton();

        //场景
        injectionBinder.Bind<AsyncSceneService>().ToSingleton();

        //静态数据
        injectionBinder.Bind<DefaultDataIOService>().ToSingleton();

        //游戏数据
        injectionBinder.Bind<GameDataService>().ToSingleton();

        //网络
        injectionBinder.Bind<NetService>().ToSingleton();

        //单机存档
        injectionBinder.Bind<SaveService>().ToSingleton();

    }


    void bindModels()
    {
        //静态数据集合
        injectionBinder.Bind<DGameDataCollection>().ToSingleton();


        //动态数据
        //splayerinfo
        injectionBinder.Bind<SPlayerInfo>().ToSingleton();
        //单人游戏数据
        GameInfo gameInfo = new GameInfo();
        injectionBinder.Bind<GameInfo>().ToValue(gameInfo);
        //Debug.Log(JsonUtility.ToJson(gameInfo));

        


    }

    void bindSignalsAndCommands()
    {
        //test
        commandBinder.Bind<TestSignal>().To<TestCommand>();

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
        commandBinder.Bind<DirectionClickSignal>().To<DirectionClickCommand>();
        injectionBinder.Bind<DirectionClickCallbackSignal>().ToSingleton();
        commandBinder.Bind<UpdateDirectionPathSignal>().To<UpdateDirectionPathCommand>();
        injectionBinder.Bind<UpdateDirectionPathCallbackSignal>().ToSingleton();

        //turn
        commandBinder.Bind<NextturnSignal>().To<NextturnCommand>();
        commandBinder.Bind<BroadcastActionSignal>().To<BroadcastActionCommand>();
        injectionBinder.Bind<DoActionAnimSignal>().ToSingleton();
        injectionBinder.Bind<DoMapUpdateSignal>().ToSingleton();
        injectionBinder.Bind<ActionAnimStartSignal>().ToSingleton();
        injectionBinder.Bind<ActionAnimFinishSignal>().ToSingleton();
        

    }

    void bindViewSignal()
    {
        //injectionBinder.Bind<DirectionClickSignal>().ToSingleton();
        injectionBinder.Bind<MapNodeSelectSignal>().ToSingleton();
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
        mediationBinder.Bind<LoadingView>().To<LoadingMediator>();
        //mediationBinder.Bind<RoleUIView>().To<RoleUIMediator>();
        mediationBinder.Bind<NextturnView>().To<NextturnMediator>();
    }

    public override IContext Start()
    {
        base.Start();

        test();



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
        ;
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
