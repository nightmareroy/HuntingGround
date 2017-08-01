using System;
using System.Collections.Generic;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;
using MapNavKit;

public class RoleUIView:View
{
    //[Inject]
    //public SPlayerInfo sPlayerInfo { get; set;}

    

    public MainContext mainContext;
    
    public Text roleName;
    public Image roleNameBack;
    public Slider blood;
    public GameObject directionsRoot;
    public GameObject directionTpl;
    public GameObject cancelPathSelect;

    //public GameObject flow_up_er;

    public Image directionImg;

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
    ActiveGameDataService activeGameDataService;
    ResourceService resourceService;
//    DirectionClickSignal directionClickSignal;
//    DirectionClickCallbackSignal directionClickCallbackSignal;
    MapNodeSelectSignal mapNodeSelectSignal;
    ActionAnimStartSignal actionAnimStartSignal;
    DoRoleActionAnimSignal doActionAnimSignal;
    FlowUpTipSignal flowUpTipSignal;
    //UpdateDirectionPathCallbackSignal updateDirectionPathCallbackSignal;

    //public Action cancelPathSelectCallback;
    CanvasScaler canvasScaler;

    ColorService colorService;

    IconSpritesService iconSpritesService;

    //int playerid;
    string role_id;

    public void Init(MainContext mainContext, GameObject roleObj, GameInfo gameInfo, DGameDataCollection dGameDataCollection,
        string role_id, ActiveGameDataService activeGameDataService, ResourceService resourceService, 
         MapNodeSelectSignal mapNodeSelectSignal, ActionAnimStartSignal actionAnimStartSignal,
        DoRoleActionAnimSignal doActionAnimSignal, FlowUpTipSignal flowUpTipSignal,ColorService colorService,IconSpritesService iconSpritesService)
    {
        this.requiresContext = false;
        roleUIRectTransform = GetComponent<RectTransform>();
        this.mainContext = mainContext;
        this.roleObj = roleObj;
        this.gameInfo = gameInfo;
        this.dGameDataCollection = dGameDataCollection;
        //this.playerid = playerid;
        this.role_id = role_id;
        this.activeGameDataService = activeGameDataService;
        this.resourceService = resourceService;
//        this.directionClickSignal = directionClickSignal;
//        this.directionClickCallbackSignal = directionClickCallbackSignal;
        this.mapNodeSelectSignal = mapNodeSelectSignal;
        this.actionAnimStartSignal = actionAnimStartSignal;
        //this.updateDirectionPathCallbackSignal = updateDirectionPathCallbackSignal;
        this.doActionAnimSignal=doActionAnimSignal;
        this.flowUpTipSignal = flowUpTipSignal;
        this.colorService = colorService;
        this.iconSpritesService = iconSpritesService;

        canvasScaler = mainContext.uiCanvas.GetComponent<CanvasScaler>();

        
        transform.SetParent(mainContext.uiCanvas.transform.FindChild("RoleUIRoot"));
        roleUIRectTransform.anchoredPosition3D = Vector3.zero;
        roleUIRectTransform.localScale = Vector3.one;
        roleUIRectTransform.localRotation = Quaternion.identity;

//        this.mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);
//        this.directionClickCallbackSignal.AddListener(OnDirectionClickCallbackSignal);
        //this.actionAnimStartSignal.AddListener(OnActionAnimStartSignal);
        //this.updateDirectionPathCallbackSignal.AddListener(OnUpdateDirectionPathCallbackSignal);


        doActionAnimSignal.AddListener(OnDoActionAnimSignal);

        cancelPathSelect.GetComponentInChildren<Button>().onClick.AddListener(() => {
            //cancelPathSelect.SetActive(false);
            //if (cancelPathSelectCallback != null)
            //{
            //    cancelPathSelectCallback();
            //}
            SetNormalUIVisible(true);
        });

//        damageAnim.Stop();
//        recoveryAnim.Stop();

        UpdateNameAndColor();
//        UpdateDirections();
    }


    void LateUpdate()
    {
//        Debug.Log(roleUIRectTransform.anchoredPosition);
//        Debug.Log(roleObj);
        float s_factor = canvasScaler.referenceResolution.x / (float)Screen.width;
        roleUIRectTransform.anchoredPosition = s_factor * mainContext.uiCamera.WorldToScreenPoint(roleObj.transform.position);
//        Debug.Log();
    }

