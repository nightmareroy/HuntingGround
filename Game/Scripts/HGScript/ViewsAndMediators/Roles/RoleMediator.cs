using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using MapNavKit;
using UnityEngine;



public class RoleMediator:Mediator
{
    [Inject]
    public RoleView roleView { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    //[Inject]
    //public SetRolePosSignal setRolePosSignal { get; set; }


    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection { get; set; }

    [Inject]
    public GameDataService gameDataService { get; set; }

    [Inject]
    public DirectionClickSignal directionClickSignal { get; set; }

    [Inject]
    public DirectionClickCallbackSignal directionClickCallbackSignal { get; set; }

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal { get; set; }

    [Inject]
    public UpdateDirectionPathCallbackSignal updateDirectionPathCallbackSignal { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public DoActionAnimSignal doActionAnimSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    MapRootMediator mapRootMediator;

    RoleInfo roleInfo;

    RoleUIView roleUIView;

    //int currentDirectionId;

    public override void OnRegister()
    {
        Debug.Log(roleView.gameObject.name);
        string[] ids = roleView.gameObject.name.Split('_');
        roleView.Init();
        //int uid=int.Parse(ids[1]);
        int roleid = int.Parse(ids[1]);

        roleInfo = gameInfo.role_dic[roleid];

        mapRootMediator = mainContext.mapRootMediator;
        //Debug.Log("addlistener");

        //SetRolePosSignal.Param setRolePosParam = new SetRolePosSignal.Param();
        //setRolePosParam.playerid = playerid;
        //setRolePosParam.roleid = roleid;
        //setRolePosParam.pos_x = 5;
        //setRolePosParam.pos_y = 10;
        //setRolePosParam.callback = UpdateRolePos;
        //setRolePosSignal.Dispatch(setRolePosParam);
        //setRolePosSignal.Dispatch();
        //roleSelectSignal.AddListener(OnRoleSelectSignal);
        updateDirectionPathCallbackSignal.AddListener(OnUpdateDirectionPathCallbackSignal);
        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        UpdateRolePos();

        //directionClickSignal.AddListener((int directionid) => {
        //    currentDirectionId = directionid;
        //});

        roleUIView = resourceService.Spawn("roleui/roleui").GetComponent<RoleUIView>();
        roleUIView.Init(mainContext, gameObject, gameInfo, dGameDataCollection, roleid, gameDataService, resourceService, directionClickSignal, directionClickCallbackSignal, mapNodeSelectSignal, actionAnimStartSignal);
        //roleUIView.cancelPathSelectCallback += () =>
        //{
        //    //取消选区路径，仍然发送一个路径选取完成的signal，参数为0长列表
        //    pathSetFinishedSignal.Dispatch(roleInfo,currentDirectionId,new List<MapNavNode>());
        //    //Debug.Log("send");
        //};
        //pathSetFinishedSignal.AddListener((roleInfo, currentDirectionId,selectedNodeList) =>
        //{
        //    roleUIView.cancelPathSelect.SetActive(false);
        //});



        doActionAnimSignal.AddListener(OnDoActionAnimSignal);
        
    }

    void UpdateRolePos()
    {

        if (mapRootMediator != null)
        {
            //gameObject.transform.position = mapRootMediator.mapRootView.NodeAt<MapNavNode>(roleInfo.pos_x, roleInfo.pos_y).position;
            gameObject.transform.position = mapRootMediator.mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id).position+new Vector3(0,6,0);
        }
        else
            Debug.Log("mapRootMediator isn't exist!");

    }

    //public override void OnRemove()
    //{
    //    //base.OnRemove();
    //    setRolePosCallbackSignal.RemoveListener(UpdateRolePos);
    //    //roleSelectSignal.RemoveListener(OnRoleSelectSignal);
    //}

    void OnUpdateDirectionPathCallbackSignal(UpdateDirectionPathSignal.Param param)
    {
        //Debug.Log(roleid);
        if (param.roleid != roleInfo.roleid)
            return;
        int direction_id = gameInfo.role_dic[param.roleid].direction_id;
        List<int> direction_path = gameInfo.role_dic[param.roleid].direction_path;
        //Debug.Log(directionInfo.path.Count);
        //MapRootMediator mapRootMediator = mainContext.mapRootMediator;
        
        roleView.ClearAllDirectionNode();
        if (direction_path.Count == 0)
        {
            return;
        }
        //Debug.Log(directionInfo.path.Count);
        List<int> tempList = new List<int>();
        tempList.Add(gameInfo.role_dic[param.roleid].pos_id);
        tempList.AddRange(direction_path);
        for (int i = 0; i < tempList.Count; i++)
        {
            int nodeid = tempList[i];
            int lastnodeid;
            int nextnodeid;
            if(i==0)
            {
                lastnodeid=0;
                nextnodeid = tempList[i + 1];
                
            }
            else if (i == tempList.Count - 1)
            {
                lastnodeid = tempList[i - 1];
                nextnodeid=0;
            }
            else
            {
                lastnodeid = tempList[i - 1];
                nextnodeid = tempList[i + 1];
            }
            //Debug.Log(directionInfo.path.Count);
            roleView.GenerateADirectionNode(lastnodeid,nextnodeid,nodeid, direction_id);
        }

    }

    void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
        if (gameDataService.GetRoleInMap(mapNavNode.idx) == null)
        {
            //SetAllUIVisible(false);
            roleView.SetPathVisible(false);
        }
        else if (gameDataService.GetRoleInMap(mapNavNode.idx).roleid == roleInfo.roleid)
        {
            roleView.SetPathVisible(true);
        }
        else
        {
            roleView.SetPathVisible(false);
        }
    }

    //public void OnRoleSelectSignal(RoleInfo selectedRole)
    //{
    //    if (roleInfo == selectedRole)
    //    {
    //        roleUIView.SetDirectionsRootVisible(true);
    //    }
    //    else
    //    {
    //        roleUIView.SetDirectionsRootVisible(false);
    //    }
    //}

    public void OnDestroy()
    {
        //setRolePosCallbackSignal.RemoveListener(UpdateRolePos);
        //roleSelectSignal.AddListener(OnRoleSelectSignal);
        updateDirectionPathCallbackSignal.RemoveListener(OnUpdateDirectionPathCallbackSignal);
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
        doActionAnimSignal.RemoveListener(OnDoActionAnimSignal);
    }

    void OnDoActionAnimSignal(DoActionAnimSignal.Param param)
    {
        switch (param.type)
        {
            //移动
            case 0:
                //int roleid=int.Parse(param.param["roleid"].ToString());
                //int pos_id=int.Parse(param.param["pos_id"].ToString());

                if(param.roleid==roleInfo.roleid)
                {
                    //Vector3 source_pos = gameObject.transform.position;
                    Vector3 target_pos = mapRootMediator.mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id).position+new Vector3(0,6,0);
                    //gameObject.transform.LookAt(target_pos, new Vector3(0,1,0));
                    iTween.MoveTo(gameObject, iTween.Hash("position", target_pos, "easeType", "linear","time",1));
                }

                break;
            //攻击
            case 3:
                break;
            //防御
            case 4:
                break;
            default:
                break;
        }
    }
}

