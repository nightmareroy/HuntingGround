using System;
using System.Collections.Generic;
using strange.extensions.command.impl;
using SimpleJson;

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

    public override void Execute()
    {
        JsonObject user = new JsonObject();
        JsonObject direction = new JsonObject();
        user.Add("direction", direction);

        Dictionary<int, RoleInfo> role_dic = gameInfo.role_dic;
        foreach (int roleid in role_dic.Keys)
        {
            if (role_dic[roleid].uid == sPlayerInfo.uid)
            {
                JsonObject roledirectioninfo = new JsonObject();
                roledirectioninfo.Add("directionid", role_dic[roleid].direction_id);
                roledirectioninfo.Add("directionpath", SimpleJson.SimpleJson.SerializeObject(role_dic[roleid].direction_path));
                direction.Add(roleid.ToString(), roledirectioninfo);
            }
        }

        user.Add("current_turn", gameInfo.current_turn);


        netService.Request(netService.nextturn, user, (msg) => {
            callback((bool)msg.data);
        });
    }
}

