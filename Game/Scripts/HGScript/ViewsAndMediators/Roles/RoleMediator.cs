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
    public ActiveGameDataService activeGameDataService { get; set; }

//    [Inject]
//    public DirectionClickSignal directionClickSignal { get; set; }
//
//    [Inject]
//    public DirectionClickCallbackSignal directionClickCallbackSignal { get; set; }

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal { get; set; }

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal { get; set; }


    [Inject]
    public SPlayerInfo sPlayerInfo { get; set; }

    [Inject]
    public DoRoleActionAnimSignal doActionAnimSignal { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    [Inject]
    public FlowUpTipSignal flowUpTipSignal { get; set; }

    MapRootMediator mapRootMediator;

    RoleInfo roleInfo;

    RoleUIView roleUIView;

    //int currentDirectionId;
    string role_id;

    public override void OnRegister()
    {
        Debug.Log(roleView.gameObject.name);
        string[] ids = roleView.gameObject.name.Split('_');
        roleView.Init();
        role_id = ids[1];

        roleInfo = gameInfo.role_dic[role_id];

        mapRootMediator = mainContext.mapRootMediator;
        updateRoleDirectionSignal.AddListener(OnUpdateRoleDirectionSignal);
        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        UpdateRolePos();

        roleUIView = resourceService.Spawn("roleui/roleui").GetComponent<RoleUIView>();
        roleUIView.Init(mainContext, gameObject, gameInfo, dGameDataCollection, role_id, activeGameDataService, resourceService, mapNodeSelectSignal, actionAnimStartSignal,doActionAnimSignal,flowUpTipSignal);
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

    void OnUpdateRoleDirectionSignal(string updated_role_id)
    {
        //Debug.Log(roleid);
        if (updated_role_id != roleInfo.role_id)
            return;
        int direction_did = gameInfo.role_dic[updated_role_id].direction_did;
        List<int> direction_path = gameInfo.role_dic[updated_role_id].direction_param;

        if (direction_did != 8)//不是吃料理指令
        {
            if (direction_path.Count == 0)
            {
                roleView.ClearArrow();
                return;
            }

            Vector3[] tempList = new Vector3[direction_path.Count + 1];
            tempList[direction_path.Count] = mapRootMediator.mapRootView.GetNodeObj(gameInfo.role_dic[updated_role_id].pos_id).transform.position;
            for (int i = 0; i < direction_path.Count; i++)
            {
                tempList[direction_path.Count - 1 - i] = (mapRootMediator.mapRootView.GetNodeObj(direction_path[i]).transform.position);
            }
            Vector3 newFirst = new Vector3(tempList[0].x * 0.7f + tempList[1].x * 0.3f, 0, tempList[0].z * 0.7f + tempList[1].z * 0.3f);
            //        Vector3 newLast = new Vector3(tempList[tempList.Length-2].x*0.3f+tempList[tempList.Length-1].x*0.7f,0,tempList[tempList.Length-2].z*0.3f+tempList[tempList.Length-1].z*0.7f);
            tempList[0] = newFirst;


            roleView.GenerateArrow(tempList);
        }
        else
        {
            roleView.ClearArrow();
        }

    }



    void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
//        if (mapNavNode == null)
//            return;
//        RoleInfo r=activeGameDataService.GetRoleInMap(mapNavNode.idx);
//        if (r != null)
//        {
//            List<int> l = activeGameDataService.GetAllDirectionDids(r.role_id);
//        }
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
        Debug.Log("destroy role " + role_id);
        //setRolePosCallbackSignal.RemoveListener(UpdateRolePos);
        //roleSelectSignal.AddListener(OnRoleSelectSignal);
        updateRoleDirectionSignal.RemoveListener(OnUpdateRoleDirectionSignal);
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
        doActionAnimSignal.RemoveListener(OnDoActionAnimSignal);
    }

    void OnDoActionAnimSignal(DoRoleActionAnimSignal.Param param)
    {
        if(param.role_id==roleInfo.role_id)
        {
            switch (param.type)
            {

                //移动
                case 0:
                    //int roleid=int.Parse(param.param["roleid"].ToString());
                    //int pos_id=int.Parse(param.param["pos_id"].ToString());


                    //Vector3 source_pos = gameObject.transform.position;
                    Vector3 target_pos = mapRootMediator.mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id).position+new Vector3(0,6,0);
                    //gameObject.transform.LookAt(target_pos, new Vector3(0,1,0));
                    iTween.MoveTo(gameObject, iTween.Hash("position", target_pos, "easeType", "linear", "time", 0.5));

                    break;
//                //出现
//                case 1:
//                    break;
                //消失
                case 2:
                    Destroy(gameObject);
                    break;
                //掉血（在ui处理）
                case 3:
                    
                    break;
                //回血（ui处理）
                case 4:
                    break;
                //攻击
                case 5:
                    int pos_id = (int)param.value;
                    roleView.DoAttack(roleInfo.pos_id ,pos_id);
                    break;
                //转圈
                case 6:
                    break;

            }
        }

    }

    void OnMoveAnimComplete()
    {
        iTween.Stop(gameObject);
    }

}

