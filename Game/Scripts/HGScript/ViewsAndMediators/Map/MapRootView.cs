using UnityEngine;
using System.Collections;
using MapNavKit;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;

public class MapRootView : MapNavHexa,IView {
    public GameObject nodeObj;
    TerrainData mainTerrainData;
    public Terrain mainTerrain;
    public Camera gameCamera;
    public GameObject gameAndUICamerasRoot;
    public Camera overviewCamera;

    public Material stoneMat;
    public Material forestMat;

    

    bool isPressed = false;

    //两者意义不同，并不严格相反，当isDraggingCameraMode刚刚变为false时，isDraggingPath仍然为false，因为尚未开始拖拽
    bool isDraggingCameraMode = true;
    bool isDraggingPath = false;

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

    //[Inject]
    //public PathSetFinishedSignal pathSetFinishedSignal { get; set; }

    public System.Action pathSetFinishCallback;

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


    
    float width, length, hori, vert;//, offs;

    float terrainWidth;

    float[,] mountainHeightData_12;

    float heightMapPointDist;
    

    public void Init(GameInfo gameInfo)
    {
        mainTerrainData = mainTerrain.terrainData;

        //Debug.Log(gameInfo.mapInfo.width);

        mapHorizontalSize = gameInfo.map_info.width;
        mapVerticalSize = gameInfo.map_info.height;
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
        //GenMountToHeightMap();
        
    }


