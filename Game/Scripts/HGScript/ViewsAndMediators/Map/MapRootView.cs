using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using MapNavKit;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using Priority_Queue;
//using 

public class MapRootView : MapNavHexa,IView {
    public GameObject nodeObj;
    TerrainData mainTerrainData;
    public Terrain mainTerrain;
    public Camera gameCamera;
    public Camera uiCamera;
    public GameObject gameAndUICamerasRoot;
    public Camera overviewCamera;

    public Material hillMat;
    public Material mountainMat;

    public Material grassMat;
    public Material forestMat;

    public UnityEngine.UI.GraphicRaycaster graphicRaycaster;
    public UnityEngine.EventSystems.EventSystem eventSystem;



    bool isDraggingCam = false;
    bool isDraggingPath =false;

    //两者意义不同，并不严格相反，当isDraggingCameraMode刚刚变为false时，isDraggingPath仍然为false，因为尚未开始拖拽
//    bool isDraggingCameraMode = true;
//    bool isDraggingPath = false;

    float MovingSpeed = 25;

    //public System.Action<MapNavNode> OnMouseDown;
    //public System.Action<MapNavNode> OnMouseUp;
    public System.Action<MapNavNode,MapNavNode> OnMouseEnter;//只在拖拽路径模式下触发

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal { get; set; }

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public DoMapUpdateSignal doMapUpdateSignal { get; set; }

    [Inject]
    public ActiveGameDataService activeGameDataService{ get; set;}

    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    [Inject]
    public DoSightzoonUpdateSignal doSightzoonUpdateSignal{ get; set;}

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal{ get; set;}

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal{ get; set;}

