using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using MapNavKit;


public class RoleView:View
{
    GameObject pathRoot;

    Material normalNodeMat;
    Material exploreNodeMat;
    Material attackNodeMat;
    Material defendNodeMat;
    Material moveNodeMat;

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    Animator animator;
    //MapRootMediator mapRootMediator;

    bool isWalking = false;

    public void Init()
    {
        //mapRootMediator = mainContext.mapRootMediator;
        normalNodeMat = Resources.Load("map/HexNode/hexMat") as Material;
        exploreNodeMat = Resources.Load("map/HexNode/hexExploreMat") as Material;
        attackNodeMat = Resources.Load("map/HexNode/hexAttackMat") as Material;
        defendNodeMat = Resources.Load("map/HexNode/hexDefendMat") as Material;
        moveNodeMat = Resources.Load("map/HexNode/hexMoveMat") as Material;

        pathRoot=new GameObject();
        pathRoot.transform.SetParent(transform);
        pathRoot.transform.localPosition = Vector3.zero;

        animator = GetComponent<Animator>();

        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);

    }

    public void DoWalking()
    {
        animator.SetBool("isWalking",true);
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void DoAttack()
    {
        animator.SetTrigger("attack");
    }

    public void DoDefend()
    {
        animator.SetTrigger("defend");
    }


    #region direction path
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startid">入口编号，从上方按照顺时针方向依次为1~6，如果为0表示没有入口，此节点为开始点</param>
    /// <param name="endid">同上，为0表示没有出口，此节点为终结点</param>
    public void GenerateADirectionNode(int lastid, int nextid,int nodeid, int directionid )
    {
        //DirectionInfo directionInfo = gameInfo.curr_direction_dic[roleid];
        //MapRootMediator mapRootMediator = mainContext.mapRootMediator;

        GameObject pathNode = resourceService.Spawn("map/HexNode/HexNode");
        pathNode.transform.SetParent(pathRoot.transform);
        pathNode.transform.position = mainContext.mapRootMediator.mapRootView.GetNodeObj(nodeid).transform.position+new Vector3(0, 5f, 0);

        Renderer rd = pathNode.GetComponent<Renderer>();
        switch (directionid)
        {
            case 1:
                rd.material = moveNodeMat;
                break;
            case 2:
                rd.material = exploreNodeMat;
                break;
            case 3:
                rd.material = attackNodeMat;
                break;
            case 4:
                rd.material = defendNodeMat;
                break;
            default:
                break;
        }
    }

    public void ClearAllDirectionNode()
    {
        List<GameObject> objToBeDespawn = new List<GameObject>();
        for (int i = 0; i < pathRoot.transform.childCount; i++)
        {
            objToBeDespawn.Add(pathRoot.transform.GetChild(i).gameObject);

        }
        foreach (GameObject obj in objToBeDespawn)
        {
            resourceService.Despawn("map/HexNode/HexNode", obj);
        }
    }

    public void SetPathVisible(bool visible)
    {
        pathRoot.SetActive(visible);
    }

    public void OnActionAnimStartSignal()
    {
        //SetPathVisible(false);
        ClearAllDirectionNode();
    }

    public Vector3 GetRelativePosToAnotherNode(int fromid, int toid)
    {

        Transform fromTrans = mainContext.mapRootMediator.mapRootView.GetNodeObj(fromid).transform;
        Transform toTrans = mainContext.mapRootMediator.mapRootView.GetNodeObj(toid).transform;
        return toTrans.position - fromTrans.position;
        
    }

    void OnDestroy()
    {
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
    }

    public int GetRelativeIdToAnotherNode(int fromid, int toid)
    {
        int width=gameInfo.map_info.width;//mapRootMediator.mapRootView.mapHorizontalSize;
        int from_x=fromid%width;
        int from_y=fromid/width;
        //int to_x=toid%width;
        //int to_y=toid/width;

        if(toid==fromid+width)
            return 1;
        else if(toid==fromid-width)
            return 4;

        if (width % 2 == 0||(width%2==1&&from_y%2==0))
        {
            if (from_x % 2 == 0)
            {
                if (toid == fromid + 1)
                    return 3;
                else if (toid == fromid - 1)
                    return 5;
                else if (toid == fromid + width + 1)
                    return 2;
                else// if (toid == fromid + width - 1)
                    return 6;
            }
            else
            {
                if (toid == fromid + 1)
                    return 2;
                else if (toid == fromid - 1)
                    return 6;
                else if (toid == fromid - width + 1)
                    return 3;
                else// if (toid == fromid - width - 1)
                    return 5;
            }
        }
        else
        {
            
            if (from_x % 2 == 1)
            {
                if (toid == fromid + 1)
                    return 3;
                else if (toid == fromid - 1)
                    return 5;
                else if (toid == fromid + width + 1)
                    return 2;
                else// if (toid == fromid + width - 1)
                    return 6;
            }
            else
            {
                if (toid == fromid + 1)
                    return 2;
                else if (toid == fromid - 1)
                    return 6;
                else if (toid == fromid - width + 1)
                    return 3;
                else// if (toid == fromid - width - 1)
                    return 5;
            }
        }
    }
    #endregion
}

