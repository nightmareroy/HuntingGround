using System;
using System.Collections.Generic;
using UnityEngine;

public class DGameDataCollection
{
    //save
    public SaveService dSave=new SaveService();

    //roles
    public DRoleCollection dRoleCollection = new DRoleCollection();
    public DSkillCollection dSkillCollection = new DSkillCollection();
    public DDirectionCollection dDirectionCollection = new DDirectionCollection();
    

    //map
    public DExploreStatusCollection dDetectiveStatusCollection = new DExploreStatusCollection();
    public DLandformCollection dLandformCollection = new DLandformCollection();
    public DResourceCollection dResourceCollection = new DResourceCollection();
    

    //buildings
    public DBuildingTypeCollection dBuildingTypeCollection = new DBuildingTypeCollection();
    
    
}