    [Inject]
    public NextturnSignal nextturnSignal { get;set; }

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get;set; }

    [Inject]
    public SPlayerInfo sPlayerInfo { get;set; }

    [Inject]
    public FindNodeSignal findNodeSignal { get; set; }

    //[Inject]
    //public PathSetFinishedSignal pathSetFinishedSignal { get; set; }

    public System.Action pathSetFinishCallback;


    List<int> unmovablePosIdList=new List<int>();

    MapNavNode lastEnteredNode = null;

    //用来记录一次移动操作在两个方向上各自一共移动了多少距离
    float totalMoveX = 0;
    float totalMoveY = 0;

    //当拖拽范围不超过这个距离时，鼠标抬起，认定为触发了选取节点操作；超过这个距离则只拖拽，不选取
    const float clickBoundWithinMove = 0.05f;

    Vector3 startHit=Vector3.zero;
    Vector3 thisHit=Vector3.zero;
    Vector3 startCamPos = Vector3.zero;
    Vector3 startHit2 = Vector3.zero;
    Vector3 thisHit2 = Vector3.zero;
    //Vector3 startCamPos2 = Vector3.zero;

    int[,][,] sightSys = new int[,][,]
        {
            {
                new int[,]
                {
                    {1,1},{1,0},{0,-1},{-1,0},{-1,1},{0,1}
                },
                new int[,]
                {
                    {2,1},{2,0},{2,-1},{1,-1},{0,-2},{-1,-1},{-2,-1},{-2,0},{-2,1},{-1,2},{0,2},{1,2}
                },
                new int[,]
                {
                    {3,2},{3,1},{3,0},{3,-1},{2,-2},{1,-2},{0,-3},{-1,-2},{-2,-2},{-3,-1},{-3,0},{-3,1},{-3,2},{-2,2},{-1,3},{0,3},{1,3},{2,2}
                }
            },
            {
                new int[,]
                {
                    {1,0},{1,-1},{0,-1},{-1,-1},{-1,0},{0,1}
                },
                new int[,]
                {
                    {2,1},{2,0},{2,-1},{1,-2},{0,-2},{-1,-2},{-2,-1},{-2,0},{-2,1},{-1,1},{0,2},{1,1}
                },
                new int[,]
                {
                    {3,1},{3,0},{3,-1},{3,-2},{2,-2},{1,-3},{0,-3},{1,-3},{-2,-2},{-3,-2},{-3,-1},{-3,0},{-3,1},{-2,2},{-1,2},{0,3},{1,2},{2,2}
                }
            }
        };

    
    float width, length, hori, vert;//, offs;

    float terrainWidth;

    float[,] mountainHeightData_12;

    float heightMapPointDist;

    int selected_pos_id=-1;

    bool isActing=false;

    public void Init(GameInfo gameInfo)
    {
        mainTerrainData = mainTerrain.terrainData;

        //Debug.Log(gameInfo.mapInfo.width);

        mapHorizontalSize = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameInfo.gametype_id].width;
        mapVerticalSize = dGameDataCollection.dGameTypeCollection.dGameTypeDic[gameInfo.gametype_id].height;
        coordSystem = CoordinateSystem.EvenOffset;
        gridOrientation = GridOrientation.FlatTop;


        nodeSize = 0.5f;
        maxNodeHeight = 0;
        //CreateGrid();


        //width:六边形宽度（最长的对角线）
        //length:六边形高度（两个平行边的距离）
        //hori:六边形的平均宽度（用来和水平总个数相乘）
        //vert:六边形的平均高度(用来和垂直总个数相乘)
        if (gridOrientation == GridOrientation.FlatTop)
        {
            width = nodeSize * 2f;
            length = Mathf.Sqrt(3f) * 0.5f * width;
            hori = 0.75f * width;
            vert = length;
            //offs = vert * 0.5f;
        }
        else
        {
            length = nodeSize * 2f;
            width = Mathf.Sqrt(3f) * 0.5f * length;
            vert = 0.75f * length;
            hori = width;
            //offs = hori * 0.5f;
        }

        //整个terrain的宽度（地形必须是正方形的，方便设置高度分辨率）
        terrainWidth=(mapVerticalSize + 6f) * vert;

        mainTerrainData.heightmapResolution = 512;

        //高度图的点间距
        heightMapPointDist = terrainWidth / (float)mainTerrainData.heightmapResolution;

        //float blockPointCount = length / heightMapPointDist;
        //Debug.Log(blockPointCount);
        //float[,] mountainHeightData_8 = new float[8,8]
        //   {{ 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        //    { 0.0f, 0.1f, 0.1f, 0.2f, 0.2f, 0.1f, 0.1f, 0.0f },
        //    { 0.0f, 0.1f, 0.4f, 0.6f, 0.6f, 0.4f, 0.1f, 0.0f },
        //    { 0.0f, 0.2f, 0.6f, 0.8f, 0.8f, 0.6f, 0.2f, 0.0f },
        //    { 0.0f, 0.2f, 0.6f, 0.8f, 0.8f, 0.6f, 0.2f, 0.0f },
        //    { 0.0f, 0.1f, 0.4f, 0.6f, 0.6f, 0.4f, 0.1f, 0.0f },
        //    { 0.0f, 0.1f, 0.1f, 0.2f, 0.2f, 0.1f, 0.1f, 0.0f },
        //    { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }};
        mountainHeightData_12 = new float[12, 12]
           {{ 0.0f, 0.0f, 0.0f, 0.0f, 0.1f, 0.1f ,0.1f, 0.1f, 0.0f, 0.0f, 0.0f, 0.0f },
            { 0.0f, 0.0f, 0.1f, 0.1f, 0.1f, 0.2f ,0.2f, 0.1f, 0.1f, 0.1f, 0.0f, 0.0f },
            { 0.0f, 0.1f, 0.1f, 0.2f, 0.2f, 0.3f ,0.3f, 0.2f, 0.2f, 0.1f, 0.1f, 0.0f },
            { 0.0f, 0.1f, 0.2f, 0.0f, 0.5f, 0.6f ,0.6f, 0.5f, 0.0f, 0.2f, 0.1f, 0.0f },
            { 0.1f, 0.1f, 0.2f, 0.5f, 0.7f, 0.8f ,0.8f, 0.7f, 0.5f, 0.2f, 0.1f, 0.1f },
            { 0.1f, 0.2f, 0.3f, 0.6f, 0.8f, 0.9f ,0.9f, 0.8f, 0.6f, 0.3f, 0.2f, 0.1f },
            { 0.1f, 0.2f, 0.3f, 0.6f, 0.8f, 0.9f ,0.9f, 0.8f, 0.6f, 0.3f, 0.2f, 0.1f },
            { 0.1f, 0.1f, 0.2f, 0.5f, 0.7f, 0.8f ,0.8f, 0.7f, 0.5f, 0.2f, 0.1f, 0.1f },
            { 0.0f, 0.1f, 0.2f, 0.0f, 0.5f, 0.6f ,0.6f, 0.5f, 0.0f, 0.2f, 0.1f, 0.0f },
            { 0.0f, 0.1f, 0.1f, 0.2f, 0.2f, 0.3f ,0.3f, 0.2f, 0.2f, 0.1f, 0.1f, 0.0f },
            { 0.0f, 0.0f, 0.1f, 0.1f, 0.1f, 0.2f ,0.2f, 0.1f, 0.1f, 0.1f, 0.0f, 0.0f },
            { 0.0f, 0.0f, 0.0f, 0.0f, 0.1f, 0.1f ,0.1f, 0.1f, 0.0f, 0.0f, 0.0f, 0.0f }};

        //mainTerrainData.SetHeights(4, 0, mountainHeightData_8);
        //mainTerrainData.SetHeights(4, 8, mountainHeightData_8);
        
        //
        //mainTerrainData.size = new Vector3(mapHorizontalSize * hori + width * 0.25f, 1, (mapVerticalSize + 0.5f) * vert);
        
        mainTerrainData.size = new Vector3(terrainWidth, 0.3f, terrainWidth);

        mainTerrain.transform.position = new Vector3(mainTerrain.transform.position.x - terrainWidth * 0.5f, 0, mainTerrain.transform.position.z - terrainWidth * 0.5f);
        //Debug.Log(mainTerrainData.detailResolution);


        CreateGrid<MapNavNode>();
        SetNodeView();

        doMapUpdateSignal.AddListener(OnDoMapUpdate);
        //mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);
        doSightzoonUpdateSignal.AddListener(OnDoSightzoonUpdateSignal);

        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);
        nextturnSignal.AddListener(OnNextturnSignal);
        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);
        //GenMountToHeightMap();
        
    }

    public void ClearUnmovableZoon()
    {
        SetMoveMarker(unmovablePosIdList,false);
        unmovablePosIdList.Clear();
    }

    public void SetUnmovableZoon(int pos_id, float move)
    {
        int center_x = pos_id % mapHorizontalSize;

//        int[][][] sightSystem = sightSys[center_x & 1];
        List<int> list = new List<int>();
        list.Add(pos_id);
//        for (int j = 0; j < Math.Floor( move); j++)
//        {
//            
//            for (int i = 0; i < sightSys[center_x & 1,j].GetLength(0); i++)
//            {
//                
//                int pos_id_n = pos_id+sightSys[center_x & 1,j][i,0] + sightSys[center_x & 1,j][i,1] * mapHorizontalSize;
//                list.Add(pos_id_n);
//            }
//        }


        List<MapNavNode> l= NodesAround<MapNavNode>(NodeAt<MapNavNode>(pos_id),move,NodeCostCallback);
        foreach (MapNavNode n in l)
        {
            list.Add(n.idx);
        }


        SetMoveMarker(unmovablePosIdList,false);
        unmovablePosIdList = list;
        SetMoveMarker(unmovablePosIdList,true);
    }
        
    bool GetNodeInDragCamZoon(MapNavNode mapNavNode)
    {
        if (mapNavNode == null)
            return true;
        if (unmovablePosIdList.IndexOf(mapNavNode.idx) == -1)
            return true;
        else
            return false;
    }

