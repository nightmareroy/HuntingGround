using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class DGameDataCollection
{
    //save
//    public SaveService dSave=new SaveService();

    //roles
    public DSkillCollection dSkillCollection;// = new DSkillCollection();
    public DDirectionCollection dDirectionCollection;// = new DDirectionCollection();

    //building
    public DBuildingCollection dBuildingCollection;

    //map
    public DLandformCollection dLandformCollection;// = new DLandformCollection();
    public DResourceCollection dResourceCollection;// = new DResourceCollection();
    public DMeatCollection dMeatCollection;

    //gametype
    public DGameTypeCollection dGameTypeCollection;//=new DGameTypeCollection();

    //banana value
    public DBananaValueCollection dBananaValueCollection;

    //single game info
    public DSingleGameInfoCollection dSingleGameInfoCollection;
    public DStoryTalkCollection dStoryTalkCollection;

    //food
    public DCookSkillCollection dCookSkillCollection;
    public DFoodCollection dFoodCollection;

}