    //根据gameInfo初始化按钮视图，并返回按钮脚本列表
//    public List<Button> UpdateDirections()
//    {
//        List<int> directionList = activeGameDataService.GetAllDirectionDids(role_id); 
//        directionTpl.transform.SetParent(null);
//        Tools.ClearChildren(directionsRoot.transform);
//
//        List<Button> btns = new List<Button>();
//        foreach (int directionid in directionList)
//        {
//            DDirection ddirection = dGameDataCollection.dDirectionCollection.dDirectionDic[directionid];
//            GameObject ddirectionObj = GameObject.Instantiate(directionTpl);
//            ddirectionObj.transform.SetParent(directionsRoot.transform);
//            ddirectionObj.transform.localPosition = Vector3.zero;
//            ddirectionObj.transform.Find("Text").GetComponent<Text>().text = ddirection.name;
//            ddirectionObj.SetActive(true);
//
//
//            btns.Add(ddirectionObj.GetComponent<Button>());
//            int id = directionid;
//            ddirectionObj.GetComponent<Button>().onClick.AddListener(() =>
//            {
//                //Debug.Log("direction click!");
//                //StartCoroutine(WaitForOneFrame());
//                directionClickSignal.Dispatch(new DirectionClickSignal.Param(role_id,id));
//                
//            });
//        }
//        directionTpl.transform.SetParent(directionsRoot.transform);
//        directionTpl.SetActive(false);
//        //directionTpl.transform.localPosition = Vector3.zero;
//        //directionTpl.GetComponent<Button>().onClick.RemoveAllListeners();
//        //directionTpl.GetComponent<Button>().onClick.AddListener(() => { SetDirectionsRootVisible(false); });
//        return btns;
//    }

    void SetNormalUIVisible(bool visible)
    {
        //roleName.gameObject.SetActive(visible);
        blood.gameObject.SetActive(visible);
//        directionsRoot.SetActive(visible);
//        cancelPathSelect.SetActive(!visible);
    }

    void SetAllUIVisible(bool visible)
    {
        //roleName.gameObject.SetActive(visible);
        //blood.gameObject.SetActive(visible);
//        directionsRoot.SetActive(visible);
        cancelPathSelect.SetActive(visible);
    }




//    void OnMapNodeSelectSignal(MapNavNode mapNavNode)
//    {
////        Debug.Log("map");
//        if (mapNavNode != null)
//        {
//            if (activeGameDataService.GetRoleInMap(mapNavNode.idx) == null)
//            {
//                SetNormalUIVisible(false);
//            }
//            else if (activeGameDataService.GetRoleInMap(mapNavNode.idx).role_id == role_id)
//            {
//                SetNormalUIVisible(true);
//            }
//            else
//            {
//                SetNormalUIVisible(false);
//            }
//        }
//        else
//        {
//            SetNormalUIVisible(false);
//        }
////        
//    }

    void OnDoActionAnimSignal(DoRoleActionAnimSignal.Param param)
    {
        if (param.role_id == role_id)
        {
            FlowUpTipSignal.Param flowUpTipSignalParam;

            switch (param.type)
            {
                case 2:
                    Destroy(gameObject);
                    break;
                //case 3:
                //    //flow_up_er.GetComponent<Text>().text = param.value.ToString();
                //    //flow_up_er.GetComponent<Animator>().SetTrigger("damage_trigger");
                //    //

                //    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.blood,-1*param.value);
                //    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                //    blood.value = gameInfo.role_dic[role_id].health;
                //    break;
                //case 4:
                //    //flow_up_er.GetComponent<Text>().text = param.value.ToString();
                //    //flow_up_er.GetComponent<Animator>().SetTrigger("recovery_trigger");
                //    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.blood,param.value);
                //    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                //    blood.value=gameInfo.role_dic[role_id].health;
                //    break;

                case 7:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.blood,param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    blood.value = gameInfo.role_dic[role_id].health;
                    break;

                case 8:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.blood_max,param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;

                case 9:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.muscle,param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 10:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.fat, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 11:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.inteligent, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 12:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.amino_acid, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 13:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.breath, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 14:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.digest, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 15:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.courage, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 16:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.life, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 17:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.skill, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 18:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.cook_skill, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 20:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.meat, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 21:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.banana, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 22:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.ant, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 23:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.egg, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
                case 24:
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform, FlowUpTipSignal.Type.honey, param.value);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;


            }
        }
    }

    void OnActionAnimStartSignal()
    {
        SetAllUIVisible(false);
    }

    


    public void UpdateNameAndColor()
    {
        RoleInfo roleInfo=gameInfo.role_dic[role_id];
        //roleName.text = dGameDataCollection.dRoleCollection.dRoleDic[gameInfo.allplayers[playerid].role_dic[roleid].did].name;
        roleName.text = roleInfo.name;
        roleNameBack.color = colorService.getColor(gameInfo.allplayers_dic[roleInfo.uid].color_index);
        blood.value = roleInfo.health;
    }

    public void SetDirection(int direction_did)
    {
        

        switch (direction_did)
        {
            case 1:
                directionImg.sprite = iconSpritesService.GetView().move;
                break;
            case 2:
                directionImg.sprite = iconSpritesService.GetView().defend;
                break;
            //case 3:
            //    directionImg.sprite = iconSpritesService.GetView().banana;
            //    break;
            case 8:
                directionImg.sprite = iconSpritesService.GetView().food;
                break;
            case 11:
                directionImg.sprite = iconSpritesService.GetView().fead;
                break;
            case 13:
                directionImg.sprite = iconSpritesService.GetView().banana;
                break;
            case 15:
                directionImg.sprite = iconSpritesService.GetView().fin;
                break;

        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
//        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
//        directionClickCallbackSignal.RemoveListener(OnDirectionClickCallbackSignal);
        //actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);

        doActionAnimSignal.RemoveListener(OnDoActionAnimSignal);
    }
}

