using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPlayerInfo
{
    public int uid=-1;
    public string account;
    //public int current_game_id;
    public int win_count;
    public int failure_count;
    //public string name="";

    public void UpdateSplayer(SPlayerInfo sPlayerInfo)
    {
        this.uid = sPlayerInfo.uid;
        this.account = sPlayerInfo.account;
        this.win_count = sPlayerInfo.win_count;
        this.failure_count = sPlayerInfo.failure_count;
    }

    //public SPlayerInfo(int id,string name)
    //{
    //    this.id = id;
    //    this.name = name;
    //}
}

