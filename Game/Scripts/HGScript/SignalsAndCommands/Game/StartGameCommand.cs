using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;


public class StartGameCommand : Command
{
    //[Inject]
    //public StartGameSignal.Param param { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public AsyncSceneService asyncSceneService { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public IconSpritesService iconSpritesService { get; set; }

    //[Inject]
    //public AddGamePlayerSignal addGamePlayerSignal { get; set; }

    //[Inject]
    //public AddRoleSignal addRoleSignal { get; set; }

    //[Inject]
    //public SaveService saveService { get;set; }

    public override void Execute()
    {
        //gameInfo.map_info = param.map_info;
        //gameInfo.allplayers = param.allplayers;


        //gameInfo.map_info.width = param.width;
        //gameInfo.map_info.height = param.height;

        //for (int i = 0; i < param.width * param.height; i++)
        //{
        //    gameInfo.map_info.landform_map.Add(0);
        //    gameInfo.map_info.resource_map.Add(0);
        //    gameInfo.map_info.explorestatus_map.Add(0);
        //}



        //if(param.dataCallback!=null)
        //    param.dataCallback();
        asyncSceneService.LoadScene("Game", () =>
        {
            //AddRoleSignal.Param addRoleParam=new AddRoleSignal.Param();
            //RoleInfo uplayerInfo=new GamePlayerInfo();
            //uplayerInfo.id=-1;
            //addRoleParam.roleInfo = uplayerInfo;
            //addRoleParam.isuplayer=true;
            //addRoleParam.playerid=-1;

            //AddGamePlayerSignal.Param addGamePlayerParam=new AddGamePlayerSignal.Param();
            //addGamePlayerParam.gamePlayerInfo=new GamePlayerInfo();
            //addGamePlayerParam.gamePlayerInfo.id
            //addGamePlayerSignal.Dispatch(
            
            //addRoleSignal.Dispatch(addRoleParam);
            //GameObject testrole = resourceManager.Spawn(ResourcePoolInfo.PoolName.testrole);
            //testrole.name = "-1_0";

            mainContext.uiCanvas = GameObject.Find("UICanvas");
            mainContext.gameCamera = GameObject.Find("GameAndUICamerasRoot/GameCamera").GetComponent<Camera>();
            mainContext.uiCamera = GameObject.Find("GameAndUICamerasRoot/UICamera").GetComponent<Camera>();
            mainContext.overviewCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            //if(param.loadlevelCallback!=null)
            //    param.loadlevelCallback();



            //预先实例化IconSpritesView
            iconSpritesService.GetView();
        });

    }

}

