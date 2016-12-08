using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;

public class DefaultGameDataUpdateCommand:Command
{
    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public StaticFileIOService staticFileIOService { get; set; }

    [Inject]
    public Action callback { get; set; }

    public override void Execute()
    {
        

        //roles
        dGameDataCollection.dRoleCollection = JsonUtility.FromJson<DRoleCollection>(staticFileIOService.ReadAllText("/DRole.txt"));
        foreach (DRole drole in dGameDataCollection.dRoleCollection.dRoleList)
        {
            dGameDataCollection.dRoleCollection.dRoleDic.Add(drole.id,drole);
        }

        dGameDataCollection.dSkillCollection = JsonUtility.FromJson<DSkillCollection>(staticFileIOService.ReadAllText("/DSkill.txt"));
        foreach (DSkill dskill in dGameDataCollection.dSkillCollection.dSkillList)
        {
            dGameDataCollection.dSkillCollection.dSkillDic.Add(dskill.id,dskill);
        }
        dGameDataCollection.dDirectionCollection = JsonUtility.FromJson<DDirectionCollection>(staticFileIOService.ReadAllText("/DDirection.txt"));
        foreach (DDirection ddirection in dGameDataCollection.dDirectionCollection.dDirectionList)
        {
            dGameDataCollection.dDirectionCollection.dDirectionDic.Add(ddirection.id, ddirection);
        }

        //map
        dGameDataCollection.dDetectiveStatusCollection = JsonUtility.FromJson<DExploreStatusCollection>(staticFileIOService.ReadAllText("/DDetectiveStatus.txt"));
        foreach (DExploreStatus ddetectivestatus in dGameDataCollection.dDetectiveStatusCollection.dExploreStatusList)
        {
            dGameDataCollection.dDetectiveStatusCollection.dDetectiveStatusDic.Add(ddetectivestatus.id, ddetectivestatus);
        }

        dGameDataCollection.dLandformCollection = JsonUtility.FromJson<DLandformCollection>(staticFileIOService.ReadAllText("/DLandform.txt"));
        foreach (DLandform dlandform in dGameDataCollection.dLandformCollection.dLandformList)
        {
            dGameDataCollection.dLandformCollection.dLandformDic.Add(dlandform.id, dlandform);
        }

        dGameDataCollection.dResourceCollection = JsonUtility.FromJson<DResourceCollection>(staticFileIOService.ReadAllText("/DResource.txt"));
        foreach (DResource dresource in dGameDataCollection.dResourceCollection.dResourceList)
        {
            dGameDataCollection.dResourceCollection.dResourceDic.Add(dresource.id, dresource);
        }

        //buildings
        dGameDataCollection.dBuildingTypeCollection = JsonUtility.FromJson<DBuildingTypeCollection>(staticFileIOService.ReadAllText("/DBuildingType.txt"));
        foreach (DBuildingType dbuildingtype in dGameDataCollection.dBuildingTypeCollection.dBuildingTypeList)
        {
            dGameDataCollection.dBuildingTypeCollection.dBuildingTypeDic.Add(dbuildingtype.id, dbuildingtype);
        }

        callback();
    }
}

