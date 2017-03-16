using System;
using System.Collections.Generic;

public class PlayerStateChangeQueue
{
    public Queue<int> player_id_queue=new Queue<int>();

    public Queue<int> change_type_queue = new Queue<int>();//0：加入频道 1：离开频道 2：失败退出游戏
//    public void AddFailId(int uid)
//    {
//        fail_id_queue.Enqueue(uid);
//    }
//    public void DeleteFailId(int uid)
//    {
//        fail_id_queue.Dequeue(uid);
//    }
}

