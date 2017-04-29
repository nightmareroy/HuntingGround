using System;
using System.Collections.Generic;
using SimpleJson;

public class GameStateChangeQueue
{
    //public Queue<int> player_id_queue=new Queue<int>();

    public Queue<int> change_type_queue = new Queue<int>();//0：加入频道 1：离开频道 2：游戏结束

    public Queue<JsonObject> change_data_queue = new Queue<JsonObject>();
//    public void AddFailId(int uid)
//    {
//        fail_id_queue.Enqueue(uid);
//    }
//    public void DeleteFailId(int uid)
//    {
//        fail_id_queue.Dequeue(uid);
//    }
}

