using System;
using UnityEngine;
using SimpleJson;
using Pomelo.DotNetClient;

public class NetPushSignalSerivice
{
    

    //push routes
    //game hall
    public const string CreateMultiGame="CreateMultiGame";
    public const string CancelMultiGame="CancelMultiGame";
    public const string JoinMultiGame="JoinMultiGame";
    public const string LeaveMultiGame="LeaveMultiGame";

    //game
    public const string MultiGameStart="MultiGameStart";
    public const string NextTurn="NextTurn";
    public const string DoAction="DoAction";
    public const string UpdateDirectionTurn = "UpdateDirectionTurn";
    public const string PlayerFail="PlayerFail";

    //signal
    //game hall
    [Inject]
    public CreateMultiGamePushSignal createMultiGamePushSignal { get; set; }
    [Inject]
    public CancelMultiGamePushSignal cancelMultiGamePushSignal { get; set; }
    [Inject]
    public JoinMultiGamePushSignal joinMultiGamePushSignal { get; set; }
    [Inject]
    public LeaveMultiGamePushSignal leaveMultiGamePushSignal { get; set; }

    //game
    [Inject]
    public MultiGameStartPushSignal multiGameStartPushSignal { get; set; }
    [Inject]
    public NextTurnPushSignal nextTurnPushSignal { get; set; }
    [Inject]
    public BroadcastActionSignal broadcastActionSignal { get; set; }
    [Inject]
    public PlayerFailPushSignal playerFailPushSignal{ get; set;}
    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get; set; }






    [Inject]
    public GameInfo gameInfo{ get; set;}
    [Inject]
    public PlayerFailQueue playerFailQueue{ get; set;}



    public void Init(Connection pclient)
    {
        //game hall
        pclient.on(CreateMultiGame,(msg)=>{
            createMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        });
        pclient.on(CancelMultiGame,(msg)=>{
            cancelMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        });
        pclient.on(JoinMultiGame,(msg)=>{
            joinMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        });
        pclient.on(LeaveMultiGame,(msg)=>{
            leaveMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        });

        //game
        pclient.on(MultiGameStart,(msg)=>{
            multiGameStartPushSignal.Dispatch(msg.data as JsonObject); 
        });
        pclient.on(NextTurn,(msg)=>{
            nextTurnPushSignal.Dispatch(msg.data as JsonObject); 
        });
        pclient.on(DoAction, (msg) => {
//            Debug.Log((msg.data as JsonArray).ToString());
//            RoleActionList roleActionList=new RoleActionList();
//            roleActionList.InitFromJson(msg.data as JsonArray);
            broadcastActionSignal.Dispatch(msg.data as JsonArray);

        });
        pclient.on(PlayerFail,(msg)=>{
            int fail_uid=int.Parse((msg.data as JsonObject)["uid"].ToString());
            playerFailQueue.fail_id_queue.Enqueue(fail_uid);
            playerFailPushSignal.Dispatch(fail_uid);
        });
        pclient.on(UpdateDirectionTurn, (msg) =>
        {
            int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            updateDirectionTurnSignal.Dispatch(uid,direction_turn);
        });
    }

}

