using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SaveService
{
    [Inject]
    public StaticFileIOService staticFileIOService { get; set; }

    //GameInfo gameInfo;

    public GameInfo Load(string saveName)
    {
        return JsonUtility.FromJson<GameInfo>(staticFileIOService.ReadAllText("/Save/" + saveName + ".txt"));
    }

    public void Save(GameInfo gameInfo,string saveName)
    {
        staticFileIOService.WriteAllTxt(JsonUtility.ToJson(gameInfo),"/Save/"+saveName+".txt");
    }
}

