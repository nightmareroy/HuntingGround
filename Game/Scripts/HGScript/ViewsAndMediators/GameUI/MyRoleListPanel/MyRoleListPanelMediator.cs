using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

public class MyRoleListPanelMediator : Mediator
{
    [Inject]
    public MyRoleListPanelView myRoleListPannelView{ get; set;}

    [Inject]
    public GameInfo gameInfo { get; set;}

    [Inject]
    public SPlayerInfo sPlayerInfo{ get; set;}

    [Inject]
    public DGameDataCollection dGameDataCollection{ get; set;}

    [Inject]
    public DoRoleActionAnimSignal doRoleActionAnimSignal{ get; set;}



//    List<RoleInfo> myRoleList=new List<RoleInfo>();

    public override void OnRegister()
    {
        StartCoroutine(Init());
    }


    IEnumerator Init()
    {
        //这里等待1帧，因为content size fitter组件必须等一帧才能生效，优化方案http://www.xuanyusong.com/archives/4234，http://www.tuicool.com/articles/AnQv2yB，有时间再来优化
        yield return null;
        myRoleListPannelView.Init();

        doRoleActionAnimSignal.AddListener(OnDoRoleActionAnimSignal);
    }

    void OnDoRoleActionAnimSignal(DoRoleActionAnimSignal.Param param)
    {
        switch (param.type)
        {
            //移动
            case 0:
                break;
            //出现
            case 1:
                myRoleListPannelView.AddRole(param.role_id);
                break;
            //消失
            case 2:
                myRoleListPannelView.DeleteRole(param.role_id);
                break;
            //掉血
            case 3:
                break;
            //回血
            case 4:
                break;

        }
    }

    void OnDestroy()
    {
        doRoleActionAnimSignal.RemoveListener(OnDoRoleActionAnimSignal);
    }
}
