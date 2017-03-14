using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using SimpleJson;
using UnityEngine;

public class NextturnCommand:Command
{
    [Inject]
    public Action<bool> callback { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public NetService netService { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal{ get; set;}

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal{ get; set;}

    public override void Execute()
    {
        //gameInfo.anim_lock++;
        actionAnimStartSignal.Dispatch();

        mapNodeSelectSignal.Dispatch(null);

        JsonObject user = new JsonObject();
        JsonObject direction = new JsonObject();
        user.Add("direction", direction);

        Dictionary<string, RoleInfo> role_dic = gameInfo.role_dic;
        foreach (string role_id in role_dic.Keys)
        {
            if (role_dic[role_id].uid == sPlayerInfo.uid)
            {
                JsonObject roledirectioninfo = new JsonObject();
                roledirectioninfo.Add("direction_did", role_dic[role_id].direction_did);
//                roledirectioninfo.Add("direction_param", SimpleJson.SimpleJson.SerializeObject(role_dic[roleid].direction_param));
                roledirectioninfo.Add("direction_param", role_dic[role_id].direction_param);
                direction.Add(role_id.ToString(), roledirectioninfo);
//                Debug.Log(role_id+",,,"+role_dic[role_id].direction_param[0]);
            }
        }

        user.Add("current_turn", gameInfo.current_turn);


        netService.Request(NetService.NextTurn, user, (msg) => {
//            Debug.Log(msg.rawString);
            //gameInfo.anim_lock--;
            //actionAnimFinishSignal.Dispatch();
            callback((bool)msg.data);
        });
    }
}

