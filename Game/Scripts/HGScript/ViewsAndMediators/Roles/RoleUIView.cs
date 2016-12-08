using System;
using System.Collections.Generic;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using MapNavKit;

public class RoleUIView:View
{
    public MainContext mainContext;
    
    public Text roleName;
    public Slider blood;
    public GameObject directionsRoot;
    public GameObject directionTpl;
    public GameObject cancelPathSelect;

    

    //public GameObject pathRoot;

    //public Material normalNodeMat;
    //public Material exploreNodeMat;
    //public Material attackNodeMat;
    //public Material defendNodeMat;
    //public Material moveNodeMat;
    


    RectTransform roleUIRectTransform;
    
    
    GameObject roleObj;

    GameInfo gameInfo;
    DGameDataCollection dGameDataCollection;
    GameDataService gameDataService;
    ResourceService resourceService;
    DirectionClickSignal directionClickSignal;
    DirectionClickCallbackSignal directionClickCallbackSignal;
    MapNodeSelectSignal mapNodeSelectSignal;
    ActionAnimStartSignal actionAnimStartSignal;
    //UpdateDirectionPathCallbackSignal updateDirectionPathCallbackSignal;

    //public Action cancelPathSelectCallback;

    //int playerid;
    int roleid;

    public void Init(MainContext mainContext, GameObject roleObj, GameInfo gameInfo, DGameDataCollection dGameDataCollection,
        int roleid, GameDataService gameDataService, ResourceService resourceService, DirectionClickSignal directionClickSignal,
        DirectionClickCallbackSignal directionClickCallbackSignal, MapNodeSelectSignal mapNodeSelectSignal, ActionAnimStartSignal actionAnimStartSignal)
    {
        this.requiresContext = false;
        roleUIRectTransform = GetComponent<RectTransform>();
        this.mainContext = mainContext;
        this.roleObj = roleObj;
        this.gameInfo = gameInfo;
        this.dGameDataCollection = dGameDataCollection;
        //this.playerid = playerid;
        this.roleid = roleid;
        this.gameDataService = gameDataService;
        this.resourceService = resourceService;
        this.directionClickSignal = directionClickSignal;
        this.directionClickCallbackSignal = directionClickCallbackSignal;
        this.mapNodeSelectSignal = mapNodeSelectSignal;
        this.actionAnimStartSignal = actionAnimStartSignal;
        //this.updateDirectionPathCallbackSignal = updateDirectionPathCallbackSignal;

        
        transform.SetParent(mainContext.uiCanvas.transform);
        roleUIRectTransform.anchoredPosition3D = Vector3.zero;
        roleUIRectTransform.localScale = Vector3.one;
        roleUIRectTransform.localRotation = Quaternion.identity;

        this.mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);
        this.directionClickCallbackSignal.AddListener(OnDirectionClickCallbackSignal);
        this.actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        //this.updateDirectionPathCallbackSignal.AddListener(OnUpdateDirectionPathCallbackSignal);

        cancelPathSelect.GetComponentInChildren<Button>().onClick.AddListener(() => {
            //cancelPathSelect.SetActive(false);
            //if (cancelPathSelectCallback != null)
            //{
            //    cancelPathSelectCallback();
            //}
            SetNormalUIVisible(true);
        });

        UpdateName();
        UpdateDirections();
    }


    void LateUpdate()
    {
        roleUIRectTransform.anchoredPosition = mainContext.uiCamera.WorldToScreenPoint(roleObj.transform.position);
    }

    //根据gameInfo初始化按钮视图，并返回按钮脚本列表
    public List<Button> UpdateDirections()
    {
        List<int> directionList = gameDataService.GetAllDirectionIds(roleid); 
        directionTpl.transform.SetParent(null);
        Tools.ClearChildren(directionsRoot.transform);

        List<Button> btns = new List<Button>();
        foreach (int directionid in directionList)
        {
            DDirection ddirection = dGameDataCollection.dDirectionCollection.dDirectionDic[directionid];
            GameObject ddirectionObj = GameObject.Instantiate(directionTpl);
            ddirectionObj.transform.SetParent(directionsRoot.transform);
            ddirectionObj.transform.localPosition = Vector3.zero;
            ddirectionObj.transform.Find("Text").GetComponent<Text>().text = ddirection.name;
            ddirectionObj.SetActive(true);


            btns.Add(ddirectionObj.GetComponent<Button>());
            int id = directionid;
            ddirectionObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                //Debug.Log("direction click!");
                //StartCoroutine(WaitForOneFrame());
                directionClickSignal.Dispatch(new DirectionClickSignal.Param(roleid,id));
                
            });
        }
        directionTpl.transform.SetParent(directionsRoot.transform);
        directionTpl.SetActive(false);
        //directionTpl.transform.localPosition = Vector3.zero;
        //directionTpl.GetComponent<Button>().onClick.RemoveAllListeners();
        //directionTpl.GetComponent<Button>().onClick.AddListener(() => { SetDirectionsRootVisible(false); });
        return btns;
    }

    void SetNormalUIVisible(bool visible)
    {
        //roleName.gameObject.SetActive(visible);
        //blood.gameObject.SetActive(visible);
        directionsRoot.SetActive(visible);
        cancelPathSelect.SetActive(!visible);
    }

    void SetAllUIVisible(bool visible)
    {
        //roleName.gameObject.SetActive(visible);
        //blood.gameObject.SetActive(visible);
        directionsRoot.SetActive(visible);
        cancelPathSelect.SetActive(visible);
    }


    void OnDirectionClickCallbackSignal(DirectionClickSignal.Param param)
    {
        if (param.roleid == roleid)
        {
            SetNormalUIVisible(false);
        }
        else
        {
            SetAllUIVisible(false);
        }
        //directionsRoot.SetActive(visible);
        //cancelPathSelect.SetActive(true);
    }

    void OnMapNodeSelectSignal(MapNavNode mapNavNode)
    {
        if (gameDataService.GetRoleInMap(mapNavNode.idx) == null)
        {
            SetAllUIVisible(false);
            //pathRoot.gameObject.SetActive(false);
        }
        else if (gameDataService.GetRoleInMap(mapNavNode.idx).roleid == roleid)
        {
            //Debug.Log(roleid);
            SetNormalUIVisible(true);
            //pathRoot.gameObject.SetActive(true);
        }
        else
        {
            SetAllUIVisible(false);
            //pathRoot.gameObject.SetActive(false);
        }
    }

    void OnActionAnimStartSignal()
    {
        SetAllUIVisible(false);
    }

    


    public void UpdateName()
    {
        //roleName.text = dGameDataCollection.dRoleCollection.dRoleDic[gameInfo.allplayers[playerid].role_dic[roleid].did].name;
        roleName.text = dGameDataCollection.dRoleCollection.dRoleDic[gameInfo.role_dic[roleid].roledid].name;//roleid.ToString();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
        directionClickCallbackSignal.RemoveListener(OnDirectionClickCallbackSignal);
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
    }
}