    protected void LateUpdate()
    {
        //if (Input.touchCount == 0)
        //{
        //    return;
        //}

        //如果鼠标或者手势在ui元素上方，则不处理
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        //查找鼠标射线与terrain的碰撞点处的node
        RaycastHit hit;
        //Ray r = gameCamera.ScreenPointToRay(Input.GetTouch(0).position);//(Input.mousePosition);
        Ray r = gameCamera.ScreenPointToRay(Input.mousePosition);   
        if (Physics.Raycast(r, out hit,100f,1<<LayerMask.NameToLayer("Terrain")))
        {
            thisHit = hit.point;
            //if (Input.GetTouch(0).phase == TouchPhase.Began)//Input.GetMouseButtonDown(0))
            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                startHit = hit.point;
                startCamPos = gameAndUICamerasRoot.transform.position;

            }
            //if (Input.GetTouch(0).phase == TouchPhase.Ended)//(Input.GetMouseButtonUp(0))
            if (Input.GetMouseButtonUp(0))
            {
                isPressed = false;
            }
            Vector3 delta = thisHit - startHit;              
            MapNavNode n = NodeAtWorldPosition<MapNavNode>(hit.point);

            //拖拽移动摄像机模式
            if (isDraggingCameraMode)
            {
                if (isPressed)
                {
                    gameAndUICamerasRoot.transform.position = new Vector3(gameAndUICamerasRoot.transform.position.x - delta.x, gameAndUICamerasRoot.transform.position.y, gameAndUICamerasRoot.transform.position.z - delta.z);

                }
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 camTrans = gameAndUICamerasRoot.transform.position - startCamPos;
                    if (camTrans.x * camTrans.x + camTrans.z * camTrans.z < clickBoundWithinMove)
                    {
                        mapNodeSelectSignal.Dispatch(n);
                    }
                }
            }
            //拖拽设置路径模式
            else
            {
                ////如果鼠标或者手势在ui元素上方，则不处理
                //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                //{
                //    return;
                //}
                //if (Input.GetTouch(0).phase == TouchPhase.Began)
                if (Input.GetMouseButtonDown(0))
                {
                    //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                }
                //if (Input.GetTouch(0).phase == TouchPhase.Move)
                if (Input.GetMouseButton(0))
                {
                    isDraggingPath = true;

                    MapNavNode enteredNode = n;
                    if (lastEnteredNode != enteredNode)
                    {
                        //Debug.Log("enter:" + enteredNode.q + "," + enteredNode.r);
                        if (OnMouseEnter != null)
                        {
                            OnMouseEnter(lastEnteredNode,enteredNode);

                        }
                        lastEnteredNode = enteredNode;
                    }

                    //第二个手指此时可以拖动摄像机
                    if (Input.touchCount > 1)// && Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        RaycastHit hit2;
                        Ray r2 = gameCamera.ScreenPointToRay(Input.GetTouch(1).position);

                        if (Physics.Raycast(r2, out hit2, 100f, 1 << LayerMask.NameToLayer("Terrain")))
                        {
                            thisHit2 = hit2.point;
                            if (Input.GetTouch(1).phase == TouchPhase.Began)
                            {
                                startHit2 = hit2.point;
                            }
                            if (Input.GetTouch(1).phase == TouchPhase.Moved)
                            {
                                Vector3 delta2 = thisHit2 - startHit2;
                                gameAndUICamerasRoot.transform.position = new Vector3(gameAndUICamerasRoot.transform.position.x - delta2.x, gameAndUICamerasRoot.transform.position.y, gameAndUICamerasRoot.transform.position.z - delta2.z);

                            }
                        }
                    }

                }
                else
                {
                    lastEnteredNode = null;
                }
                if (isDraggingPath&&Input.GetMouseButtonUp(0))
                {
                    if (pathSetFinishCallback != null)
                        pathSetFinishCallback();
                    isDraggingPath = false;
                }
            }
        }
    }

    public void SetDraggingCameraMode(bool isDraggingCameraMode)
    {
        this.isDraggingCameraMode = isDraggingCameraMode;
        isDraggingPath = isDraggingCameraMode;
    }

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
        for(int i=0;i<gameInfo.map_info.landform_map.Count;i++)
        {
            int x = i % gameInfo.map_info.width;
            int y = i / gameInfo.map_info.width;

            //将山地与山林的地形抬高
            if (gameInfo.map_info.landform_map[i] == 2 || gameInfo.map_info.landform_map[i] == 4)
            {
                BumpNodeMountToHeightMap(x,y);
            }
        }


    }

    void SetNodeView()
    {
        for (int i = 0; i < gameInfo.map_info.landform_map.Count; i++)
        {
            GameObject nodeObj = GetNodeObj(i);
            Transform hillT=nodeObj.transform.Find("Hill");
            Transform resourceT = nodeObj.transform.Find("Resource");
            Transform shadowT = nodeObj.transform.Find("Shadow");

            switch (gameInfo.map_info.landform_map[i])
            {
                case 0:
                    shadowT.gameObject.SetActive(true);
                    break;
                case 1:
                    shadowT.gameObject.SetActive(false);
                    hillT.gameObject.SetActive(false);
                    break;
                case 2:
                    shadowT.gameObject.SetActive(false);
                    hillT.gameObject.SetActive(true);
                    break;
            }

            //resource_map长度和landform_map相同，此处直接使用
            switch (gameInfo.map_info.resource_map[i])
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
                    resourceT.GetComponent<MeshRenderer>().material = stoneMat;
                    break;
            }

        }
    }

    void OnDoMapUpdate(DoMapUpdateSignal.Param param)
    {
        foreach (int pos_id in param.landformList.Keys)
        {
            GameObject nodeObj = GetNodeObj(pos_id);
            Transform hillT = nodeObj.transform.Find("Hill");
            Transform resourceT = nodeObj.transform.Find("Resource");
            Transform shadowT = nodeObj.transform.Find("Shadow");

            switch (param.landformList[pos_id])
            {
                case 0:
                    shadowT.gameObject.SetActive(true);
                    break;
                case 1:
                    shadowT.gameObject.SetActive(false);
                    hillT.gameObject.SetActive(false);
                    break;
                case 2:
                    shadowT.gameObject.SetActive(false);
                    hillT.gameObject.SetActive(true);
                    break;
            }

            //resource_map长度、key和landform_map相同，此处直接使用
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
                    resourceT.GetComponent<MeshRenderer>().material = stoneMat;
                    break;
            }
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
    }

    


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
                    Object.Destroy(parent.GetChild(i).gameObject);
                }
                else
                {	// was called from editor
                    Object.DestroyImmediate(parent.GetChild(i).gameObject);
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
