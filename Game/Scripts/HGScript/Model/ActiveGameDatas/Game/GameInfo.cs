using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;


//[Serializable]
public class GameInfo//  : ISerializationCallbackReceiver
{

    //List<int> _keys = new List<int>();
    //List<int> _values = new List<int>();

    //public int gameid;
    public int gametype;
    public int current_turn = 0;
    public int creatorid;
    //public List<int> playercount_in_group=new List<int>();
    //public List<int> npccount_in_group=new List<int>();


    public MapInfo map_info=new MapInfo();
    //public GamePlayerInfo uplayer;//=new GamePlayerInfo();
    public Dictionary<int, PlayerInfo> allplayers_dic = new Dictionary<int, PlayerInfo>();
    public Dictionary<int, RoleInfo> role_dic = new Dictionary<int, RoleInfo>();
    //public List<PlayerInfo> allplayers_list=new List<PlayerInfo>();

    
    //public Dictionary<int, DirectionInfo> curr_direction_dic = new Dictionary<int, DirectionInfo>(); 
    //public List<DirectionInfo> curr_direction_list=new List<DirectionInfo>();

    public void UpdateGameInfo(GameInfo gameInfo)
    {
        //this.gameid = gameInfo.gameid;
        this.map_info = gameInfo.map_info;
        this.allplayers_dic = gameInfo.allplayers_dic;
        //this.allplayers_list = gameInfo.allplayers_list;
        this.current_turn = gameInfo.current_turn;
        //this.curr_direction_dic = gameInfo.curr_direction_dic;
        //this.curr_direction_list = gameInfo.curr_direction_list;
    }

    public void InitFromJson(JsonObject jsonobj, SPlayerInfo sPlayerInfo)
    {
        this.gametype = int.Parse(jsonobj["gametype"].ToString());
        this.current_turn = int.Parse(jsonobj["current_turn"].ToString());
        this.creatorid = int.Parse(jsonobj["creatorid"].ToString());
        this.map_info = SimpleJson.SimpleJson.DeserializeObject<MapInfo>(jsonobj["mapinfo"].ToString());

        allplayers_dic.Clear();
        foreach (string key in (jsonobj["allplayers_dic"] as JsonObject).Keys)
        {
            PlayerInfo playerInfo=new PlayerInfo();
            playerInfo.InitFromJson((jsonobj["allplayers_dic"] as JsonObject)[key] as JsonObject);
            allplayers_dic.Add(int.Parse(key), playerInfo);
        }

        role_dic.Clear();
        foreach (string key in (jsonobj["role_dic"] as JsonObject).Keys)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.InitFromJson((jsonobj["role_dic"] as JsonObject)[key] as JsonObject);
            role_dic.Add(roleInfo.roleid, roleInfo);
        }
    }



    //public GameInfo(MapInfo mapInfo, GamePlayerInfo user, Dictionary<int, GamePlayerInfo> opponents)
    //{
    //    this.mapInfo = mapInfo;
    //    this.user = user;
    //    this.opponents = opponents;



    //}

    //public void SetParam(MapSize mapSize)
    //{
    //    switch (mapSize)
    //    {
    //        case MapSize.small:
    //            mapInfo.width = 20;
    //            mapInfo.height = 40;
    //            break;
    //        case MapSize.big:
    //            mapInfo.width = 30;
    //            mapInfo.height = 60;
    //            break;
    //    }


    //    //测试数据
    //    GamePlayerInfo gamePlayer = new GamePlayerInfo();
    //    playerList.Add(gamePlayer);

    //    gamePlayer.id = 0;
    //    gamePlayer.roleList = new List<RoleInfo>();

    //    RoleInfo role = new RoleInfo();
    //    gamePlayer.roleList.Add(role);

    //    role.id = 0;
    //    role.did = 0;
    //    role.pos_x = 10;
    //    role.pos_y = 5;
    //}

    //public void AddPlayer(PlayerInfo player)
    //{
    //    playerList.Add(player);
    //}




    //public void OnBeforeSerialize()
    //{
    //    allplayers_list.Clear();
    //    foreach (PlayerInfo playerInfo in allplayers_dic.Values)
    //    {
    //        allplayers_list.Add(playerInfo);
    //    }

    //    curr_direction_list.Clear();
    //    foreach (DirectionInfo directionInfo in curr_direction_dic.Values)
    //    {
    //        curr_direction_list.Add(directionInfo);
    //    }
    //}

    //public void OnAfterDeserialize()
    //{
    //    allplayers_dic.Clear();
    //    foreach (PlayerInfo playerInfo in allplayers_list)
    //    {
    //        allplayers_dic.Add(playerInfo.uid,playerInfo);
    //    }

    //    curr_direction_dic.Clear();
    //    foreach (DirectionInfo directionInfo in curr_direction_list)
    //    {
    //        curr_direction_dic.Add(directionInfo.roleid, directionInfo);
    //    }
    //}

}

