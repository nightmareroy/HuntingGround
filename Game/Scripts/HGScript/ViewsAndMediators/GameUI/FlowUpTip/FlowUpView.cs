using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FlowUpView:MonoBehaviour
{
    



    //public Image image;
    public Text textName;
    public Text text;

    //public Sprite[] sprites;

    ResourceService resourceService;

    Animator animator;

    DGameDataCollection dGameDataCollection;


    public void Init(FlowUpTipSignal.Param param, IconSpritesService iconSpritesService,ResourceService resourceService,DGameDataCollection dGameDataCollection)
    {
        this.resourceService = resourceService;
        this.dGameDataCollection=dGameDataCollection;
        animator = GetComponent<Animator>();

        transform.SetParent(param.parent);
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;


        if (param.value > 0)
        {
            text.text = "+" + param.value.ToString();
        }
        else if (param.value < 0)
        {
            text.text = param.value.ToString(); //"-" + (-1 * param.value).ToString();
        }


        switch (param.type)
        {
            case FlowUpTipSignal.Type.common:
                textName.text = "";
                //textName.color = Color.red;
                text.color = Color.white;
                text.text = param.content;
                break;
            
            case FlowUpTipSignal.Type.blood:
                textName.text = "血糖";
                textName.color = Color.red;
                text.color = Color.red;
                break;
            case FlowUpTipSignal.Type.blood_max:
                textName.text = "血糖上限";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.muscle:
                textName.text = "肌肉";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.fat:
                textName.text = "脂肪";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.inteligent:
                textName.text = "智商";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.amino_acid:
                textName.text = "氨基酸";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.breath:
                textName.text = "肺活量";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.digest:
                textName.text = "肠胃";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.courage:
                textName.text = "勇气";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.life:
                textName.text = "衰老";
                textName.color = Color.white;
                text.color = Color.white;
                break;
            case FlowUpTipSignal.Type.skill:
                textName.text = "战斗技能";
                textName.color = Color.white;
                text.color = Color.white;
                text.text = dGameDataCollection.dSkillCollection.dSkillDic[param.value].name;
                break;
            case FlowUpTipSignal.Type.cook_skill:
                textName.text = "料理技能";
                textName.color = Color.white;
                text.color = Color.white;
                text.text=dGameDataCollection.dCookSkillCollection.dCookSkillDic[param.value].name;
                break;
            case FlowUpTipSignal.Type.meat:
                textName.text = "生肉";
                textName.color = Color.yellow;
                text.color = Color.yellow;
                break;
            case FlowUpTipSignal.Type.banana:
                textName.text = "香蕉";
                textName.color = Color.yellow;
                text.color = Color.yellow;
                break;
            case FlowUpTipSignal.Type.ant:
                textName.text = "白蚁";
                textName.color = Color.yellow;
                text.color = Color.yellow;
                break;
            case FlowUpTipSignal.Type.egg:
                textName.text = "鸟蛋";
                textName.color = Color.yellow;
                text.color = Color.yellow;
                break;
            case FlowUpTipSignal.Type.honey:
                textName.text = "蜂蜜";
                textName.color = Color.yellow;
                text.color = Color.yellow;
                break;
        }

        //animator.SetTrigger("flowup");

        //Debug.Log(iconSpritesService);
        
    }

    public void DestroyItself()
    {
        //Destroy(gameObject);
        //Debug.Log("DestroyItself");
        
        resourceService.Despawn("flowuptip/FlowUpTip",gameObject);
    }

    public void OnDestroy()
    {

    }
}
