using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using UnityEngine;


public class AddCountySkillCommand:Command
{
    [Inject]
    public AddCountySkillSignal.Param param { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public AddCountySkillCallbackSignal addCountySkillCallbackSignal { get; set; }

    public override void Execute()
    {
        //if (!gameInfo.allplayers_dic[param.player_id].country_skill_dic.ContainsKey(param.role_did))
        //    gameInfo.allplayers_dic[param.player_id].country_skill_dic.Add(param.role_did, new List<int>());
        //gameInfo.allplayers_dic[param.player_id].country_skill_dic[param.role_did].Add(param.skill_id);

        addCountySkillCallbackSignal.Dispatch();
    }
}