//    bool GetPathWithinMaxMoveDistance(MapNavNode mapNavNode)
//    {
//
//        RoleInfo roleInfo=activeGameDataService.GetRoleInMap(selected_pos_id);
//        if (roleInfo == null)
//        {
//            return false;
//        }
//        int path_length = roleInfo.direction_param.Count;
//        int move = roleInfo.move;
//        return path_length < move;
//
//    }

    protected void LateUpdate()
    {
        

        //查找鼠标射线与terrain的碰撞点处的node
        RaycastHit hit;
        Ray r = gameCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out hit, 100f, 1 << LayerMask.NameToLayer("Terrain")|LayerMask.NameToLayer("UI")))
        {
            thisHit = hit.point;
//            Debug.Log(hit.transform.gameObject.layer);
//            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
//            {
//                
//                Debug.Log("1");
//            }
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Terrain"))
            {
                Debug.Log("null");
                return;
            }

            MapNavNode n = NodeAtWorldPosition<MapNavNode>(hit.point);
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                if (!isDraggingCam)
                {
                    //如果鼠标或者手势在ui元素上方，则不处理
                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    {
                        UnityEngine.EventSystems.PointerEventData eventData = new UnityEngine.EventSystems.PointerEventData(eventSystem);
                        eventData.pressPosition = Input.mousePosition;
                        eventData.position = Input.mousePosition;
                        List<UnityEngine.EventSystems.RaycastResult> list = new List<UnityEngine.EventSystems.RaycastResult>();
                        graphicRaycaster.Raycast(eventData, list);
                        if (list.Count != 0)
                        {
                            UnityEngine.EventSystems.RaycastResult result = list[0];
                            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                            {
                                return;
                            }
                        }


                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (GetNodeInDragCamZoon(n))
                    {
                        isDraggingCam = true;
                        startHit = hit.point;
                        startCamPos = gameAndUICamerasRoot.transform.position;
                    }
                    else
                    {
                        isDraggingPath = true;
                    }


                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (isDraggingCam == true)
                    {
                        isDraggingCam = false;

                        Vector3 camTrans = gameAndUICamerasRoot.transform.position - startCamPos;
                        if (camTrans.x * camTrans.x + camTrans.z * camTrans.z < clickBoundWithinMove)
                        {
//                            SetNodeSelect(selected_pos_id,false);
//                            selected_pos_id = n.idx;
//                            SetNodeSelect(selected_pos_id,true);
                            //Debug.Log(isActing);
                            if (isActing == false)
                            {
                                mapNodeSelectSignal.Dispatch(n);
                                
                            }
                        }
                    }

                    if (isDraggingPath == true)
                    {
                        isDraggingPath = false;
//                        SetNodeSelect(selected_pos_id,false);
                        if (isActing == false)
                        {
                            mapNodeSelectSignal.Dispatch(null);
                            findNodeSignal.Dispatch(null, false);
                        }
                    }




                }
            }


            if (Input.GetMouseButton(0))
            {
                if (isDraggingPath)
                {
                    MapNavNode enteredNode = n;

                    if (lastEnteredNode != enteredNode)
                    {
//                        if (GetPathWithinMaxMoveDistance(n))
                        if(!GetNodeInDragCamZoon(n))
                        {
                            if (OnMouseEnter != null)
                            {
                                OnMouseEnter(lastEnteredNode, enteredNode);

                            }
                            lastEnteredNode = enteredNode;
                        }
                        else
                        {
//                            if (pathSetFinishCallback != null)
//                                pathSetFinishCallback();
                            isDraggingPath = false;
                            if (isActing == false)
                            {
                                mapNodeSelectSignal.Dispatch(null);
                                findNodeSignal.Dispatch(null, false);
                            }
                        }

                    }
                }


            }
            else
            {
                lastEnteredNode = null;
            }
                



            Vector3 delta = thisHit - startHit;
            if (isDraggingCam)
            {
                gameAndUICamerasRoot.transform.position = new Vector3(gameAndUICamerasRoot.transform.position.x - delta.x, gameAndUICamerasRoot.transform.position.y, gameAndUICamerasRoot.transform.position.z - delta.z);

            }
        }
    }

    //void OnMapNodeSelectSignal(MapNavNode n)
    //{
    //    Hashtable args = new Hashtable();
    //    args.Add("from", gameCamera.orthographicSize);
    //    args.Add("time", 0.1f);
    //    args.Add("onupdate", "OnITweenUpdate");

    //    if (n != null)
    //    {
    //        RoleInfo roleInfo = activeGameDataService.GetRoleInMap(n.idx);
    //        if (roleInfo != null && gameInfo.allplayers_dic[roleInfo.uid].group_id == gameInfo.allplayers_dic[sPlayerInfo.uid].group_id)
    //        {
    //            float sourceY = gameAndUICamerasRoot.transform.position.y;
    //            float toValue = 3.5f;
    //            switch ((int)Math.Floor( roleInfo.max_move))
    //            {
    //                case 1:
    //                    toValue = 3.5f;
    //                    break;
    //                case 2:
    //                    toValue = 5f;
    //                    break;
    //            }



    //            args.Add("to", toValue);
    //            iTween.ValueTo(gameObject, args);

    //            iTween.MoveTo(gameAndUICamerasRoot, new Vector3(n.position.x, sourceY, n.position.z), 0.3f);
    //        }
    //        else
    //        {
                
    //            args.Add("to",3.5f);
    //            iTween.ValueTo(gameObject,args);
    //        }

    //        SetNodeSelect(n.idx);
    //    }
    //    else
    //    {
    //        SetNodeSelect(-1);

    //        args.Add("to",3.5f);
    //        iTween.ValueTo(gameObject,args);
    //    }
    //}

    public void CamConcentrate(MapNavNode n, bool enableSetPath)
    {
        Hashtable args = new Hashtable();
        args.Add("from", gameCamera.orthographicSize);
        args.Add("time", 0.1f);
        args.Add("onupdate", "OnITweenUpdate");

        if (n != null)
        {
            RoleInfo roleInfo = activeGameDataService.GetRoleInMap(n.idx);

            if (roleInfo != null && gameInfo.allplayers_dic[roleInfo.uid].group_id == gameInfo.allplayers_dic[sPlayerInfo.uid].group_id)
            {
                float sourceY = gameAndUICamerasRoot.transform.position.y;
                Vector3 node_pos = GetNodeObj(roleInfo.pos_id).transform.position;
                iTween.MoveTo(gameAndUICamerasRoot, new Vector3(node_pos.x, sourceY, node_pos.z), 0.3f);

                if (enableSetPath)
                {
                    float toValue = 3.5f;
                    switch ((int)Math.Floor(roleInfo.max_move))
                    {
                        case 1:
                            toValue = 3.5f;
                            break;
                        case 2:
                            toValue = 5f;
                            break;
                    }
                    args.Add("to", toValue);
                    iTween.ValueTo(gameObject, args);

                    SetUnmovableZoon(roleInfo.pos_id, roleInfo.max_move);
                }
            }
            else
            {

                args.Add("to", 3.5f);
                iTween.ValueTo(gameObject, args);

                ClearUnmovableZoon();
            }
        }
        else
        {
            args.Add("to", 3.5f);
            iTween.ValueTo(gameObject, args);
            ClearUnmovableZoon();
        }
        

        

        //SetNodeSelect(n.idx);


    }

    void OnITweenUpdate(object value)
    {
        float v = (float)value;
        gameCamera.orthographicSize = v;
        uiCamera.orthographicSize = v;
    }

    void OnActionAnimStartSignal()
    {
        isActing = true;
    }

    void OnActionAnimFinishSignal()
    {
        isActing = false;
    }

    void OnNextturnSignal(Action<bool> callback)
    {
        isActing = true;
    }

    void OnUpdateDirectionTurnSignal(int uid)
    {
        isActing = false;
    }
        

