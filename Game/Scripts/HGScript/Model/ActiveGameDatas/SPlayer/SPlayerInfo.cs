using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SPlayerInfo
{
    public int uid=-1;
    public string account="";
    //public int current_game_id;
    public int win_count;
    public int failure_count;
    public string name="";      
    public int single_game_progress=1;

    public void UpdateSplayer(SPlayerInfo sPlayerInfo)
    {
        this.uid = sPlayerInfo.uid;
        this.account = sPlayerInfo.account;
        this.win_count = sPlayerInfo.win_count;
        this.failure_count = sPlayerInfo.failure_count;
        this.name = sPlayerInfo.name;
        this.single_game_progress = sPlayerInfo.single_game_progress;
    }

    public void ClearSPlayer()
    {
        uid = -1;
        account="";
        win_count = 0;
        failure_count = 0;
        name = "";
        single_game_progress = 1;
    }

    //public SPlayerInfo(int id,string name)
    //{
    //    this.id = id;
    //    this.name = name;
    //}
}

