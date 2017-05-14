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
    public const string DoSubAction = "DoSubAction";
    public const string UpdateDirectionTurn = "UpdateDirectionTurn";
    //public const string PlayerFail="PlayerFail";
    public const string UserEnter = "UserEnter";
    public const string UserLeave = "UserLeave";
    public const string GameOver = "GameOver";

    public const string InviteFight = "InviteFight";
    public const string CancelInviteFight = "CancelInviteFight";
    public const string FriendGameStart = "FriendGameStart";
    public const string RefuseInviteFight = "RefuseInviteFight";

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
    [Inject]
    public BroadcastSubActionSignal broadcastSubActionSignal { get; set; }
    //[Inject]
    //public PlayerFailPushSignal playerFailPushSignal { get; set; }
    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get; set; }
    [Inject]
    public CheckGameStateQueueSignal checkGameStateQueueSignal { get; set; }

    //friend
    [Inject]
    public InviteFightPushSignal inviteFightPushSignal { get; set; }
    [Inject]
    public CancelInviteFightPushSignal canCelnviteFightPushSignal { get; set; }
    [Inject]
    public FriendGameStartPushSignal friendGameStartPushSignal { get; set; }
    [Inject]
    public RefuseInviteFightPushSignal refuseInviteFightPushSignal { get; set; }



    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal { get; set; }




    [Inject]
    public GameInfo gameInfo{ get; set;}
    [Inject]
    public GameStateChangeQueue gameStateChangeQueue{ get; set;}

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
            broadcastActionSignal.Dispatch(msg.data as JsonObject);

        });
        pclient.on(DoSubAction, (msg) =>
        {
            broadcastSubActionSignal.Dispatch(msg.data as JsonObject);

        });
        //pclient.on(PlayerFail,(msg)=>{
        //    int uid=int.Parse((msg.data as JsonObject)["uid"].ToString());
        //    playerStateChangeQueue.player_id_queue.Enqueue(uid);
        //    playerStateChangeQueue.change_type_queue.Enqueue(3);
        //    if (!isAnimActing)
        //    {
        //        checkUserStateQueueSignal.Dispatch();
        //    }
        //});
        pclient.on(UserEnter, (msg) =>
        {
            //int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            gameStateChangeQueue.change_data_queue.Enqueue(msg.data as JsonObject);
            gameStateChangeQueue.change_type_queue.Enqueue(0);
            if (!isAnimActing)
            {
                checkGameStateQueueSignal.Dispatch();
            }
        });
        pclient.on(UserLeave, (msg) =>
        {
            //int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            gameStateChangeQueue.change_data_queue.Enqueue(msg.data as JsonObject);
            gameStateChangeQueue.change_type_queue.Enqueue(1);
            if (!isAnimActing)
            {
                checkGameStateQueueSignal.Dispatch();
            }
        });
        pclient.on(GameOver, (msg) =>
        {
            //int uid = int.Parse((msg.data as JsonObject)["uid"].ToString());
            gameStateChangeQueue.change_data_queue.Enqueue(msg.data as JsonObject);
            gameStateChangeQueue.change_type_queue.Enqueue(2);
            if (!isAnimActing)
            {
                checkGameStateQueueSignal.Dispatch();
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
            int src_uid = int.Parse(msgJO["src_uid"].ToString());
            string name = msgJO["name"].ToString();
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            inviteFightPushSignal.Dispatch(src_uid,name);
        });
        pclient.on(CancelInviteFight, (msg) =>
        {
            JsonObject msgJO = msg.data as JsonObject;
            int src_uid = int.Parse(msgJO["src_uid"].ToString());
            //string name = msgJO["name"].ToString();
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            canCelnviteFightPushSignal.Dispatch(src_uid);
        });
        pclient.on(FriendGameStart, (msg) =>
        {
            //JsonObject msgJO = msg.data as JsonObject;
            //int uid = int.Parse(msgJO["uid"].ToString());
            //string name = msgJO["name"].ToString();
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            friendGameStartPushSignal.Dispatch();
        });
        pclient.on(RefuseInviteFight, (msg) =>
        {
            JsonObject msgJO = msg.data as JsonObject;
            int tar_uid = int.Parse(msgJO["tar_uid"].ToString());
            //string name = msgJO["name"].ToString();
            //int direction_turn = int.Parse((msg.data as JsonObject)["direction_turn"].ToString());
            refuseInviteFightPushSignal.Dispatch(tar_uid);
        });
    }

}

