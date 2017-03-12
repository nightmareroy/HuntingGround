using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;
using MapNavKit;
using Vectrosity;


public class RoleView:View
{
//    GameObject pathRoot;

    Material normalNodeMat;
    Material exploreNodeMat;
    Material attackNodeMat;
    Material defendNodeMat;
    Material moveNodeMat;

    Material lineMaterial;
//    Texture2D arrowStart;
//    Texture2D arrowEnd;

    [Inject]
    public ResourceService resourceService { get; set; }

    [Inject]
    public GameInfo gameInfo { get; set; }

    [Inject]
    public MainContext mainContext { get; set; }

    [Inject]
    public ActionAnimStartSignal actionAnimStartSignal { get; set; }

    Animator animator;
    MapRootMediator mapRootMediator;
    VectorLine vectorLine;


    bool isWalking = false;

    public void Init()
    {
        mapRootMediator = mainContext.mapRootMediator;
        normalNodeMat = Resources.Load("map/HexNode/hexMat") as Material;
        exploreNodeMat = Resources.Load("map/HexNode/hexExploreMat") as Material;
        attackNodeMat = Resources.Load("map/HexNode/hexAttackMat") as Material;
        defendNodeMat = Resources.Load("map/HexNode/hexDefendMat") as Material;
        moveNodeMat = Resources.Load("map/HexNode/hexMoveMat") as Material;

        lineMaterial=Resources.Load("arrow/ThickLine") as Material;


//        pathRoot=new GameObject();
//        pathRoot.transform.SetParent(transform);
//        pathRoot.transform.localPosition = Vector3.zero;

        animator = GetComponent<Animator>();

        actionAnimStartSignal.AddListener(OnActionAnimStartSignal);


        if (vectorLine != null)
        {
            VectorLine.Destroy(ref vectorLine);
        }

    }

    public void DoWalking()
    {
        animator.SetBool("isWalking",true);
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void DoAttack(int des_pos_id)
    {
        float y = transform.position.y;
        //animator.SetTrigger("attack");
        //Vector3 src_p = mapRootMediator.mapRootView.GetNodeObj(src_pos_id).transform.position;
        Vector3 des_p = mapRootMediator.mapRootView.GetNodeObj(des_pos_id).transform.position;

        Hashtable args = new Hashtable();
        args.Add("time",0.5f);
        args.Add("x",des_p.x);
        args.Add("y",y);
        args.Add("z", des_p.z);
        args.Add("loopType","pingPong");
        iTween.MoveTo(gameObject,args);
    }

    public void DoDefend()
    {
        animator.SetTrigger("defend");
    }


    public void GenerateArrow(Vector3[] points)
    {
        if (vectorLine != null)
        {
            VectorLine.Destroy(ref vectorLine);
        }


        vectorLine = new VectorLine("line",points,lineMaterial,25f,LineType.Continuous,Joins.Weld);
        vectorLine.endCap = "Arrow";
        vectorLine.layer = LayerMask.NameToLayer("Game");
        vectorLine.vectorObject.transform.position=new Vector3(0,3.5f,0);
        vectorLine.drawTransform = mapRootMediator.transform;
        vectorLine.Draw3D();

    }

    public void ClearArrow()
    {
        if (vectorLine != null)
        {
            VectorLine.Destroy(ref vectorLine);
        }
    }


    public void OnActionAnimStartSignal()
    {
        //SetPathVisible(false);
        ClearArrow();  
    }

    public Vector3 GetRelativePosToAnotherNode(int fromid, int toid)
    {

        Transform fromTrans = mainContext.mapRootMediator.mapRootView.GetNodeObj(fromid).transform;
        Transform toTrans = mainContext.mapRootMediator.mapRootView.GetNodeObj(toid).transform;
        return toTrans.position - fromTrans.position;
        
    }

    void OnDestroy()
    {
        Debug.Log("destroy "+gameObject.name+" view");
        if (vectorLine != null)
        {
            VectorLine.Destroy(ref vectorLine);
        }
        actionAnimStartSignal.RemoveListener(OnActionAnimStartSignal);
    }

}

