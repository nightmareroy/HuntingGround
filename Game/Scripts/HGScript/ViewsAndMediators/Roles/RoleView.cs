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

    //Material normalNodeMat;
    //Material exploreNodeMat;
    //Material attackNodeMat;
    //Material defendNodeMat;
    //Material moveNodeMat;

    public Transform faceT;
    public MeshRenderer color;

    public Material redMat;
    public Material blueMat;

    public Material lineMaterial;
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

    [Inject]
    public IconSpritesService iconSpritesService { get; set; }

    [Inject]
    public ColorService colorService { get; set; }

    Animator animator;
    MapRootMediator mapRootMediator;
    VectorLine vectorLine;


    bool isWalking = false;

    Vector3 attack_pos1;
    Vector3 attack_pos2;
    Vector3 attack_pos3;

    public void Init()
    {
        mapRootMediator = mainContext.mapRootMediator;
        //normalNodeMat = Resources.Load("map/HexNode/hexMat") as Material;
        //exploreNodeMat = Resources.Load("map/HexNode/hexExploreMat") as Material;
        //attackNodeMat = Resources.Load("map/HexNode/hexAttackMat") as Material;
        //defendNodeMat = Resources.Load("map/HexNode/hexDefendMat") as Material;
        //moveNodeMat = Resources.Load("map/HexNode/hexMoveMat") as Material;

        //lineMaterial=Resources.Load("arrow/ThickLine") as Material;

        string[] ids = gameObject.name.Split('_');
        string role_id = ids[1];
        //Debug.Log(role_id);
        RoleInfo roleInfo=gameInfo.role_dic[role_id];
        
        switch (gameInfo.allplayers_dic[roleInfo.uid].color_index)
        {
            case 0:
                color.material = redMat;
                break;
            case 1:
                color.material = blueMat;
                break;

        }
        

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

    public void DoAttack(int src_pos_id,int des_pos_id)
    {
        float y = transform.position.y;
        //animator.SetTrigger("attack");
        Vector3 src_p = mapRootMediator.mapRootView.GetNodeObj(src_pos_id).transform.position;
        Vector3 des_p = mapRootMediator.mapRootView.GetNodeObj(des_pos_id).transform.position;

        attack_pos1.x = src_p.x * 1.1f - des_p.x * 0.1f;
        attack_pos1.y = y;
        attack_pos1.z = src_p.z * 1.1f - des_p.z * 0.1f;

        attack_pos2.x = src_p.x * 0.7f + des_p.x * 0.3f;
        attack_pos2.y = y;
        attack_pos2.z = src_p.z * 0.7f + des_p.z * 0.3f;

        attack_pos3.x = src_p.x ;
        attack_pos3.y = y;
        attack_pos3.z = src_p.z ;

        Hashtable args = new Hashtable();
        args.Add("time",0.1f);
        args.Add("x", attack_pos1.x);
        args.Add("y", attack_pos1.y);
        args.Add("z", attack_pos1.z);
        args.Add("loopType", iTween.LoopType.none);
        args.Add("easyType", iTween.EaseType.linear);
        args.Add("oncomplete", "OnAttackAnimComplete1");
        iTween.MoveTo(gameObject,args);
    }

    void OnAttackAnimComplete1()
    {
        Hashtable args = new Hashtable();
        args.Add("time", 0.2f);
        args.Add("x", attack_pos2.x);
        args.Add("y", attack_pos2.y);
        args.Add("z", attack_pos2.z);
        args.Add("loopType", iTween.LoopType.none);
        args.Add("easyType", iTween.EaseType.linear);
        args.Add("oncomplete", "OnAttackAnimComplete2");
        iTween.MoveTo(gameObject, args);
    }

    void OnAttackAnimComplete2()
    {
        Hashtable args = new Hashtable();
        args.Add("time", 0.2f);
        args.Add("x", attack_pos3.x);
        args.Add("y", attack_pos3.y);
        args.Add("z", attack_pos3.z);
        args.Add("loopType", iTween.LoopType.none);
        args.Add("easyType", iTween.EaseType.linear);
        args.Add("oncomplete", "OnAttackAnimComplete3");
        iTween.MoveTo(gameObject, args);
    }

    public void DoRoll()
    {
        animator.SetTrigger("roll");
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
        vectorLine.vectorObject.transform.position=new Vector3(0,4.5f,0);
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

    public void SetRoleSize(int weight)
    {
        float size = (float)Math.Pow(weight/6000f, 2f / 3f);//(float)weight / 300f;
        Debug.Log(size);
        faceT.localScale = new Vector3(size, size, size);
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

