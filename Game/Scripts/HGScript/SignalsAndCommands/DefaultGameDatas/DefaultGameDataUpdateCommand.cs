using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;
using System.IO;

public class DefaultGameDataUpdateCommand:Command
{
    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public FileIOService fileIOService { get; set; }

    [Inject]
    public Action callback { get; set; }

    [Inject]
    public NetService netService{ get; set;}

    [Inject]
    public NetDataUpdateService netDataUpdateService{ get; set;}

    public override void Execute()
    {
        
        netDataUpdateService.StartUpdate(()=>{
            //roles
//            dGameDataCollection.dRoleCollection = new DRoleCollection();
//            dGameDataCollection.dRoleCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DRole.txt"));
            

            dGameDataCollection.dSkillCollection = new DSkillCollection();
            dGameDataCollection.dSkillCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DSkill.txt"));

            dGameDataCollection.dDirectionCollection = new DDirectionCollection();
            dGameDataCollection.dDirectionCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DDirection.txt"));


            //building
            dGameDataCollection.dBuildingCollection=new DBuildingCollection();
            dGameDataCollection.dBuildingCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DBuilding.txt"));

//            dGameDataCollection.dBuildingDirectionCollection=new DBuildingDirectionCollection();
//            dGameDataCollection.dBuildingDirectionCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DBuildingDirection.txt"));

            //map
            dGameDataCollection.dLandformCollection = new DLandformCollection();
            dGameDataCollection.dLandformCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DLandform.txt"));

            dGameDataCollection.dResourceCollection = new DResourceCollection();
            dGameDataCollection.dResourceCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DResource.txt"));

            dGameDataCollection.dMeatCollection = new DMeatCollection();
            dGameDataCollection.dMeatCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DMeat.txt"));

            //gametype
            dGameDataCollection.dGameTypeCollection=new DGameTypeCollection();
            dGameDataCollection.dGameTypeCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DGameType.txt"));

            //banana value
            dGameDataCollection.dBananaValueCollection=new DBananaValueCollection();
            dGameDataCollection.dBananaValueCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DBananaValue.txt"));

            //single game info
            dGameDataCollection.dSingleGameInfoCollection=new DSingleGameInfoCollection();
            dGameDataCollection.dSingleGameInfoCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DSingleGameInfo.txt"));

            dGameDataCollection.dStoryTalkCollection = new DStoryTalkCollection();
            dGameDataCollection.dStoryTalkCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DStoryTalk.txt"));

            //food
            dGameDataCollection.dCookSkillCollection = new DCookSkillCollection();
            dGameDataCollection.dCookSkillCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DCookSkill.txt"));

            dGameDataCollection.dFoodCollection = new DFoodCollection();
            dGameDataCollection.dFoodCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DFood.txt"));

            //help
            dGameDataCollection.dHelpCollection = new DHelpCollection();
            dGameDataCollection.dHelpCollection.InitFromStr(fileIOService.ReadAllText("/DefaultData/Data/DHelp.txt"));

            callback();
        });


    }
}