//    protected void LateUpdateOld()
//    {
//        //if (Input.touchCount == 0)
//        //{
//        //    return;
//        //}
//
//        //如果鼠标或者手势在ui元素上方，则不处理
////        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
////        {
////            return;
////        }
//
//
//        //查找鼠标射线与terrain的碰撞点处的node
//        RaycastHit hit;
//        //Ray r = gameCamera.ScreenPointToRay(Input.GetTouch(0).position);//(Input.mousePosition);
//        Ray r = gameCamera.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(r, out hit,100f,1<<LayerMask.NameToLayer("Terrain")))
//        {
//
//            thisHit = hit.point;
//            //if (Input.GetTouch(0).phase == TouchPhase.Began)//Input.GetMouseButtonDown(0))
//            if (Input.GetMouseButtonDown(0))
//            {
//                isDraggingCam = true;
//                startHit = hit.point;
//                startCamPos = gameAndUICamerasRoot.transform.position;
//
//            }
//            //if (Input.GetTouch(0).phase == TouchPhase.Ended)//(Input.GetMouseButtonUp(0))
//            if (Input.GetMouseButtonUp(0))
//            {
//                isDraggingCam = false;
//            }
//            Vector3 delta = thisHit - startHit;              
//            MapNavNode n = NodeAtWorldPosition<MapNavNode>(hit.point);
//
//            //拖拽移动摄像机模式
//            if (isDraggingCam)
//            {
//                if (isDraggingCam)
//                {
//                    gameAndUICamerasRoot.transform.position = new Vector3(gameAndUICamerasRoot.transform.position.x - delta.x, gameAndUICamerasRoot.transform.position.y, gameAndUICamerasRoot.transform.position.z - delta.z);
//
//                }
//                if (Input.GetMouseButtonUp(0))
//                {
//                    Vector3 camTrans = gameAndUICamerasRoot.transform.position - startCamPos;
//                    if (camTrans.x * camTrans.x + camTrans.z * camTrans.z < clickBoundWithinMove)
//                    {
//                        mapNodeSelectSignal.Dispatch(n);
//                    }
//                }
//            }
//            //拖拽设置路径模式
//            else
//            {
//                ////如果鼠标或者手势在ui元素上方，则不处理
//                //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
//                //{
//                //    return;
//                //}
//                //if (Input.GetTouch(0).phase == TouchPhase.Began)
//                if (Input.GetMouseButtonDown(0))
//                {
//                    //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
//                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
//                    {
//                        return;
//                    }
//                }
//                //if (Input.GetTouch(0).phase == TouchPhase.Move)
//                if (Input.GetMouseButton(0))
//                {
//                    isDraggingPath = true;
//
//                    MapNavNode enteredNode = n;
//                    if (lastEnteredNode != enteredNode)
//                    {
//                        //Debug.Log("enter:" + enteredNode.q + "," + enteredNode.r);
//                        if (OnMouseEnter != null)
//                        {
//                            OnMouseEnter(lastEnteredNode,enteredNode);
//
//                        }
//                        lastEnteredNode = enteredNode;
//                    }
//
//                    //第二个手指此时可以拖动摄像机
//                    if (Input.touchCount > 1)// && Input.GetTouch(1).phase == TouchPhase.Moved)
//                    {
//                        RaycastHit hit2;
//                        Ray r2 = gameCamera.ScreenPointToRay(Input.GetTouch(1).position);
//
//                        if (Physics.Raycast(r2, out hit2, 100f, 1 << LayerMask.NameToLayer("Terrain")))
//                        {
//                            thisHit2 = hit2.point;
//                            if (Input.GetTouch(1).phase == TouchPhase.Began)
//                            {
//                                startHit2 = hit2.point;
//                            }
//                            if (Input.GetTouch(1).phase == TouchPhase.Moved)
//                            {
//                                Vector3 delta2 = thisHit2 - startHit2;
//                                gameAndUICamerasRoot.transform.position = new Vector3(gameAndUICamerasRoot.transform.position.x - delta2.x, gameAndUICamerasRoot.transform.position.y, gameAndUICamerasRoot.transform.position.z - delta2.z);
//
//                            }
//                        }
//                    }
//
//                }
//                else
//                {
//                    lastEnteredNode = null;
//                }
//                if (isDraggingPath&&Input.GetMouseButtonUp(0))
//                {
//                    if (pathSetFinishCallback != null)
//                        pathSetFinishCallback();
//                    isDraggingPath = false;
//                }
//            }
//        }
//    } 

