﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using MapNavKit;


public class MyRoleListPanelView : View
{
    [Inject]
    public GameInfo gameInfo{ get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}

    [Inject]
    public MapNodeSelectSignal mapNodeSelectSignal{ get; set;}

    [Inject]
    public FindNodeSignal findNodeSignal { get;set; }

    [Inject]
    public ActiveGameDataService activeGameDataService{ get; set;}

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal{ get; set;}

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal{ get; set;}

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal{ get; set;}

    [Inject]
    public UpdateDirectionTurnSignal updateDirectionTurnSignal { get;set; }

    [Inject]
    public IconSpritesService iconSpritesService { get; set; }


    public GameObject scrollViewObj;
    public GameObject contentRootObj;
    public GameObject roleItemTpl;
    public ToggleGroup toggleGroup;

    public MapRootView mapRootView;

    public Button findBtn;
//    public ScrollRect sr;

    bool allToggleChangedListenerEnable=true;

    List<string> assignedRoleList=new List<string>();
    int selecter_id=0;

    public void Init()
    {
        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);

        updateRoleDirectionSignal.AddListener(OnUpdateRoleDirectionSignal);

        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);

        actionAnimFinishSignal.AddListener(OnActionAnimFinishSignal);

        updateDirectionTurnSignal.AddListener(OnUpdateDirectionTurnSignal);

        foreach (string role_id in gameInfo.role_dic.Keys)
        {
            RoleInfo roleInfo = gameInfo.role_dic[role_id];
            if (roleInfo.uid == sPlayerInfo.uid)
            {
                if (roleInfo.direction_did == 1)
                {
                    assignedRoleList.Add(role_id);
                }
            }
        }

        UpdateRoles();
    }

    //public void InitDirections()
    //{

    //}

    public void UpdateRoles()
    {
        Tools.ClearChildren(contentRootObj.transform);
        foreach(string role_id in gameInfo.role_dic.Keys)
        {
            RoleInfo roleInfo = gameInfo.role_dic[role_id];
            if (roleInfo.uid == sPlayerInfo.uid)
            {
                GameObject roleItemObj = GameObject.Instantiate(roleItemTpl as Object) as GameObject;

                roleItemObj.transform.SetParent(contentRootObj.transform);
                roleItemObj.transform.localScale = Vector3.one;
                roleItemObj.transform.localPosition = Vector3.zero;
                roleItemObj.transform.localRotation = Quaternion.identity;
                roleItemObj.name = roleInfo.role_id.ToString();
                roleItemObj.SetActive(true);

                Text name = roleItemObj.transform.FindChild("Name").GetComponent<Text>();
                name.text = roleInfo.name;

                Toggle toggle=roleItemObj.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener((active)=>{
                    if(allToggleChangedListenerEnable)
                    {
                        if(active)
                        {
    //                        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);
                            MapNavNode node = mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id);
                            mapNodeSelectSignal.Dispatch(node);
                            findNodeSignal.Dispatch(node, false);
                        }
                    }
//                    Debug.Log(toggleGroup.ActiveToggles().ToString());
                });
//                toggle.onValueChanged.

                UpdateRoleDirection(role_id, roleInfo.direction_did);
            }
                
        }

