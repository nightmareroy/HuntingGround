using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;
using SimpleJson;

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
            dGameDataCollection.dSkillCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DSkill.txt"));

            dGameDataCollection.dDirectionCollection = new DDirectionCollection();
            dGameDataCollection.dDirectionCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DDirection.txt"));


            //building
            dGameDataCollection.dBuildingCollection=new DBuildingCollection();
            dGameDataCollection.dBuildingCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DBuilding.txt"));

//            dGameDataCollection.dBuildingDirectionCollection=new DBuildingDirectionCollection();
//            dGameDataCollection.dBuildingDirectionCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DBuildingDirection.txt"));

            //map
            dGameDataCollection.dLandformCollection = new DLandformCollection();
            dGameDataCollection.dLandformCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DLandform.txt"));

            dGameDataCollection.dResourceCollection = new DResourceCollection();
            dGameDataCollection.dResourceCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DResource.txt"));

            dGameDataCollection.dMeatCollection = new DMeatCollection();
            dGameDataCollection.dMeatCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DMeat.txt"));

            //gametype
            dGameDataCollection.dGameTypeCollection=new DGameTypeCollection();
            dGameDataCollection.dGameTypeCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DGameType.txt"));

            //banana value
            dGameDataCollection.dBananaValueCollection=new DBananaValueCollection();
            dGameDataCollection.dBananaValueCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DBananaValue.txt"));

            //single game info
            dGameDataCollection.dSingleGameInfoCollection=new DSingleGameInfoCollection();
            dGameDataCollection.dSingleGameInfoCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DSingleGameInfo.txt"));

            //food
            dGameDataCollection.dCookSkillCollection = new DCookSkillCollection();
            dGameDataCollection.dCookSkillCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DCookSkill.txt"));

            dGameDataCollection.dFoodCollection = new DFoodCollection();
            dGameDataCollection.dFoodCollection.InitFromStr(fileIOService.ReadAllText("/Defaultdata/Data/DFood.txt"));

            callback();
        });


    }
}