//    void UpdateMoveMarkers()
//    {
//        // do not bother to do anything if the unit got no moves left
////        if (activeUnit.movesLeft == 0) return;
//
//        // first find out which nodes the unit could move to and keep that 
//        // list around to later use when checking what tile the player click
//        List<MapNavNode> validMoveNodes = NodesAround<Sample4Tile>(activeUnit.tile, activeUnit.movesLeft, OnNodeCostCallback);
//
//        // In this example I will show a border to indicate the valid movement area
//        // I need to pass a list of the valid node to GetBroderNodes() and it will give back
//        // a list of those nodes that are neighbouring invalid nodes
//
//        // I suggest you have a look at the GetBorderNodes() function and simply copy its
//        // code and create your own variation that can instantiate the borders in the same loop
//        // that the border node is discovered since the code below will basically copy the inner
//        // loop of the function. (for a little optimization of this)
//        List<Sample4Tile> borderNodes = map.GetBorderNodes<Sample4Tile>(validMoveNodes, CheckIfValidNode);
//
//        // now create the border objects. might be better to use a object cache solution
//        // with this or you might even want to mesh combine the result if needed
//        for (int i = 0; i < borderNodes.Count; i++)
//        {
//            // I run through all the border nodes and then check the neighbours for each to find out
//            // in what direction the placed border object should be rotated to create the correct border
//            // Each hexa node has 6 neighbours. An invalid neighbour will be marked as null
//            List<Sample4Tile> neighbours = map.NodesAround<Sample4Tile>(borderNodes[i], true, false, CheckIfValidNode);
//            for (int j = 0; j < neighbours.Count; j++)
//            {
//                // each hexa node has 6 neighbours.
//                // an invalid neighbour will be null or not in the list of valid nodes.
//                // also check that the node is not the one the active unit is on that
//                // a border is not drawn around the selected unit's tile
//                if (neighbours[j] != activeUnit.tile && (neighbours[j] == null || false == validMoveNodes.Contains(neighbours[j])))
//                {
//                    GameObject go = (GameObject)Instantiate(borderFab);
//                    go.transform.position = borderNodes[i].position + new Vector3(0f, 0.1f, 0f);
//                    go.transform.rotation = Quaternion.Euler(0f, BorderRotations[j], 0f);
//                    go.transform.parent = bordersContainer;
//                }
//            }
//        }
//    }