//        toggleGroup.
    }

    public void AddRole(string role_id)
    {
        RoleInfo roleInfo = gameInfo.role_dic[role_id];
        if (roleInfo.uid == sPlayerInfo.uid)
        {
            GameObject roleItemObj = GameObject.Instantiate(roleItemTpl as Object) as GameObject;

            roleItemObj.transform.SetParent(contentRootObj.transform);
            roleItemObj.transform.localScale = Vector3.one;
            roleItemObj.transform.localPosition = Vector3.zero;
            roleItemObj.transform.localRotation = Quaternion.identity;
            roleItemObj.name = roleInfo.role_id.ToString();
            roleItemObj.SetActive(true);

            Text name = roleItemObj.transform.FindChild("Name").GetComponent<Text>();
            name.text = roleInfo.name;

            Toggle toggle=roleItemObj.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((active)=>{
                if(allToggleChangedListenerEnable)
                {
                    if(active)
                    {
                        MapNavNode node = mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id);
                        mapNodeSelectSignal.Dispatch(node);
                        findNodeSignal.Dispatch(node, false);

                    }
                }
            });
        }
    }

    public void DeleteRole(string role_id)
    {
        Transform roleItemT = contentRootObj.transform.FindChild(role_id);
        Tools.Destroy(roleItemT);
    }

    void OnMapNodeSelectSignal(MapNavNode n)
    {
        
        if (n != null)
        {
            RoleInfo roleInfo=activeGameDataService.GetRoleInMap(n.idx);
            //选中有角色节点,激活toggle
            if(roleInfo!=null)
            {
                if (roleInfo.uid == sPlayerInfo.uid)
                {
                    Toggle toggle = null;
                    for (int i = 0; i < contentRootObj.transform.childCount; i++)
                    {
                        Transform roleT = contentRootObj.transform.GetChild(i);
                        if (roleT.name == roleInfo.role_id)
                        {
                            toggle = roleT.GetComponent<Toggle>();
                            break;
                        }
                    }
                    if (toggle == null)
                    {
                        return;
                    }
                    if (!toggle.isOn)
                    {
                        allToggleChangedListenerEnable = false;
                        toggle.isOn = true;
                        allToggleChangedListenerEnable = true;
                    }

                    
                    int index = 0;
                    int total = contentRootObj.transform.childCount;
                    for (int i = 0; i < total; i++)
                    {
                        Transform childT = contentRootObj.transform.GetChild(i);
                        if (childT.name == roleInfo.role_id)
                        {
                            index = i;
                            break;
                        }
                    }
                    scrollViewObj.GetComponent<ScrollRect>().verticalNormalizedPosition=1f-(float)index/(float)total;
                    //Debug.Log(roleInfo.name);
                    //Debug.Log(1f - (float)index / (float)total);
                    return;
                }
            }

        }

        //选中无角色节点,将激活的toggle关闭
        for (int i = 0; i < contentRootObj.transform.childCount; i++)
        {
            Transform roleT = contentRootObj.transform.GetChild(i);
            Toggle toggle = roleT.GetComponent<Toggle>();
            if (toggle.isOn)
            {
                allToggleChangedListenerEnable = false;
                toggleGroup.allowSwitchOff = true;
                toggle.isOn = false;
                toggleGroup.allowSwitchOff = false;
                allToggleChangedListenerEnable = true;
                break;
            }
        }




//        mapNodeSelectSignal.AddListener(OnMapNodeSelectSignal);
    }

    public void FindBtn()
    {
//        foreach (string role_id in gameInfo.role_dic.Keys)
//        {
//            RoleInfo roleInfo = gameInfo.role_dic[role_id];
//            if (roleInfo.direction_did == 2)
//            {
//                mapNodeSelectSignal.Dispatch(mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id));
//                break;
//            }
//        }
        if (assignedRoleList.Count > 0)
        {
            //            int r = Random.Range(0, assignedRoleList.Count);
            selecter_id = (selecter_id + 1) % assignedRoleList.Count;
            RoleInfo roleInfo = gameInfo.role_dic[assignedRoleList[selecter_id]];
            MapNavNode node = mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id);
            mapNodeSelectSignal.Dispatch(node);
            findNodeSignal.Dispatch(node, false);
        }
        else
        {
            mapNodeSelectSignal.Dispatch(null);
        }
    }

    void OnUpdateRoleDirectionSignal(string role_id)
    {
        assignedRoleList.Remove(role_id);
        
    }

    void OnActionAnimStartSignal()
    {
        findBtn.interactable = false;
    }

    void OnActionAnimFinishSignal()
    {
        assignedRoleList.Clear();
        foreach (string role_id in gameInfo.role_dic.Keys)
        {
            RoleInfo roleInfo = gameInfo.role_dic[role_id];
            if (roleInfo.uid == sPlayerInfo.uid)
            {
                if (roleInfo.direction_did == 1)
                {
                    assignedRoleList.Add(role_id);
                }
            }
        }

        findBtn.interactable = true;
    }

    void OnUpdateDirectionTurnSignal(int uid)
    {
        findBtn.interactable = true;
    }

    public void UpdateRoleDirection(string role_id, int direction_did)
    {
        Transform roleT = contentRootObj.transform.FindChild(role_id);

        Image directionIcon = roleT.FindChild("DirectionIcon").GetComponent<Image>();

        switch (direction_did)
        {
            case 1:
                directionIcon.sprite = iconSpritesService.GetView().move;
                break;
            case 2:
                directionIcon.sprite = iconSpritesService.GetView().defend;
                break;
            //case 3:
            //    directionIcon.sprite = iconSpritesService.GetView().banana;
            //    break;
            case 8:
                directionIcon.sprite = iconSpritesService.GetView().food;
                break;
            case 11:
                directionIcon.sprite = iconSpritesService.GetView().fead;
                break;
            case 13:
                directionIcon.sprite = iconSpritesService.GetView().banana;
                break;
            case 15:
                directionIcon.sprite = iconSpritesService.GetView().fin;
                break;
            
        }
        //
    }

    

    void OnDestroy()
    {
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);

        updateRoleDirectionSignal.RemoveListener(OnUpdateRoleDirectionSignal);

        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);

        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);

        updateDirectionTurnSignal.RemoveListener(OnUpdateDirectionTurnSignal);
    }
}
