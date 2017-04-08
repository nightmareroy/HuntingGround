using System;
using UnityEngine;
using SimpleJson;
using Pomelo.DotNetClient;

public class NetPushSignalSerivice
{
    

    //push routes
    //game hall
    //public const string CreateMultiGame="CreateMultiGame";
    //public const string CancelMultiGame="CancelMultiGame";
    //public const string JoinMultiGame="JoinMultiGame";
    //public const string LeaveMultiGame="LeaveMultiGame";

    //game
    public const string MultiGameStart="MultiGameStart";
    public const string NextTurn="NextTurn";
    public const string DoAction="DoAction";
    public const string UpdateDirectionTurn = "UpdateDirectionTurn";
    public const string PlayerFail="PlayerFail";
    public const string UserEnter = "UserEnter";
    public const string UserLeave = "UserLeave";

    public const string InviteFight = "InviteFight";

    //signal
    ////game hall
    //[Inject]
    //public CreateMultiGamePushSignal createMultiGamePushSignal { get; set; }
    //[Inject]
    //public CancelMultiGamePushSignal cancelMultiGamePushSignal { get; set; }
    //[Inject]
    //public JoinMultiGamePushSignal joinMultiGamePushSignal { get; set; }
    //[Inject]
    //public LeaveMultiGamePushSignal leaveMultiGamePushSignal { get; set; }

    //game
    [Inject]
    public MultiGameStartPushSignal multiGameStartPushSignal { get; set; }
    [Inject]
    public NextTurnPushSignal nextTurnPushSignal { get; set; }
    [Inject]
    public BroadcastActionSignal broadcastActionSignal { get; set; }
    //[Inject]
    //public PlayerFailPushSignal playerFailPushSignal { get; set; }
    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get; set; }
    [Inject]
    public CheckUserStateQueueSignal checkUserStateQueueSignal { get; set; }

    //friend
    [Inject]
    public InviteFightPushSignal inviteFightPushSignal { get; set; }




    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }




    [Inject]
    public GameInfo gameInfo{ get; set;}
    [Inject]
    public PlayerStateChangeQueue playerStateChangeQueue{ get; set;}

    bool isAnimActing = false;

    public void Init(Connection pclient)
    {
        actionAnimStartSignal.AddListener(() => {
            isAnimActing = true;
        });

        actionAnimFinishSignal.AddListener(() => {
            isAnimActing = false;
        });


        //game hall
        //pclient.on(CreateMultiGame,(msg)=>{
        //    createMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        //});
        //pclient.on(CancelMultiGame,(msg)=>{
        //    cancelMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        //});
        //pclient.on(JoinMultiGame,(msg)=>{
        //    joinMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        //});
        //pclient.on(LeaveMultiGame,(msg)=>{
        //    leaveMultiGamePushSignal.Dispatch(msg.data as JsonObject); 
        //});

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
            int uid=int.Parse((msg.data as JsonObject)["uid"].ToString());
            playerStateChangeQueue.player_id_queue.Enqueue(uid);
            playerStateChangeQueue.change_type_queue.Enqueue(3);
            if (!isAnimActing)
            {
                checkUserStateQueueSignal.Dispatch();
            }
        });
        pclient.on(UserEnter, (msg) =>
        {
            int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            playerStateChangeQueue.player_id_queue.Enqueue(uid);
            playerStateChangeQueue.change_type_queue.Enqueue(0);
            if (!isAnimActing)
            {
                checkUserStateQueueSignal.Dispatch();
            }
        });
        pclient.on(UserLeave, (msg) =>
        {
            int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            playerStateChangeQueue.player_id_queue.Enqueue(uid);
            playerStateChangeQueue.change_type_queue.Enqueue(1);
            if (!isAnimActing)
            {
                checkUserStateQueueSignal.Dispatch();
            }
        });
        pclient.on(UpdateDirectionTurn, (msg) =>
        {
            int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            updateDirectionTurnSignal.Dispatch(uid);
        });

        //friend
        pclient.on(InviteFight, (msg) =>
        {
            JsonObject msgJO=msg.data as JsonObject;
            int uid = int.Parse(msgJO["uid"].ToString());
            string name = msgJO["name"].ToString();
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            inviteFightPushSignal.Dispatch(uid,name);
        });
    }

}

