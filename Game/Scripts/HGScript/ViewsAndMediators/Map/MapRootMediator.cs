using UnityEngine;
using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using MapNavKit;

public class MapRootMediator : Mediator {

    [Inject]
    public MapRootView mapRootView { get; set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get; set;}

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal { get; set; }

    [Inject]
    public FindNodeSignal findNodeSignal { get; set; }

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal { get; set; }

    [Inject]
    public FindFreeRoleSignal findFreeRoleSignal { get; set; }

    //[Inject]
    //public RoleSelectSignal roleSelectSignal { get; set; }

    [Inject]
    public ActiveGameDataService activeGameDataService { get; set; }

    //[Inject]
    //public TestSignal testSignal { get; set; }

//    [Inject]
//    public DirectionClickCallbackSignal directionClickCallbackSignal { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public DGameDataCollection dGameDataCollection{get;set;}



    //[Inject]
    //public PathSetFinishedSignal pathSetFinishedSignal { get; set; }

//    List<int> selectedNodeList = new List<int>();

    //List<MapNavNode> pathList=new List<MapNavNode>();

    MapNavNode selectedNode = null;
    RoleInfo selectedRoleInfo = null;

    //int currentDirectionId;

    public override void OnRegister()
    {
        mapRootView.Init(gameInfo);
        //mapRootView.CreateGrid<MapNavNode>();



        MainContext mainContext = contextView.GetComponent<BootstrapView>().context as MainContext;
        mainContext.mapRootMediator = this;

        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        findNodeSignal.AddListener(OnFindRoleSignal);

        //updateDirectionPathSignal.AddListener(OnUpdateDirectionPathSignal);

        //pathSetFinishedSignal.AddListener(OnPathSetFinishedSignal);

//        directionClickCallbackSignal.AddListener(OnDirectionClickCallbackSignal);

        //cancelPathSelectSignal.AddListener(OnCancelPathSelectSignal);

        mapRootView.OnMouseEnter += OnMouseEnter;

        mapRootView.pathSetFinishCallback += OnPathSetFinished;

        //testSignal.Dispatch();
        InstanciateGameInfo();

        
    }

    void InstanciateGameInfo()
    {
        foreach (string role_id in gameInfo.role_dic.Keys)
        {
            GameObject roleobj = resourceService.Spawn("role/" + gameInfo.role_dic[role_id].role_did);
            roleobj.name = "role_" + role_id;
//            Debug.Log("123");
        }
    }

    void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
        //Debug.Log("mapNavNode");
        findNodeSignal.Dispatch(null, false);
        if (mapNavNode == null)
        {
            selectedNode = null;
            selectedRoleInfo = null;
            
            //mapRootView.ClearUnmovableZoon();
            //findNodeSignal.Dispatch(null,false);
            mapRootView.SetNodeSelect(-1);
            return;
        }
        selectedNode = mapNavNode;
        selectedRoleInfo = activeGameDataService.GetRoleInMap(mapNavNode.idx);
        
        if (selectedNode != null)
        {
            mapRootView.SetNodeSelect(selectedNode.idx);
        }
        else
        {
            mapRootView.SetNodeSelect(-1);
        }

        //if (selectedRoleInfo == null)
        //{
        //    findNodeSignal.Dispatch(null, false);
        //}



        //if (selectedRoleInfo != null)
        //{
            
        //    if (selectedRoleInfo.uid == sPlayerInfo.uid)
        //    {
        //        mapRootView.SetUnmovableZoon(selectedRoleInfo.pos_id, selectedRoleInfo.max_move);
        //    }
        //    else
        //    {
        //        mapRootView.ClearUnmovableZoon();
        //    }

        //}
        //else
        //{
        //    mapRootView.ClearUnmovableZoon();
        //}


    }

    void OnFindRoleSignal(MapNavNode mapNavNode,bool enableSetPath)
    {
        mapRootView.CamConcentrate(mapNavNode, enableSetPath);
    }

