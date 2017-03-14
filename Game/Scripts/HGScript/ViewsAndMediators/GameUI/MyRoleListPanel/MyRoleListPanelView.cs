using UnityEngine;
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
    public ActiveGameDataService activeGameDataService{ get; set;}

    [Inject]
    public UpdateRoleDirectionSignal updateRoleDirectionSignal{ get; set;}

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal{ get; set;}

    [Inject]
    public ActionAnimFinishSignal actionAnimFinishSignal{ get; set;}

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

        foreach (string role_id in gameInfo.role_dic.Keys)
        {
            assignedRoleList.Add(role_id);
        }

        UpdateRoles();
    }

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
                            mapNodeSelectSignal.Dispatch(mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id));

                        }
                    }
//                    Debug.Log(toggleGroup.ActiveToggles().ToString());
                });
//                toggle.onValueChanged.
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
                        mapNodeSelectSignal.Dispatch(mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id));

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
                return;
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
            selecter_id=(selecter_id+1)%assignedRoleList.Count;
            RoleInfo roleInfo = gameInfo.role_dic[assignedRoleList[selecter_id]];
            mapNodeSelectSignal.Dispatch(mapRootView.NodeAt<MapNavNode>(roleInfo.pos_id));
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
            assignedRoleList.Add(role_id);
        }

        findBtn.interactable = true;
    }

    void OnDestroy()
    {
        mapNodeSelectSignal.RemoveListener(OnMapNodeSelectSignal);

        updateRoleDirectionSignal.RemoveListener(OnUpdateRoleDirectionSignal);

        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);

        actionAnimFinishSignal.RemoveListener(OnActionAnimFinishSignal);
    }
}
