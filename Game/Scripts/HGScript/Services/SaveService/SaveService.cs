using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class SaveService
{
    [Inject]
    public FileIOService defaultDataIOService { get; set; }

    //GameInfo gameInfo;

    public GameInfo Load(string saveName)
    {
        return JsonUtility.FromJson<GameInfo>(defaultDataIOService.ReadAllText("/Save/" + saveName + ".txt"));
    }

    public void Save(GameInfo gameInfo,string saveName)
    {
        defaultDataIOService.WriteAllTxt(JsonUtility.ToJson(gameInfo),"/Save/"+saveName+".txt");
    }
}