//    public void SetDraggingCameraMode(bool isDraggingCam)
//    {
//        this.isDraggingCam = isDraggingCam;
//        isDraggingPath = isDraggingCam;
//    }

    public GameObject GetNodeObj(int idx)
    {
        return gameObject.transform.GetChild(idx).gameObject;
    }

    public GameObject GetNodeObj(int x, int y)
    {
        return GetNodeObj(x+y*mapHorizontalSize);
    }

    void BumpNodeMountToHeightMap(int x, int y)
    {
        Vector3 nodePosition = NodeAt<MapNavNode>(x, y).position;
        Vector3 terrainPosition = mainTerrain.transform.position;

        int heightMapX = (int)(((nodePosition.x - 6 * heightMapPointDist - terrainPosition.x)/terrainWidth*mainTerrainData.heightmapResolution));
        int heightMapY = (int)(((nodePosition.z - 6 * heightMapPointDist - terrainPosition.z)/terrainWidth*mainTerrainData.heightmapResolution));

        float[,] oldHeights = mainTerrainData.GetHeights(heightMapX, heightMapY,12,12);
        float[,] newHeights=new float[12,12];
        for (int i = 0; i < oldHeights.GetLength(0); i++)
        {
            for (int j = 0; j < oldHeights.GetLength(1); j++)
            {
                newHeights[i, j] = oldHeights[i, j] + mountainHeightData_12[i, j];
            }
        }

        mainTerrainData.SetHeights(heightMapX, heightMapY, newHeights);



    }

    void GenMountToHeightMap()
    {
        for(int i=0;i<gameInfo.map_info.landform.Count;i++)
        {
            int x = i % mapHorizontalSize;
            int y = i / mapVerticalSize;

            //将山地与山林的地形抬高
            if (gameInfo.map_info.landform[i] == 2 || gameInfo.map_info.landform[i] == 4)
            {
                BumpNodeMountToHeightMap(x,y);
            }
        }


    }

    void SetNodeView()
    {
        for (int i = 0; i < gameInfo.map_info.landform.Count; i++)
        {
            GameObject nodeObj = GetNodeObj(i);
            Transform landformT=nodeObj.transform.FindChild("Landform");
            Transform resourceT = nodeObj.transform.FindChild("Resource");
            Transform shadowT = nodeObj.transform.FindChild("Shadow");
            Transform outsightMask = nodeObj.transform.FindChild("OutsightMask");

            switch (gameInfo.map_info.landform[i])
            {
                case 0:
                    shadowT.gameObject.SetActive(true);
                    break;
                case 1:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(false);
                    break;
                case 2:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(true);
                    landformT.GetComponent<MeshRenderer>().material = hillMat;
                    break;
                case 3:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(true);
                    landformT.GetComponent<MeshRenderer>().material = mountainMat;
                    break;
            }

            //resource_map长度和landform_map相同，此处直接使用
            switch (gameInfo.map_info.resource[i])
            {
                case 1:
                    resourceT.gameObject.SetActive(false);
                    break;
                case 2:
                    resourceT.gameObject.SetActive(true);
                    resourceT.GetComponent<MeshRenderer>().material = forestMat;
                    break;
                case 3:
                    resourceT.gameObject.SetActive(true);
                    resourceT.GetComponent<MeshRenderer>().material = grassMat;
                    break;
            }

            if (gameInfo.map_info.sightzoon.IndexOf(i) == -1)
            {
                outsightMask.gameObject.SetActive(true);
            }
            else
            {
                outsightMask.gameObject.SetActive(false);
            }

        }
    }



    void OnDoMapUpdate(DoMapUpdateSignal.Param param)
    {
        foreach (int pos_id in param.landformList.Keys)
        {
            GameObject nodeObj = GetNodeObj(pos_id);
            Transform landformT = nodeObj.transform.FindChild("Landform");
            Transform shadowT = nodeObj.transform.Find("Shadow");

            switch (param.landformList[pos_id])
            {
                //case 0:
                //    shadowT.gameObject.SetActive(true);
                //    break;
                case 1:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(false);
                    break;
                case 2:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(true);
                    landformT.GetComponent<MeshRenderer>().material = hillMat;
                    break;
                case 3:
                    shadowT.gameObject.SetActive(false);
                    landformT.gameObject.SetActive(true);
                    landformT.GetComponent<MeshRenderer>().material = mountainMat;
                    break;
            }
                    

        }

        foreach (int pos_id in param.resourceList.Keys)
        {

            GameObject nodeObj = GetNodeObj(pos_id);
            Transform resourceT = nodeObj.transform.Find("Resource");
            switch (param.resourceList[pos_id])
            {
                case 1:
                    resourceT.gameObject.SetActive(false);
                    break;
                case 2:
                    resourceT.gameObject.SetActive(true);
                    resourceT.GetComponent<MeshRenderer>().material = forestMat;
                    break;
                case 3:
                    resourceT.gameObject.SetActive(true);
                    resourceT.GetComponent<MeshRenderer>().material = grassMat;
                    break;
            }
        }
    }

    void OnDoSightzoonUpdateSignal(Dictionary<int,int> landform_map)
    {
        ClearSightzoon();
        gameInfo.map_info.sightzoon.Clear();
        foreach (int pos_id in landform_map.Keys)
        {
            gameInfo.map_info.sightzoon.Add(pos_id);
        }
        SetSightzoon();
    }

    void ClearSightzoon()
    {
        foreach (int pos_id in gameInfo.map_info.sightzoon)
        {
            GameObject outsightMaskObj = GetNodeObj(pos_id).transform.FindChild("OutsightMask").gameObject;
            outsightMaskObj.SetActive(true);
        }
    }

    void SetSightzoon()
    {
        foreach (int pos_id in gameInfo.map_info.sightzoon)
        {
            GameObject outsightMaskObj = GetNodeObj(pos_id).transform.FindChild("OutsightMask").gameObject;
            outsightMaskObj.SetActive(false);
        }
    }

    void SetMoveMarker(List<int> pos_id_list,bool visible)
    {
        foreach (int pos_id in pos_id_list)
        {
            GameObject nodeObj = GetNodeObj(pos_id);
            Transform moveMarker = nodeObj.transform.FindChild("MoveMarker");
            moveMarker.gameObject.SetActive(visible);
        }
    }

    public void SetNodeSelect(int pos_id)//,bool visible)
    {
        if (selected_pos_id != -1)
        {
            GameObject nodeObj = GetNodeObj(selected_pos_id);
            Transform selectBorder = nodeObj.transform.FindChild("SelectBorder");
            selectBorder.gameObject.SetActive(false);
        }

        
        selected_pos_id = pos_id;


        if (selected_pos_id != -1)
        {
            GameObject nodeObj = GetNodeObj(selected_pos_id);
            Transform selectBorder = nodeObj.transform.FindChild("SelectBorder");
            selectBorder.gameObject.SetActive(true);
        }
    }

    //public void SetColor(int directionid,int idx)
    //{
    //    switch (directionid)
    //    {
    //        case 1:
    //            gameObject.transform.GetChild(idx).GetComponent<Renderer>().material = moveNodeMat;
    //            break;
    //        case 2:
    //            gameObject.transform.GetChild(idx).GetComponent<Renderer>().material = exploreNodeMat;
    //            break;
    //        case 3:
    //            gameObject.transform.GetChild(idx).GetComponent<Renderer>().material = attackNodeMat;
    //            break;
    //        case 4:
    //            gameObject.transform.GetChild(idx).GetComponent<Renderer>().material = defendNodeMat;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    void OnDistroy()
    {
        doMapUpdateSignal.RemoveListener(OnDoMapUpdate);
        //mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
        doSightzoonUpdateSignal.RemoveListener(OnDoSightzoonUpdateSignal);

        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
        nextturnSignal.RemoveListener(OnNextturnSignal);
        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
    }


    #region nav map overrides

    public override void OnGridChanged(bool created)
    {
        // The parent object that will hold all the instantiated tile objects
        Transform parent = gameObject.transform;

        // Remove existing tiles and place new ones if map was (re)created
        // since the number of tiles might be different now
        if (created)
        {
            //selectedNode = null;
            //altSelectedNode = null;
            //invalidNodes.Clear();
            //ClearMarkedNodes();
            //GeneratePath();

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {	// was called at runtime
                    Destroy(parent.GetChild(i).gameObject);
                }
                else
                {	// was called from editor
                    DestroyImmediate(parent.GetChild(i).gameObject);
                }
            }

            // Place tiles according to the generated grid
            for (int idx = 0; idx < grid.Length; idx++)
            {
                // make sure it is a valid node before placing tile here
                if (false == grid[idx].isValid) continue;

                // create a new tile
                GameObject go = (GameObject)Instantiate(nodeObj);
                go.name = "Tile " + idx.ToString();
                go.transform.position = grid[idx].position;// +new Vector3(0, 1, 0);
                go.transform.parent = parent;
            }

        }

        // else, simply update the position of existing tiles
        else
        {
            for (int idx = 0; idx < grid.Length; idx++)
            {
                // make sure it is a valid node before processing it
                if (false == grid[idx].isValid) continue;

                // Since I gave the tiles proper names I can easily find them by name
                GameObject go = parent.Find("Tile " + idx.ToString()).gameObject;
                go.transform.position = grid[idx].position;
            }
        }
    }

    /// <summary> Returns a list of nodes that represents a path from one node to another. An A* algorithm is used
    /// to calculate the path. Return an empty list on error or if the destination node can't be reached. </summary>
    /// <param name="start">    The node where the path should start. </param>
    /// <param name="end">      The node to reach. </param>
    /// <param name="callback"> An optional callback that can return an integer value to indicate the 
    ///                         cost of moving onto the specified node. This callback should return 1
    ///                         for normal nodes and 2+ for higher cost to move onto the node and 0
    ///                         if the node can't be moved onto; for example when the node is occupied. </param>
    /// <returns> Return an empty list on error or if the destination node can't be reached. </returns>
    public override List<T> Path<T>(MapNavNode start, MapNavNode end, NodeCostCallback callback)
    //        where T : MapNavNode
    {
        if (start == null || end == null) return new List<T>(0);
        if (start.idx == end.idx) return new List<T>(0);

        List<T> path = new List<T>();
        int current = -1;
        int next = -1;
        float new_cost = 0;
        float next_cost = 0;
        double priority = 0;

        // first check if not direct neighbour and get out early
        List<int> neighbors = PathNodeIndicesAround(start.idx); // NodeIndicesAround(start.idx, false, false, null);
        if (neighbors != null)
        {
            if (neighbors.Contains(end.idx))
            {
                if (callback != null)
                {
                    next_cost = callback(start, end);
                    if (next_cost >= 1) path.Add((T)end);
                }
                return path;
            }
        }

        HeapPriorityQueue<PriorityQueueNode> frontier = new HeapPriorityQueue<PriorityQueueNode>(grid.Length);
        frontier.Enqueue(new PriorityQueueNode() { idx = start.idx }, 0);
        Dictionary<int, int> came_from = new Dictionary<int, int>(); // <for_idx, came_from_idx>
        Dictionary<int, float> cost_so_far = new Dictionary<int, float>(); // <idx, cost>
        came_from.Add(start.idx, -1);
        cost_so_far.Add(start.idx, 0);

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue().idx;
            if (current == end.idx) break;

            neighbors = PathNodeIndicesAround(current); //NodeIndicesAround(current, false, false, null);
            if (neighbors != null)
            {
                for (int i = 0; i < neighbors.Count; i++)
                {
                    next = neighbors[i];
                    if (callback != null) next_cost = callback(grid[current], grid[next]);
                    if (next_cost <= 0.0f) continue;

                    new_cost = cost_so_far[current] + next_cost;
                    if (false == cost_so_far.ContainsKey(next)) cost_so_far.Add(next, new_cost + 1);
                    if (new_cost < cost_so_far[next])
                    {
                        cost_so_far[next] = new_cost;
                        priority = new_cost + Heuristic(start.idx, end.idx, next);
                        frontier.Enqueue(new PriorityQueueNode() { idx = next }, priority);
                        if (false == came_from.ContainsKey(next)) came_from.Add(next, current);
                        else came_from[next] = current;
                    }
                }
            }
        }

        // build path
        next = end.idx;
        while (came_from.ContainsKey(next))
        {
            if (came_from[next] == -1) break;
            if (came_from[next] == start.idx) break;
            path.Add((T)grid[came_from[next]]);
            next = came_from[next];
        }

        if (path.Count > 0)
        {
            path.Reverse();
            path.Add((T)end);
        }

        return path;
    }


    /// <summary> This returns a list of nodes around the given node, using the notion of "movement costs" to determine
    /// if a node should be included in the returned list or not. The callback can be used to specify the "cost" to 
    /// reach the specified node, making this useful to select the nodes that a unit might be able to move to. </summary>
    /// <param name="node"> The central node. </param>
    /// <param name="radius"> The maximum area around the node to select nodes from. </param>
    /// <param name="callback"> An optional callback (pass null to not use it) that can be used to indicate the cost of
    /// moving from one node to another. By default it will "cost" 1 to move from one node to another. By returning 0 in 
    /// this callback you can indicate that the target node can't be moved to (for example when the tile is occupied).
    /// Return a value higher than one (like 2 or 3) if moving to the target node would cost more and potentially
    /// exclude the node from the returned list of nodes (when cost to reach it would be bigger than "radius"). </param>
    /// <returns> Returns a list of nodes that can be used with grid[]. Returns empty list (not null) if there was an error. </returns>
    public virtual List<T> NodesAround<T>(MapNavNode node, float radius, NodeCostCallback callback)
        where T : MapNavNode
    {
        List<int> accepted = NodeIndicesAround(node.idx, radius, callback);
        if (accepted.Count > 0)
        {
            List<T> res = new List<T>();
            for (int i = 0; i < accepted.Count; i++) res.Add((T)grid[accepted[i]]);
            return res;
        }
        return new List<T>(0);
    }


    /// <summary> This returns a list of nodes around the given node, using the notion of "movement costs" to determine
    /// if a node should be included in the returned list or not. The callback can be used to specify the "cost" to 
    /// reach the specified node, making this useful to select the nodes that a unit might be able to move to. </summary>
    /// <param name="nodeIdx"> The central node's index. </param>
    /// <param name="radius"> The maximum area around the node to select nodes from. </param>
    /// <param name="callback"> An optional callback (pass null to not use it) that can be used to indicate the cost of
    /// moving from one node to another. By default it will "cost" 1 to move from one node to another. By returning 0 in 
    /// this callback you can indicate that the target node can't be moved to (for example when the tile is occupied).
    /// Return a value higher than one (like 2 or 3) if moving to the target node would cost more and potentially
    /// exclude the node from the returned list of nodes (when cost to reach it would be bigger than "radius"). </param>
    /// <returns> Returns a list of node indices that can be used with grid[]. Returns empty list (not null) if there was an error. </returns>
    public virtual List<int> NodeIndicesAround(int nodeIdx, float radius, NodeCostCallback callback)
    {
        List<int> accepted = new List<int>(); // accepted nodes
        Dictionary<int, float> costs = new Dictionary<int, float>(); // <idx, cost> - used to track which nodes have been checked
        CheckNodesRecursive(nodeIdx, radius, callback, -1, 0, ref accepted, ref costs);
        return accepted;
    }




    /// <summary> This is a Helper for NodeIndicesAround(int idx, int radius, bool includeCentralNode, ValidationCallback callback) </summary>
    protected virtual void CheckNodesRecursive(int idx, float radius, NodeCostCallback callback, int cameFrom, float currDepth, ref List<int> accepted, ref Dictionary<int, float> costs)
    {
        List<int> ids = _neighbours(idx);
        for (int i = 0; i < ids.Count; i++)
        {
            // skip if came into this function from this node. no point in testing
            // against the node that caused this one to be called for checking
            if (cameFrom == ids[i]) continue;

            // get cost to move to the node
            float res = callback == null ? 1f : callback(grid[idx], grid[ids[i]]);

            // can move to node?
            if (res <= 0.0f) continue;

            // how much would it cost in total?
            float d = currDepth + res;

            // too much to reach node?
            if (d > radius) continue;

            // this neighbour node can be moved to, add it to the accepted list if not yet present
            if (false == accepted.Contains(ids[i])) accepted.Add(ids[i]);

            // do not bother to check the node's neighbours if already reached the max 
            if (d == radius) continue;

            // check if should look into the neighbours of this node
            if (costs.ContainsKey(ids[i]))
            {
                // if the new total cost is higher than previously checked then skip this neighbour 
                // since testing with the higher costs will not change any results when checking 
                // the neighbours of this neighbour node
                if (costs[ids[i]] <= d) continue;
            }
            else costs.Add(ids[i], d);

            // update the cost to move to this node
            costs[ids[i]] = d;

            // and test its neighbours for possible valid nodes
            CheckNodesRecursive(ids[i], radius, callback, idx, d, ref accepted, ref costs);
        }
    }


    public float NodeCostCallback(MapNavNode fromNode, MapNavNode toNode)
    {
        Dictionary<int, DLandform> dLandformDic = dGameDataCollection.dLandformCollection.dLandformDic;

        float fromCost=dLandformDic[gameInfo.map_info.landform[fromNode.idx]].cost;
        float toCost = dLandformDic[gameInfo.map_info.landform[toNode.idx]].cost;

        //if(toCost==0f)
        //{
        //    return 0f;
        //}
        //else
        //{
        //    return fromCost + toCost ;
        //}

        return toCost;
    }

    #endregion

    #region strange view impl
    /// Leave this value true most of the time. If for some reason you want
    /// a view to exist outside a context you can set it to false. The only
    /// difference is whether an error gets generated.
    private bool _requiresContext = true;
    public bool requiresContext
    {
        get
        {
            return _requiresContext;
        }
        set
        {
            _requiresContext = value;
        }
    }

    /// A flag for allowing the View to register with the Context
    /// In general you can ignore this. But some developers have asked for a way of disabling
    ///  View registration with a checkbox from Unity, so here it is.
    /// If you want to expose this capability either
    /// (1) uncomment the commented-out line immediately below, or
    /// (2) subclass View and override the autoRegisterWithContext method using your own custom (public) field.
    //[SerializeField]
    protected bool registerWithContext = true;
    virtual public bool autoRegisterWithContext
    {
        get { return registerWithContext; }
        set { registerWithContext = value; }
    }

    public bool registeredWithContext { get; set; }

    /// A MonoBehaviour Awake handler.
    /// The View will attempt to connect to the Context at this moment.
    protected virtual void Awake()
    {
        if (autoRegisterWithContext && !registeredWithContext)
            bubbleToContext(this, true, false);
    }

    /// A MonoBehaviour Start handler
    /// If the View is not yet registered with the Context, it will 
    /// attempt to connect again at this moment.
    protected virtual void Start()
    {
        if (autoRegisterWithContext && !registeredWithContext)
            bubbleToContext(this, true, true);
    }

    /// A MonoBehaviour OnDestroy handler
    /// The View will inform the Context that it is about to be
    /// destroyed.
    protected virtual void OnDestroy()
    {
        bubbleToContext(this, false, false);
    }

    /// Recurses through Transform.parent to find the GameObject to which ContextView is attached
    /// Has a loop limit of 100 levels.
    /// By default, raises an Exception if no Context is found.
    virtual protected void bubbleToContext(MonoBehaviour view, bool toAdd, bool finalTry)
    {
        const int LOOP_MAX = 100;
        int loopLimiter = 0;
        Transform trans = view.gameObject.transform;
        while (trans.parent != null && loopLimiter < LOOP_MAX)
        {
            loopLimiter++;
            trans = trans.parent;
            if (trans.gameObject.GetComponent<ContextView>() != null)
            {
                ContextView contextView = trans.gameObject.GetComponent<ContextView>() as ContextView;
                if (contextView.context != null)
                {
                    IContext context = contextView.context;
                    if (toAdd)
                    {
                        context.AddView(view);
                        registeredWithContext = true;
                        return;
                    }
                    else
                    {
                        context.RemoveView(view);
                        return;
                    }
                }
            }
        }
        if (requiresContext && finalTry)
        {
            //last ditch. If there's a Context anywhere, we'll use it!
            if (Context.firstContext != null)
            {
                Context.firstContext.AddView(view);
                registeredWithContext = true;
                return;
            }


            string msg = (loopLimiter == LOOP_MAX) ?
                msg = "A view couldn't find a context. Loop limit reached." :
                    msg = "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose.";
            msg += "\nView: " + view.ToString();
            throw new MediationException(msg,
                MediationExceptionType.NO_CONTEXT);
        }
    }




   
    #endregion

}