//    void OnDirectionClickCallbackSignal(DirectionClickSignal.Param param)
//    {
//        //Debug.Log("direction:"+directionid+" has been clicked!");
//        mapRootView.SetDraggingCameraMode(false);
//        //gameInfo.curr_direction_dic[param.roleid].path.Clear();
//        selectedNodeList.Clear();
//        updateDirectionPathSignal.Dispatch(new UpdateDirectionPathSignal.Param(selectedRoleInfo.role_id,selectedNodeList));
//        //currentDirectionId = directionid;
//
//    }


    void OnMouseEnter(MapNavNode lastNode,MapNavNode thisNode)
    {
        
//        if (lastNode == null)
//        {
//            //selectedNodeList.Clear();
//            if (mapRootView.NodeIndicesAround(selectedRoleNode.idx, true, false, null).Contains(thisNode.idx))
//            {
//                //selectedNodeList.Add(selectedRoleNode.idx);
//                selectedNodeList.Add(thisNode.idx);
//            }
//            //else if (thisNode.idx == selectedRoleNode.idx)
//            //{
//            //    selectedNodeList.Add(thisNode.idx);
//            //}
//        }
//        else
//        {
//            if (mapRootView.NodeIndicesAround(lastNode.idx, true, false, null).Contains(thisNode.idx))
//            {
//                selectedNodeList.Add(thisNode.idx);
//            }
//        }
        Dictionary<int,DLandform> dLandformDic=dGameDataCollection.dLandformCollection.dLandformDic;
        //第一个按下的node
        if (lastNode == null)
        {
            gameInfo.role_dic[selectedRoleInfo.role_id].direction_param.Clear();
//            selectedNodeList.Clear();
            if (thisNode.idx != selectedNode.idx)
            {
                List<MapNavNode> autoGeneratedPath = mapRootView.Path<MapNavNode>(selectedNode, thisNode, mapRootView.NodeCostCallback, selectedRoleInfo);
//                float cost=0;
                for (int i = 0; i < autoGeneratedPath.Count; i++)
                {
//                    selectedNodeList.Add(autoGeneratedPath[i].idx);



//                    float temp_cost;
//                    if (i > 0)
//                    {
//                        temp_cost = dLandformDic[gameInfo.map_info.landform[autoGeneratedPath[i - 1].idx]].cost;
//                    }
//                    else
//                    {
//                        temp_cost = dLandformDic[gameInfo.map_info.landform[selectedRoleInfo.pos_id]].cost;
//                    }
//
//                    temp_cost+=dLandformDic[gameInfo.map_info.landform[autoGeneratedPath[i].idx]].cost;
//
//                    if (cost + temp_cost > selectedRoleInfo.max_move)
//                    {
//                        break;
//                    }
//                    else
//                    {
                        gameInfo.role_dic[selectedRoleInfo.role_id].direction_param.Add(autoGeneratedPath[i].idx);
                        
//                    }
                }
            }

        }
        else
        {
//            foreach (int pos_id in gameInfo.role_dic[selectedRoleInfo.role_id].direction_param)
//            {
//                
//            }
            gameInfo.role_dic[selectedRoleInfo.role_id].direction_param.Add(thisNode.idx);
        }


        //if (selectedNodeList.Count == 0)
        //{
        //    //由于这个path方法返回的元素不包括起点，却包括终点，而我想要的列表是包括起点，不包括终点，因此将起点和终点对调，最后把返回的list倒过来添加到路径列表中
        //    List<MapNavNode> autoGeneratedInversPath = mapRootView.Path<MapNavNode>(mapNavNode, selectedRoleNode, NodeCostCallback);
        //    for (int i = autoGeneratedInversPath.Count - 1; i > 0; i--)
        //    {
        //        selectedNodeList.Add(autoGeneratedInversPath[i].idx);
        //    }
        //}
        //selectedNodeList.Add(mapNavNode.idx);

        //if (selectedNodeList.Count == 0)
        //{
        //    List<MapNavNode> autoGeneratedPath = mapRootView.Path<MapNavNode>(selectedRoleNode, mapNavNode, NodeCostCallback);
        //    if (autoGeneratedPath.Count > 0)
        //    {
        //        for (int i = 0; i < autoGeneratedPath.Count - 1; i++)
        //        {
        //            selectedNodeList.Add(autoGeneratedPath[i].idx);
        //        }
        //    }
        //}
        //if (selectedRoleNode != mapNavNode)
        //{
        //    selectedNodeList.Add(mapNavNode.idx);
        //}
//        Debug.Log("dispatch");

        //更新指令
        //if (gameInfo.role_dic[selectedRoleInfo.role_id].direction_param.Count == 0)
        //{
        //    gameInfo.role_dic[selectedRoleInfo.role_id].direction_did = 2;//设为防御
        //}
        //else
        //{
        //    gameInfo.role_dic[selectedRoleInfo.role_id].direction_did = 1;//设为移动
        //}
        gameInfo.role_dic[selectedRoleInfo.role_id].direction_did = 1;//设为移动
        


        float cost=0;
        if (selectedRoleInfo.direction_param.Count > 0)
        {
//            cost=dLandformDic[gameInfo.map_info.landform[selectedRoleInfo.pos_id]].cost;
            for (int i = 0; i < selectedRoleInfo.direction_param.Count; i++)
            {
                //if (i > 0)
                //{
                //    cost += dLandformDic[gameInfo.map_info.landform[selectedRoleInfo.direction_param[i - 1]]].cost;
                //}
                //else
                //{
                //    cost=dLandformDic[gameInfo.map_info.landform[selectedRoleInfo.pos_id]].cost;
                //}

                cost+=dLandformDic[gameInfo.map_info.landform[selectedRoleInfo.direction_param[i]]].cost;
            }
        }

//        Debug.Log(cost);

//        int path_length = selectedRoleInfo.direction_param.Count;
//        int move = (int)Math.Floor( selectedRoleInfo.max_move);
        if (cost > selectedRoleInfo.max_move)
        {
            selectedRoleInfo.direction_param.RemoveAt(selectedRoleInfo.direction_param.Count-1);

            //mapNodeSelectSignal.Dispatch(null);
            findFreeRoleSignal.Dispatch();
            return;
        }
        
        updateRoleDirectionSignal.Dispatch(selectedRoleInfo.role_id);
    }

    //void OnUpdateDirectionPathSignal(RoleInfo roleInfo, int directionId, List<MapNavNode> selectdNodeList)
    //{
    //    //foreach (MapNavNode mapNavNode in selectedNodeList)
    //    //{
    //    //    mapRootView.SetColor(currentDirectionId, autoGeneratedInversPath[i].idx);
    //    //}
    //}

    void OnPathSetFinished()
    {
//        mapRootView.SetDraggingCameraMode(true);
        //pathSetFinishedSignal.Dispatch(selectedRoleInfo, currentDirectionId, selectedNodeList);
//        mapNodeSelectSignal.Dispatch(null);
        
    }

//    void OnPathSetFinishedSignal(RoleInfo roleInfo,int directionId, List<MapNavNode> selectdNodeList)
//    {
//        mapRootView.SetDraggingCameraMode(true);
//        selectedRoleNode = null;
//        selectedRoleInfo = null;
//
//        //Debug.Log(selectedNodeList.Count);
//    }



    //void OnCancelPathSelectSignal()
    //{
    //    mapRootView.SetDraggingCameraMode(true);
    //    //Debug.Log("cancel");
    //}

    //public override void OnRemove()
    //{
    //    mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
    //    //pathSetFinishedSignal.RemoveListener(OnPathSetFinishedSignal);
    //    directionClickCallbackSignal.RemoveListener(OnDirectionClickCallbackSignal);
    //}

    public void OnDestroy()
    {
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
        findNodeSignal.RemoveListener(OnFindRoleSignal);
        //pathSetFinishedSignal.RemoveListener(OnPathSetFinishedSignal);
//        directionClickCallbackSignal.RemoveListener(OnDirectionClickCallbackSignal);
    }
}
