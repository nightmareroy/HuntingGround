using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIView:View
{
    //[Inject]
    //public MainContext mainContext { get; set; }

    [Inject]
    public DoBuildingActionAnimSignal doBuildingActionAnimSignal { get; set; }

    [Inject]
    public FlowUpTipSignal flowUpTipSignal { get; set; }

    MainContext mainContext;

    //public GameObject flow_up_er;


    CanvasScaler canvasScaler;

    RectTransform roleUIRectTransform;

    GameObject buildingObj;

    string building_id;

    public void Init(GameObject buildingObj, MainContext mainContext, DoBuildingActionAnimSignal doBuildingActionAnimSignal,string building_id)
    {
        
        

        this.buildingObj = buildingObj;
        this.mainContext = mainContext;
        this.doBuildingActionAnimSignal = doBuildingActionAnimSignal;
        this.building_id = building_id;

        //Debug.Log(doBuildingActionAnimSignal);

        canvasScaler = mainContext.uiCanvas.GetComponent<CanvasScaler>();

        transform.SetParent(mainContext.uiCanvas.transform.FindChild("BuildingUIRoot"));

        roleUIRectTransform = GetComponent<RectTransform>();
        roleUIRectTransform.anchoredPosition3D = Vector3.zero;
        roleUIRectTransform.localScale = Vector3.one;
        roleUIRectTransform.localRotation = Quaternion.identity;


        doBuildingActionAnimSignal.AddListener(OnDoBuildingActionAnimSignal);
    }

    void LateUpdate()
    {
        //        Debug.Log(roleUIRectTransform.anchoredPosition);
        //        Debug.Log(roleObj);
        float s_factor = canvasScaler.referenceResolution.x / (float)Screen.width;
        roleUIRectTransform.anchoredPosition = s_factor * mainContext.uiCamera.WorldToScreenPoint(buildingObj.transform.position);
        //        Debug.Log();
    }

    void OnDoBuildingActionAnimSignal(DoBuildingActionAnimSignal.Param param)
    {
        if (param.building_id == building_id)
        {
            FlowUpTipSignal.Param flowUpTipSignalParam;

            switch (param.type)
            {
                //出现
                case 0:
                    break;
                //消失
                case 1:
                    Destroy(gameObject);
                    break;
                //产出香蕉
                case 2:
                    //flow_up_er.GetComponent<Text>().text = param.banana.ToString();
                    //flow_up_er.GetComponent<Animator>().SetTrigger("banana");
                    flowUpTipSignalParam = new FlowUpTipSignal.Param(transform,FlowUpTipSignal.Type.banana,param.banana);
                    flowUpTipSignal.Dispatch(flowUpTipSignalParam);
                    break;
            }
        }
    }

    void OnDestroy()
    {
        doBuildingActionAnimSignal.RemoveListener(OnDoBuildingActionAnimSignal);
    }
}
