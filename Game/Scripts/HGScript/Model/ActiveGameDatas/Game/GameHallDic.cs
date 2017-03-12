using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;

public class GameHallGame
{
    public int creator_id;
    public string game_name;
    public int gametype_id;

    //uid:groupid
    public Dictionary<int,GameHallPlayer> players_info;

    public void InitFromJson(JsonObject gameJO)
    {
        JsonObject gameObj = gameJO["game"] as JsonObject;
        JsonObject playersObj = gameJO["players"] as JsonObject;

        creator_id = int.Parse(gameObj["creator_id"].ToString());
        game_name = gameObj["game_name"].ToString();
        gametype_id = int.Parse(gameObj["gametype_id"].ToString());

        players_info = new Dictionary<int, GameHallPlayer>();
        foreach (string key in playersObj.Keys)
        {
            GameHallPlayer gameHallPlayer = SimpleJson.SimpleJson.DeserializeObject<GameHallPlayer>(playersObj[key].ToString());
                //new GameHallPlayer();
//            gameHallPlayer.InitFromJson(playersinfoJO[key] as JsonObject);
            players_info.Add(int.Parse(key),gameHallPlayer );
        }

    }
}

public class GameHallPlayer
{
    public int uid;
    public string name;
    public int group_id;

//    public void InitFromJson(JsonObject playerJO)
//    {
//        uid = int.Parse(playerJO["uid"].ToString());
//        name = playerJO["name"].ToString();
//        groupid = int.Parse(playerJO["groupid"].ToString());
//    }
}

public class GameHallDic
{
    public Dictionary<int,GameHallGame> gameHallDic;

    public void InitFromJson(JsonObject gamedicJO)
    {
        gameHallDic = new Dictionary<int, GameHallGame>();
        foreach (string key in gamedicJO.Keys)
        {
            int creator_id = int.Parse(key);
            GameHallGame gameHallGame = new GameHallGame();
            gameHallGame.InitFromJson(gamedicJO[key] as JsonObject);
            gameHallDic.Add(creator_id,gameHallGame);
        }
    }
}

