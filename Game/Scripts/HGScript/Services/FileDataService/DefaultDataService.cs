using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DefaultDataIOService
{
    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public StaticFileIOService staticFileIOService { get; set; }

    public void SaveExampleData()
    {
        ////droles
        //DRole drole = new DRole();
        //drole.id = 0;
        //drole.name = "测试角色";
        //dGameDataCollection.dRoleCollection.dRoleList.Add(drole);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dRoleCollection), "/DRole.txt");


        //DSkill dskill = new DSkill();
        //dskill.id = 0;
        //dskill.name = "移动";
        //dskill.isaction = true;
        //dGameDataCollection.dSkillCollection.dSkillList.Add(dskill);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dSkillCollection), "/DSkill.txt");


        ////dmap
        //DExploreStatus dexplorestatus = new DExploreStatus();
        //dexplorestatus.id = 0;
        //dexplorestatus.name = "可见";
        //dGameDataCollection.dExploreStatusCollection.dExploreStatusList.Add(dexplorestatus);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dExploreStatusCollection), "/DExploreStatus.txt");


        //DLandform dlandform = new DLandform();
        //dlandform.id = 0;
        //dlandform.name = "平原";
        //dlandform.movein_cost_base = 0;
        //dGameDataCollection.dLandformCollection.dLandformList.Add(dlandform);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dLandformCollection), "/DLandform.txt");

        //DResource dresource = new DResource();
        //dresource.id = 0;
        //dresource.name = "无资源";
        //dGameDataCollection.dResourceCollection.dResourceList.Add(dresource);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dResourceCollection), "/DResource.txt");

        ////dbuildings
        //DBuildingType dbuildingtype = new DBuildingType();
        //dbuildingtype.id = 0;
        //dbuildingtype.name = "主基地";
        //dGameDataCollection.dBuildingTypeCollection.dBuildingTypeList.Add(dbuildingtype);
        //staticFileIOService.WriteAllTxt(JsonUtility.ToJson(dGameDataCollection.dBuildingTypeCollection), "/DBuildingType.txt");

    }

    public void CopyFromStreamingToPersistant()
    {
        staticFileIOService.CopyFromResourceToPersistant("DRole");
        staticFileIOService.CopyFromResourceToPersistant("DSkill");
        staticFileIOService.CopyFromResourceToPersistant("DExploreStatus");
        staticFileIOService.CopyFromResourceToPersistant("DLandform");
        staticFileIOService.CopyFromResourceToPersistant("DResource");
        staticFileIOService.CopyFromResourceToPersistant("DBuildingType");
        staticFileIOService.CopyFromResourceToPersistant("DDirection");
    }
}

